#include <algorithm>
#include <iostream>
#include "ladder.h"

using std::vector; using std::string;
using std::endl; using std::cout;
using std::max; using std::sort;
using namespace ladder_helper;

Ladder::Ladder(const ImplantConfiguration& r, const Stats& s) : required_config_(r), stats_(s) {}

void Ladder::HeightOne(const vector<LadderSlot>& ladderSlots)
{
  double maxAvgQL = 0;
  bool increasingAvgQL = true;
  vector<Implant> limps;
  while(increasingAvgQL && limps.size() != 10){
    increasingAvgQL = false;
    limps.push_back(Implant());
    vector<LadderSlot>::const_iterator slotPosition = ladderSlots.begin();
    int vectorPosition = 0;
    int indexPosition = 0;
    // choose limp:
    // choose slot
    vector<LadderSlot>::const_iterator it;
    for(it = ladderSlots.begin(); it != ladderSlots.end(); ++it){
      if(SlotTaken(it->name(), limps)){
        continue;
      }
      // determine if this slot has a final implant occupying it
      bool mustRemove = true;
      if(required_config_[SlotNameToInt(it->name())].ability_name() == "abi"){
        mustRemove = false;
      }
      // choose implant vector
      LadderSlot::size_type i;
      for(i = 0; i != it->size(); ++i){
        // choose implant
        int max = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); ++j){
          // looking at implants that have the same cluster specification, but different abi req
          // consider only one; the one for which our abi stats value is highest
          if(stats_.get_ability_from_name((*it)[i][j].ability_name()) > stats_.get_ability_from_name((*it)[i][max].ability_name())){
            max = j;
          }
        }
        // test implant
        limps[limps.size() - 1] = (*it)[i][max];
        limps[limps.size() - 1].set_remove(mustRemove);
        Ladder tempLadder(required_config_, stats_);
        tempLadder.EquipLadderImplants(limps);
        tempLadder.HeightZero();
        double tempAvgQL = tempLadder.AverageQL();
        // + .01 added because optimizations in release mode were causing rounding problems
        if(tempAvgQL > maxAvgQL + .01){
          increasingAvgQL = true;
          maxAvgQL = tempAvgQL;
          slotPosition = it;
          vectorPosition = i;
          indexPosition = max;
        }
      }
    }
    limps[limps.size() - 1] = (*slotPosition)[vectorPosition][indexPosition];
    if(required_config_[SlotNameToInt(slotPosition->name())].ability_name() == "abi"){
      limps[limps.size() - 1].set_remove(false);
    }
  }
  if(!increasingAvgQL){
    // didn't find a useful ladder imp last time, so remove
    limps.resize(limps.size() - 1);
  }

  Ladder tempLadder(required_config_,stats_);
  tempLadder.EquipLadderImplants(limps);
  tempLadder.HeightZero();
  process_ = tempLadder.process_;
  // makes output easier
  if(process_.size() == 1){
    process_.push_back(process_[0]);
  }
  stats_ = tempLadder.stats_;
}


void Ladder::EquipLadderImplants(const vector<Implant>& limps)
{
  for(vector<Implant>::size_type i = 0; i != limps.size(); ++i){
    int QL = stats_.UpdateStats(limps[i]);
    if(limps[i].remove()){
      working_config_.UpdateConfig(Implant(QL, limps[i].slot_name(), limps[i].ability_name(),
                                           limps[i].shining_abbr(), limps[i].bright_abbr(),
                                           limps[i].faded_abbr(), true));
    }
    else{
      working_config_.UpdateConfig(Implant(QL, limps[i].slot_name(), limps[i].ability_name(),
                                           limps[i].shining_abbr(), limps[i].bright_abbr(),
                                           limps[i].faded_abbr(), true, false));
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
    int QL = stats_.UpdateStats(required_ladder_implants_[j]);
    working_config_.UpdateConfig(Implant(QL, required_ladder_implants_[j].slot_name(),
                                         required_ladder_implants_[j].ability_name(),
                                         required_ladder_implants_[j].shining_abbr(),
                                         required_ladder_implants_[j].bright_abbr(),
                                         required_ladder_implants_[j].faded_abbr()));
  }
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder() && !working_config_[i].lock()){
      int QL = stats_.UpdateStats(required_config_[i]);
      working_config_.UpdateConfig(Implant(QL, required_config_[i].slot_name(),
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

void Ladder::Ordering(vector<uint>& ordered)
{
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i)
    ordered.push_back(i);
  vector<uint> partial;
  RunOrdering(ordered, partial);
}

void Ladder::RunOrdering(vector<uint>& best, vector<uint>& partial)
{
  if(partial.size() == required_ladder_implants_.size()){
    if(!Compare(best, partial))
      best = partial;
    return;
  }
  // recursively evaluate all permutations of the implants that happen to be useful for laddering
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i){
    bool avail = true;
    for(vector<uint>::size_type j = 0; j != partial.size(); ++j){
      if(partial[j] == i){
        avail = false;
        break;
      }
    }
    if(avail){
      vector<uint>::size_type old_size = partial.size();
      partial.push_back(i);
      // RECURSE -- partial will be previous, plus next available
      RunOrdering(best, partial);
      // pass by reference, then resize when done to continue recursing at this level
      partial.resize(old_size);
    }
  }
}

inline bool Ladder::Compare(const vector<uint>& best, const vector<uint>& trial) const
{
  if(!best.size())
    return false;
  return AverageQLFromOrdering(best) >= AverageQLFromOrdering(trial);
}

double Ladder::AverageQLFromOrdering(const vector<uint>& indices) const
{
  double avgQL = 0;
  Stats cStats = stats_;
  for(vector<uint>::size_type i = 0; i != indices.size(); ++i){
    avgQL += cStats.UpdateStats(required_ladder_implants_[indices[i]]);
  }
  for(vector<uint>::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder()){
     avgQL += cStats.UpdateStats(required_config_[i]);
    }
  }
  return avgQL/(required_config_.size());
}

void Ladder::UnequipLadderImplants()
{
  process_.push_back(working_config_);
  // create a temp config for correct ordering
  bool limpFound = false;
  ImplantConfiguration tempConfig;
  // go through the config, removing limps in reverse order, putting in the required imp immediately
  // do not remove limps if their remove flag is false
  for(vector<int>::const_reverse_iterator ri = working_config_.rbegin(); ri != working_config_.rend(); ++ri){
    if(working_config_[*ri].lock() && working_config_[*ri].remove()){
      limpFound = true;
      // remove it
      stats_.UpdateStats(working_config_[*ri], false);
      // update stats/config with required implant
      int QL = stats_.UpdateStats(required_config_[*ri]);
      tempConfig.UpdateConfig(Implant(QL, required_config_[*ri].slot_name(),
                              required_config_[*ri].ability_name(),
                              required_config_[*ri].shining_abbr(),
                              required_config_[*ri].bright_abbr(),
                              required_config_[*ri].faded_abbr()));
    }
  }
  // include the implants equipped previously
  if(limpFound){
    for(vector<int>::const_iterator it = working_config_.begin(); it != working_config_.end(); ++it){
      if(!working_config_[*it].lock())
        tempConfig.UpdateConfig(working_config_[*it]);
    }
    process_.push_back(tempConfig);
  }
}

double Ladder::AverageQL() const
{
  if(!process_.size())
    return 0;
  return process_[process_.size() - 1].AverageQL();
}
