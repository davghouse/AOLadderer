#include <algorithm>
#include <iostream>
#include "Ladder.h"

using std::vector; using std::string;
using std::endl; using std::cout;
using std::max; using std::sort;
using namespace laddererHelper;

Ladder::Ladder(const Config& r, const Stats& s) : fReqs(r), stats(s) {}

void Ladder::heightOne(const vector<Slot>& ladderSlots)
{
  double maxAvgQL = 0;
  bool increasingAvgQL = true;
  vector<Implant> limps;
  while(increasingAvgQL && limps.size() != 10){
    increasingAvgQL = false;
    limps.push_back(Implant());
    vector<Slot>::const_iterator slotPosition = ladderSlots.begin();
    int vectorPosition = 0;
    int indexPosition = 0;
    // choose limp:
    // choose slot
    vector<Slot>::const_iterator it;
    for(it = ladderSlots.begin(); it != ladderSlots.end(); it++){
      if(slotTaken(it->name(), limps)){
        continue;
      }
      // determine if this slot has a final implant occupying it
      bool mustRemove = true;
      if(fReqs[slotToInt(it->name())].abi() == "abi"){
        mustRemove = false;
      }
      // choose implant vector
      Slot::size_type i;
      for(i = 0; i != it->size(); i++){
        // choose implant
        int max = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
          // looking at implants that have the same cluster specification, but different abi req
          // consider only one; the one for which our abi stats value is highest
          if(stats.getAbiFromName((*it)[i][j].abi()) > stats.getAbiFromName((*it)[i][max].abi())){
            max = j;
          }
        }
        // test implant
        limps[limps.size() - 1] = (*it)[i][max];
        limps[limps.size() - 1].setRemove(mustRemove);
        Ladder tempLadder(fReqs, stats);
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
    if(fReqs[slotToInt(slotPosition->name())].abi() == "abi"){
      limps[limps.size() - 1].setRemove(false);
    }
  }
  if(!increasingAvgQL){
    // didn't find a useful ladder imp last time, so remove
    limps.resize(limps.size() - 1);
  }

  Ladder tempLadder(fReqs,stats);
  tempLadder.equipLimps(limps);
  tempLadder.heightZero();
  process = tempLadder.process;
  // makes output easier
  if(process.size() == 1){
    process.push_back(process[0]);
  }
  stats = tempLadder.stats;
}


void Ladder::equipLimps(const vector<Implant>& limps)
{
  for(vector<Implant>::size_type i = 0; i != limps.size(); i++){
    int QL = stats.updateStats(limps[i]);
    if(limps[i].mustRemove()){
      config.updateConfig(Implant(QL, limps[i].slot(), limps[i].abi(), limps[i].shi(), limps[i].bri(), limps[i].fad(), true));
    }
    else{
      config.updateConfig(Implant(QL, limps[i].slot(), limps[i].abi(), limps[i].shi(), limps[i].bri(), limps[i].fad(), true, false));
    }
  }
}

void Ladder::heightZero()
{
  findLimps();
  vector<uint> ordered;
  ordering(ordered);
  for(vector<uint>::size_type i = 0; i != ordered.size(); i++){
    int j = ordered[i];
    int QL = stats.updateStats(rlimps[j]);
    config.updateConfig(Implant(QL, rlimps[j].slot(), rlimps[j].abi(), rlimps[j].shi(), rlimps[j].bri(), rlimps[j].fad()));
  }
  for(Config::size_type i = 0; i != fReqs.size(); i++){
    if(!fReqs[i].ladder() && !config[i].isLocked()){
      int QL = stats.updateStats(fReqs[i]);
      config.updateConfig(Implant(QL, fReqs[i].slot(), fReqs[i].abi(), fReqs[i].shi(), fReqs[i].bri(), fReqs[i].fad()));
    }
  }
  unequipLimps();
}

void Ladder::findLimps()
{
  for(Config::size_type i = 0; i != fReqs.size(); i++)
    if(fReqs[i].ladder() && !config[i].isLocked())
      rlimps.push_back(fReqs[i]);
}

void Ladder::ordering(vector<uint>& ordered)
{
  for(vector<Implant>::size_type i = 0; i != rlimps.size(); i++)
    ordered.push_back(i);
  vector<uint> partial;
  run_ordering(ordered, partial);
}

void Ladder::run_ordering(vector<uint>& best, vector<uint>& partial)
{
  if(partial.size() == rlimps.size()){
    if(!compare(best, partial))
      best = partial;
    return;
  }
  // recursively evaluate all permutations of the implants that happen to be useful for laddering
  for(vector<Implant>::size_type i = 0; i != rlimps.size(); i++){
    bool avail = true;
    for(vector<uint>::size_type j = 0; j != partial.size(); j++){
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
  Stats cStats = stats;
  for(vector<uint>::size_type i = 0; i != indices.size(); i++){
    avgQL += cStats.updateStats(rlimps[indices[i]]);
  }
  for(vector<uint>::size_type i = 0; i != fReqs.size(); i++){
    if(!fReqs[i].ladder()){
     avgQL += cStats.updateStats(fReqs[i]);
    }
  }
  return avgQL/(fReqs.size());
}

void Ladder::unequipLimps()
{
  process.push_back(config);
  // create a temp config for correct ordering
  bool limpFound = false;
  Config tempConfig;
  // go through the config, removing limps in reverse order, putting in the required imp immediately
  // do not remove limps if their remove flag is false
  for(vector<int>::const_reverse_iterator ri = config.rbegin(); ri != config.rend(); ri++){
    if(config[*ri].isLocked() && config[*ri].mustRemove()){
      limpFound = true;
      // remove it
      stats.updateStats(config[*ri], false);
      // update stats/config with required implant
      int QL = stats.updateStats(fReqs[*ri]);
      tempConfig.updateConfig(Implant(QL, fReqs[*ri].slot(), fReqs[*ri].abi(), fReqs[*ri].shi(), fReqs[*ri].bri(), fReqs[*ri].fad()));
    }
  }
  // include the implants equipped previously
  if(limpFound){
    for(vector<int>::const_iterator it = config.begin(); it != config.end(); it++){
      if(!config[*it].isLocked())
        tempConfig.updateConfig(config[*it]);
    }
    process.push_back(tempConfig);
  }
}

double Ladder::avgQL() const
{
  if(!process.size())
    return 0;
  return process[process.size() - 1].avgQL();
}
