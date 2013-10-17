#include <algorithm>
#include <iostream>
#include "ladder.h"

using std::vector; using std::string;
using std::endl; using std::cout;
using std::max; using std::sort;
using namespace ladder_helper;

Ladder::Ladder(const ImplantConfiguration& required_config, const Stats& stats)
  : required_config_(required_config), stats_(stats) {}

void Ladder::HeightOne(const vector<LadderSlot>& ladder_slots)
{
  double max_avg_ql = 0;
  bool increasing_avg_ql = true;
  vector<Implant> ladder_implants;
  while(increasing_avg_ql && ladder_implants.size() != 10){
    increasing_avg_ql = false;
    ladder_implants.push_back(Implant());
    vector<LadderSlot>::const_iterator slot_pos = ladder_slots.begin();
    int subset_pos = 0;
    int implant_pos = 0;
    // choose limp:
    // choose slot
    vector<LadderSlot>::const_iterator it;
    for(it = ladder_slots.begin(); it != ladder_slots.end(); ++it){
      if(SlotTaken(it->name(), ladder_implants)){
        continue;
      }
      // determine if this slot has a final implant occupying it
      bool remove = true;
      if(required_config_[SlotNameToInt(it->name())].ability_name() == "abi"){
        remove = false;
      }
      // choose implant subset within slot
      LadderSlot::size_type i;
      for(i = 0; i != it->size(); ++i){
        // choose implant for which our ability is highest
        int best = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); ++j){
          // looking at implants that have the same cluster specification, but different abi req
          // consider only one; the one for which our abi stats value is highest
          if(stats_.ability_from_name((*it)[i][j].ability_name()) > stats_.ability_from_name((*it)[i][best].ability_name())){
            best = j;
          }
        }
        // test implant
        ladder_implants[ladder_implants.size() - 1] = (*it)[i][best];
        ladder_implants[ladder_implants.size() - 1].set_remove(remove);
        Ladder trial_ladder(required_config_, stats_);
        trial_ladder.EquipLadderImplants(ladder_implants);
        trial_ladder.HeightZero();
        double trial_avg_ql = trial_ladder.AverageQL();
        // + .01 added because optimizations in release mode were causing rounding problems
        if(trial_avg_ql > max_avg_ql + .01){
          increasing_avg_ql = true;
          max_avg_ql = trial_avg_ql;
          slot_pos = it;
          subset_pos = i;
          implant_pos = best;
        }
      }
    }
    ladder_implants[ladder_implants.size() - 1] = (*slot_pos)[subset_pos][implant_pos];
    if(required_config_[SlotNameToInt(slot_pos->name())].ability_name() == "abi"){
      ladder_implants[ladder_implants.size() - 1].set_remove(false);
    }
  }
  if(!increasing_avg_ql){
    // didn't find a useful ladder implant last time, so remove
    ladder_implants.resize(ladder_implants.size() - 1);
  }

  Ladder final_ladder(required_config_,stats_);
  final_ladder.EquipLadderImplants(ladder_implants);
  final_ladder.HeightZero();
  process_ = final_ladder.process_;
  // makes output easier
  if(process_.size() == 1){
    process_.push_back(process_[0]);
  }
  stats_ = final_ladder.stats_;
}


void Ladder::EquipLadderImplants(const vector<Implant>& ladder_implants)
{
  for(vector<Implant>::size_type i = 0; i != ladder_implants.size(); ++i){
    int ql = stats_.UpdateStats(ladder_implants[i]);
    if(ladder_implants[i].remove()){
      working_config_.UpdateConfig(Implant(ql, ladder_implants[i].slot_name(), ladder_implants[i].ability_name(),
                                           ladder_implants[i].shining_abbr(), ladder_implants[i].bright_abbr(),
                                           ladder_implants[i].faded_abbr(), true));
    }
    else{
      working_config_.UpdateConfig(Implant(ql, ladder_implants[i].slot_name(), ladder_implants[i].ability_name(),
                                           ladder_implants[i].shining_abbr(), ladder_implants[i].bright_abbr(),
                                           ladder_implants[i].faded_abbr(), true, false));
    }
  }
}

