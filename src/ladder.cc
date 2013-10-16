#include <algorithm>
#include <iostream>
#include "ladder.h"

using std::vector; using std::string;
using std::endl; using std::cout;
using std::max; using std::sort;
using namespace ladder_helper;

Ladder::Ladder(const ImplantConfiguration& r, const Stats& s) : required_config_(r), stats_(s) {}

void Ladder::heightOne(const vector<LadderSlot>& ladderSlots)
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
      if(slotTaken(it->name(), limps)){
        continue;
      }
      // determine if this slot has a final implant occupying it
      bool mustRemove = true;
      if(required_config_[slotToInt(it->name())].abi() == "abi"){
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
          if(stats_.getAbiFromName((*it)[i][j].abi()) > stats_.getAbiFromName((*it)[i][max].abi())){
            max = j;
          }
        }
        // test implant
        limps[limps.size() - 1] = (*it)[i][max];
        limps[limps.size() - 1].setRemove(mustRemove);
        Ladder tempLadder(required_config_, stats_);
        tempLadder.equipLimps(limps);
        tempLadder.heightZero();
        double tempAvgQL = tempLadder.avgQL();
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
    if(required_config_[slotToInt(slotPosition->name())].abi() == "abi"){
      limps[limps.size() - 1].setRemove(false);
    }
  }
  if(!increasingAvgQL){
    // didn't find a useful ladder imp last time, so remove
    limps.resize(limps.size() - 1);
  }

  Ladder tempLadder(required_config_,stats_);
  tempLadder.equipLimps(limps);
  tempLadder.heightZero();
  process_ = tempLadder.process_;
  // makes output easier
  if(process_.size() == 1){
    process_.push_back(process_[0]);
  }
  stats_ = tempLadder.stats_;
}


void Ladder::equipLimps(const vector<Implant>& limps)
{
  for(vector<Implant>::size_type i = 0; i != limps.size(); ++i){
    int QL = stats_.updateStats(limps[i]);
    if(limps[i].mustRemove()){
      working_config_.updateConfig(Implant(QL, limps[i].slot(), limps[i].abi(), limps[i].shi(), limps[i].bri(), limps[i].fad(), true));
    }
    else{
      working_config_.updateConfig(Implant(QL, limps[i].slot(), limps[i].abi(), limps[i].shi(), limps[i].bri(), limps[i].fad(), true, false));
    }
  }
}

void Ladder::heightZero()
{
  findLimps();
  vector<uint> ordered;
  ordering(ordered);
  for(vector<uint>::size_type i = 0; i != ordered.size(); ++i){
    int j = ordered[i];
    int QL = stats_.updateStats(required_ladder_implants_[j]);
    working_config_.updateConfig(Implant(QL, required_ladder_implants_[j].slot(), required_ladder_implants_[j].abi(), required_ladder_implants_[j].shi(), required_ladder_implants_[j].bri(), required_ladder_implants_[j].fad()));
  }
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].ladder() && !working_config_[i].isLocked()){
      int QL = stats_.updateStats(required_config_[i]);
      working_config_.updateConfig(Implant(QL, required_config_[i].slot(), required_config_[i].abi(), required_config_[i].shi(), required_config_[i].bri(), required_config_[i].fad()));
    }
  }
  unequipLimps();
}

void Ladder::findLimps()
{
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i)
    if(required_config_[i].ladder() && !working_config_[i].isLocked())
      required_ladder_implants_.push_back(required_config_[i]);
}

void Ladder::ordering(vector<uint>& ordered)
{
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i)
    ordered.push_back(i);
  vector<uint> partial;
  run_ordering(ordered, partial);
}

void Ladder::run_ordering(vector<uint>& best, vector<uint>& partial)
{
  if(partial.size() == required_ladder_implants_.size()){
    if(!compare(best, partial))
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
      run_ordering(best, partial);
      // pass by reference, then resize when done to continue recursing at this level
      partial.resize(old_size);
    }
  }
}

inline bool Ladder::compare(const vector<uint>& best, const vector<uint>& trial) const
{
  if(!best.size())
    return false;
  return find_AvgQL(best) >= find_AvgQL(trial);
}

double Ladder::find_AvgQL(const vector<uint>& indices) const
{
  double avgQL = 0;
  Stats cStats = stats_;
  for(vector<uint>::size_type i = 0; i != indices.size(); ++i){
    avgQL += cStats.updateStats(required_ladder_implants_[indices[i]]);
  }
  for(vector<uint>::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].ladder()){
     avgQL += cStats.updateStats(required_config_[i]);
    }
  }
  return avgQL/(required_config_.size());
}

void Ladder::unequipLimps()
{
  process_.push_back(working_config_);
  // create a temp config for correct ordering
  bool limpFound = false;
  ImplantConfiguration tempConfig;
  // go through the config, removing limps in reverse order, putting in the required imp immediately
  // do not remove limps if their remove flag is false
  for(vector<int>::const_reverse_iterator ri = working_config_.rbegin(); ri != working_config_.rend(); ++ri){
    if(working_config_[*ri].isLocked() && working_config_[*ri].mustRemove()){
      limpFound = true;
      // remove it
      stats_.updateStats(working_config_[*ri], false);
      // update stats/config with required implant
      int QL = stats_.updateStats(required_config_[*ri]);
      tempConfig.updateConfig(Implant(QL, required_config_[*ri].slot(), required_config_[*ri].abi(), required_config_[*ri].shi(), required_config_[*ri].bri(), required_config_[*ri].fad()));
    }
  }
  // include the implants equipped previously
  if(limpFound){
    for(vector<int>::const_iterator it = working_config_.begin(); it != working_config_.end(); ++it){
      if(!working_config_[*it].isLocked())
        tempConfig.updateConfig(working_config_[*it]);
    }
    process_.push_back(tempConfig);
  }
}

double Ladder::avgQL() const
{
  if(!process_.size())
    return 0;
  return process_[process_.size() - 1].avgQL();
}
