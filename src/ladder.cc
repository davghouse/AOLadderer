#include <algorithm>
#include <iostream>
#include "ladder.h"

using std::vector; using std::string;
using std::endl;
using std::max; using std::sort;
using namespace ladder_helper;

Ladder::Ladder(const ImplantConfiguration& required_config, const CharacterStats& stats)
  : required_config_(required_config), stats_(stats) {}

void Ladder::HeightOne(const vector<LadderSlot>& ladder_slots)
{
  double max_avg_ql = 0;
  bool increasing_avg_ql = true;
  vector<Implant> ladder_implants;
  // Look for ladder implants.
  while(increasing_avg_ql && ladder_implants.size() != 10){
    increasing_avg_ql = false;
    ladder_implants.push_back(Implant());
    vector<LadderSlot>::const_iterator slot_pos = ladder_slots.begin();
    int subset_pos = 0;
    int implant_pos = 0;
    // Choose ladder implant:
    // Choose slot.
    vector<LadderSlot>::const_iterator it;
    for(it = ladder_slots.begin(); it != ladder_slots.end(); ++it){
      if(SlotTaken(it->name(), ladder_implants)){
        continue;
      }
      // Determine if this slot has a final implant occupying it.
      bool remove = true;
      if(required_config_[SlotNameToInt(it->name())].ability_name() == "abi"){
        remove = false;
      }
      // Choose implant subset within slot (subset: same important clusters, different ability req.)
      LadderSlot::size_type i;
      for(i = 0; i != it->size(); ++i){
        // Choose implant for which our ability is highest.
        int best = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); ++j){
          if(stats_.ability_from_name((*it)[i][j].ability_name()) > stats_.ability_from_name((*it)[i][best].ability_name())){
            best = j;
          }
        }
        // Test implant.
        ladder_implants[ladder_implants.size() - 1] = (*it)[i][best];
        ladder_implants[ladder_implants.size() - 1].set_remove(remove);
        Ladder trial_ladder(required_config_, stats_);
        trial_ladder.EquipLadderImplants(ladder_implants);
        trial_ladder.HeightZero();
        double trial_avg_ql = trial_ladder.AverageQL();
        // + .01 added because optimizations in release mode were causing rounding problems.
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
    // Didn't find a useful ladder implant last time, so remove last added.
    ladder_implants.pop_back();
  }

  Ladder final_ladder(required_config_,stats_);
  final_ladder.EquipLadderImplants(ladder_implants);
  final_ladder.HeightZero();
  process_ = final_ladder.process_;
  // Makes output easier.
  if(process_.size() == 1){
    process_.push_back(process_[0]);
  }
  stats_ = final_ladder.stats_;
}


void Ladder::EquipLadderImplants(const vector<Implant>& ladder_implants)
{
  for(vector<Implant>::size_type i = 0; i != ladder_implants.size(); ++i){
    int slot_int = ladder_implants[i].slot_int();
    int ql = stats_.UpdateStats(ladder_implants[i]);
    working_config_.UpdateConfig(ladder_implants[i]);
    working_config_[slot_int].set_ql(ql);
    // Lock the implant (close the slot). To be removed during UnequipLadderImplants.
    working_config_[slot_int].set_lock(true);
  }
}

// When this function is called, EquipLadderImplants has already been called from HeightOne,
// populating this Ladder's working_config_ with the accessory ladder implants.
void Ladder::HeightZero()
{
  FindRequiredLadderImplants();
  vector<uint> ordered;
  // Find optimal ordering of the required ladder implants.
  Order(ordered);
  // Equip those implants.
  for(vector<uint>::size_type i = 0; i != ordered.size(); ++i){
    int j = ordered[i];
    int slot_int = required_ladder_implants_[j].slot_int();
    int ql = stats_.UpdateStats(required_ladder_implants_[j]);
    working_config_.UpdateConfig(required_ladder_implants_[j]);
    working_config_[slot_int].set_ql(ql);
  }
  // Equip the remaining required implants whose slots are unoccupied.
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder() && !working_config_[i].lock()){
      int slot_int = required_config_[i].slot_int();
      int ql = stats_.UpdateStats(required_config_[i]);
      working_config_.UpdateConfig(required_config_[i]);
      working_config_[slot_int].set_ql(ql);
    }
  }
  UnequipLadderImplants(); // And equip the required implants that go in their spots.
}

void Ladder::FindRequiredLadderImplants()
{
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i)
    // Implant must be required, used for laddering, and slot must be free.
    if(required_config_[i].used_to_ladder() && !working_config_[i].lock())
      required_ladder_implants_.push_back(required_config_[i]);
}

void Ladder::Order(vector<uint>& order)
{
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i)
    order.push_back(i);
  vector<uint> current;
  RunOrder(order, current);
}

void Ladder::RunOrder(vector<uint>& best, vector<uint>& current)
{
  if(current.size() == required_ladder_implants_.size()){
    if(!Compare(best, current))
      best = current;
    return;
  }
  // Recursively evaluate all permutations of the implants that happen to be useful for laddering.
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i){
    bool slot_empty = true;
    for(vector<uint>::size_type j = 0; j != current.size(); ++j){
      if(current[j] == i){
        slot_empty = false;
        break;
      }
    }
    if(slot_empty){
      current.push_back(i);
      // RECURSE -- current becomes current plus next available.
      RunOrder(best, current);
      // Pass by reference, then pop_back when done to continue recursing at this level.
      current.pop_back();
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
  CharacterStats temporary_stats = stats_;
  for(vector<uint>::size_type i = 0; i != order.size(); ++i){
    avg_ql += temporary_stats.UpdateStats(required_ladder_implants_[order[i]]);
  }
  for(vector<uint>::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder()){
     avg_ql += temporary_stats.UpdateStats(required_config_[i]);
    }
  }
  return avg_ql/(required_config_.size());
}

void Ladder::UnequipLadderImplants()
{
  process_.push_back(working_config_);
  bool ladder_implant_found = false;
  ImplantConfiguration temp_config;
  // Go through the config, removing ladder implants in reverse order, putting in the
  // required implant immediately. Ladder implants not in the way of a final implant should
  // not be touched.
  for(vector<int>::const_reverse_iterator ri = working_config_.rbegin(); ri != working_config_.rend(); ++ri){
    if(working_config_[*ri].lock() && working_config_[*ri].remove()){
      ladder_implant_found = true;
      // remove it
      stats_.UpdateStats(working_config_[*ri], false);
      // Update stats/config with required implant.
      int slot_int = required_config_[*ri].slot_int();
      int ql = stats_.UpdateStats(required_config_[*ri]);
      temp_config.UpdateConfig(required_config_[*ri]);
      temp_config[slot_int].set_ql(ql);
    }
  }
  // Include the implants equipped previously for use in GUI's Step Two output.
  if(ladder_implant_found){
    for(vector<int>::const_iterator it = working_config_.begin(); it != working_config_.end(); ++it){
      if(!working_config_[*it].lock())
        temp_config.UpdateConfig(working_config_[*it]);
    }
    // ?Take outside of if statement and remove process.push_back from HeightOne?
    process_.push_back(temp_config);
  }
}

double Ladder::AverageQL() const
{
  if(!process_.size())
    return 0;
  return process_[process_.size() - 1].AverageQL();
}