void Ladder::HeightZero()
{
  FindLadderImplants();
  vector<uint> ordered;
  Ordering(ordered);
  for(vector<uint>::size_type i = 0; i != ordered.size(); ++i){
    int j = ordered[i];
    int ql = stats_.UpdateStats(required_ladder_implants_[j]);
    working_config_.UpdateConfig(Implant(ql, required_ladder_implants_[j].slot_name(),
                                         required_ladder_implants_[j].ability_name(),
                                         required_ladder_implants_[j].shining_abbr(),
                                         required_ladder_implants_[j].bright_abbr(),
                                         required_ladder_implants_[j].faded_abbr()));
  }
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder() && !working_config_[i].lock()){
      int ql = stats_.UpdateStats(required_config_[i]);
      working_config_.UpdateConfig(Implant(ql, required_config_[i].slot_name(),
                                           required_config_[i].ability_name(),
                                           required_config_[i].shining_abbr(),
                                           required_config_[i].bright_abbr(),
                                           required_config_[i].faded_abbr()));
    }
  }
  UnequipLadderImplants();
}

void Ladder::FindLadderImplants()
{
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i)
    if(required_config_[i].used_to_ladder() && !working_config_[i].lock())
      required_ladder_implants_.push_back(required_config_[i]);
}

void Ladder::Ordering(vector<uint>& order)
{
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i)
    order.push_back(i);
  vector<uint> current;
  RunOrdering(order, current);
}

void Ladder::RunOrdering(vector<uint>& best, vector<uint>& current)
{
  if(current.size() == required_ladder_implants_.size()){
    if(!Compare(best, current))
      best = current;
    return;
  }
  // recursively evaluate all permutations of the implants that happen to be useful for laddering
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i){
    bool slot_empty = true;
    for(vector<uint>::size_type j = 0; j != current.size(); ++j){
      if(current[j] == i){
        slot_empty = false;
        break;
      }
    }
    if(slot_empty){
      vector<uint>::size_type old_size = current.size();
      current.push_back(i);
      // RECURSE -- current will be previous, plus next available
      RunOrdering(best, current);
      // pass by reference, then resize when done to continue recursing at this level
      current.resize(old_size);
    }
  }
}

inline bool Ladder::Compare(const vector<uint>& best, const vector<uint>& current) const
{
  if(!best.size())
    return false;
  return AverageQLFromOrdering(best) >= AverageQLFromOrdering(current);
}

double Ladder::AverageQLFromOrdering(const vector<uint>& order) const
{
  double avg_ql = 0;
  Stats current_stats = stats_;
  for(vector<uint>::size_type i = 0; i != order.size(); ++i){
    avg_ql += current_stats.UpdateStats(required_ladder_implants_[order[i]]);
  }
  for(vector<uint>::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder()){
     avg_ql += current_stats.UpdateStats(required_config_[i]);
    }
  }
  return avg_ql/(required_config_.size());
}

void Ladder::UnequipLadderImplants()
{
  process_.push_back(working_config_);
  // create a temp config for correct ordering
  bool ladder_implant_found = false;
  ImplantConfiguration temp_config;
  // go through the config, removing limps in reverse order, putting in the required implant immediately
  // do not remove limps if their remove flag is false
  for(vector<int>::const_reverse_iterator ri = working_config_.rbegin(); ri != working_config_.rend(); ++ri){
    if(working_config_[*ri].lock() && working_config_[*ri].remove()){
      ladder_implant_found = true;
      // remove it
      stats_.UpdateStats(working_config_[*ri], false);
      // update stats/config with required implant
      int ql = stats_.UpdateStats(required_config_[*ri]);
      temp_config.UpdateConfig(Implant(ql, required_config_[*ri].slot_name(),
                              required_config_[*ri].ability_name(),
                              required_config_[*ri].shining_abbr(),
                              required_config_[*ri].bright_abbr(),
                              required_config_[*ri].faded_abbr()));
    }
  }
  // include the implants equipped previously
  if(ladder_implant_found){
    for(vector<int>::const_iterator it = working_config_.begin(); it != working_config_.end(); ++it){
      if(!working_config_[*it].lock())
        temp_config.UpdateConfig(working_config_[*it]);
    }
    process_.push_back(temp_config);
  }
}

double Ladder::AverageQL() const
{
  if(!process_.size())
    return 0;
  return process_[process_.size() - 1].AverageQL();
}
