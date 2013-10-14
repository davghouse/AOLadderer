#include <algorithm>
#include <iostream>
#include "Ladder.h"

using std::vector; using std::string;
using std::ostream;
using std::endl; using std::cout;
using std::max; using std::sort;

int permcount = 0;
int counter = 0;

Ladder::Ladder(const Config& r, const Stats& s)
{
  fReqs = r;
  stats = s;
}
void Ladder::findLimps(){
  for(Config::size_type i = 0; i != fReqs.size(); i++){
    if(fReqs[i].ladder() && !config[i].isLocked()){
      rlimps.push_back(fReqs[i]);
    }
  }
}

void Ladder::out(ostream& out) const
{
  for(vector<Config>::const_iterator it = process.begin();
      it != process.end(); it++){
    it->out(out);
    cout << endl;
  }
  cout << "avgQL = " << process[process.size() - 1].avgQL() << endl;
  cout << "maxQL = " << process[process.size() - 1].maxQL() << endl;
  out << endl;
}

// printing out config in order
void Ladder::zeroOut(ostream& out) const
{
  for(vector<Config>::const_iterator it = process.begin();
      it != process.end(); it++){
    it->zeroOut(out);
    cout << endl;
  }
  cout << "avgQL = " << process[process.size() - 1].avgQL() << endl;
  cout << "maxQL = " << process[process.size() - 1].maxQL() << endl;
  out << endl;
}

double Ladder::avgQL() const{
  if(process.size() == 0){
    return 0;
  }
  return process[process.size() - 1].avgQL();
}

// height zero max
void Ladder::heightZero_MaxQL()
{
  findLimps();
  permcount = 0;
  vector<unsigned int> ordering_MaxQL = ordering(compare_ordering_MaxQL);
  for(vector<unsigned int>::size_type i = 0; i != ordering_MaxQL.size(); i++){
    int j = ordering_MaxQL[i];
    int QL = stats.updateStats(rlimps[j]);
    config.updateConfig(Implant(QL, rlimps[j].slot(), rlimps[j].abi(), rlimps[j].shi(), rlimps[j].bri(), rlimps[j].fad()));
  }
  for(Config::size_type i = 0; i != fReqs.size(); i++){
    if(!fReqs[i].ladder() && !config[i].isLocked()){
      int QL = stats.updateStats(fReqs[i]);
      // wasteful copying
      config.updateConfig(Implant(QL, fReqs[i].slot(), fReqs[i].abi(), fReqs[i].shi(), fReqs[i].bri(), fReqs[i].fad()));
    }
  }
  unequipLimps();
}

// height zero avg
void Ladder::heightZero_AvgQL()
{
  findLimps();
  permcount = 0;
  vector<unsigned int> ordering_AvgQL = ordering(compare_ordering_AvgQL);
  for(vector<unsigned int>::size_type i = 0; i != ordering_AvgQL.size(); i++){
    int j = ordering_AvgQL[i];
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

// height one
// improvements:
// check for any unused ladder implants that are:
// beneficial to equipping final implants and
// not in the same slot as a final implant (or a current limp)
// such implants do not need to be removed in reverse order;
// they can be removed after all required implants are equipped.
void Ladder::heightOne(const std::vector<Slot>& ladderSlots)
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
        tempLadder.heightZero_AvgQL();
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
  tempLadder.heightZero_AvgQL();
  process = tempLadder.process;
  if(process.size() == 1){
    process.push_back(process[0]);
  }
  stats = tempLadder.stats;
}

// height two
// height two isn't necessarily at least as good as height one... height one can get lucky and follow the path of a
// higher height
void Ladder::heightTwo(const std::vector<Slot>& ladderSlots)
{
  double maxAvgQL = 0;
  bool increasingAvgQL = true;
  vector<Implant> limps;
  // there are 10 ladder slots
  while(increasingAvgQL && limps.size() != 10){
    int originalSize = limps.size();
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
      // choose implant vector
      Slot::size_type i;
      for(i = 0; i != it->size(); i++){
        // choose first implant
        int max = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
          // looking at implants that have the same cluster specification, but different abi req
          // consider only one; the one for which our abi stats value is highest
          if(stats.getAbiFromName((*it)[i][j].abi()) > stats.getAbiFromName((*it)[i][max].abi())){
            max = j;
          }
        } 
        limps[limps.size() - 1] = (*it)[i][max];
        // size after the body of this if statement is the same as before the body!
        if(limps.size() != 10){
          // choose second slot
          limps.push_back(Implant());
          vector<Slot>::const_iterator it2;
          for(it2 = ladderSlots.begin(); it2 != ladderSlots.end(); it2++){
            if(slotTaken(it2->name(), limps)){
              continue;
            }
            // choose second implant vector
            Slot::size_type i2;
            for(i2 = 0; i2 != it2->size(); i2++){
              // choose second implant
              int max2 = 0;
              for(vector<Implant>::size_type j2 = 0; j2 != (*it2)[i2].size(); j2++){
                if(stats.getAbiFromName((*it2)[i2][j2].abi()) > stats.getAbiFromName((*it2)[i2][max2].abi())){
                  max2 = j2;
                }
              }
              limps[limps.size() - 1] = (*it2)[i2][max2];
              // test implants
              Ladder tempLadder(fReqs, stats);
              tempLadder.equipLimps(limps);
              tempLadder.heightZero_AvgQL();
              double tempAvgQL = tempLadder.avgQL();
              if(tempAvgQL >= maxAvgQL){
                increasingAvgQL = true;
                maxAvgQL = tempAvgQL;
                slotPosition = it;
                vectorPosition = i;
                indexPosition = max;
              }
            }
          }
          // get rid of extra spot
          limps.resize(limps.size() - 1);
        }
        else{
          // test implant
          Ladder tempLadder(fReqs, stats);
          tempLadder.equipLimps(limps);
          tempLadder.heightZero_AvgQL();
          double tempAvgQL = tempLadder.avgQL();
          if(tempAvgQL >= maxAvgQL){
            increasingAvgQL = true;
            maxAvgQL = tempAvgQL;
            slotPosition = it;
            vectorPosition = i;
            indexPosition = max;
          }
        }
      }
    }
    // found the best limp to add by seeing which limp, with another limp added after it, resulted in highest avgQL
    // only add the first limp though
    if(increasingAvgQL){
      limps[originalSize] = (*slotPosition)[vectorPosition][indexPosition]; 
      limps.resize(originalSize + 1);
    }
  }
  if(!increasingAvgQL){
    // didn't find a useful ladder imp last time, so remove
    limps.resize(limps.size() - 1);
  }

  Ladder tempLadder(fReqs,stats);
  tempLadder.equipLimps(limps);
  tempLadder.heightZero_AvgQL();
  process = tempLadder.process;
  stats = tempLadder.stats;
}

void Ladder::heightN(const vector<Slot>& ladderSlots, int N)
{
  counter = 0;
  double maxAvgQL = 0;
  bool increasingAvgQL = true;
  vector<Implant> limps;
  while(increasingAvgQL && limps.size() != 10){
    increasingAvgQL = false;
    limps.push_back(Implant());
    // recursive step -- after climb, if increasingAvgQL, limps will have the best limp if limps were 
    // added for min(N-1, 10 - limps.size()) more turns after it, and then the avgQL was calculated.
    // so, limps will be the same size as it was before the call to climb. 
    climb(ladderSlots, limps, maxAvgQL, increasingAvgQL, N, N);
    if(!increasingAvgQL){
      limps.resize(limps.size() - 1);
    }
  }
  Ladder tempLadder(fReqs,stats);
  tempLadder.equipLimps(limps);
  tempLadder.heightZero_AvgQL();
  process = tempLadder.process;
  stats = tempLadder.stats;
}


void Ladder::climb(const vector<Slot>& ladderSlots, vector<Implant>& limps, double& maxAvgQL, bool& increasingAvgQL, int bottom, int N)
{
  counter++;
  // if limps.size() == 10, limps is full; there are only 10 slots possible for laddering.
  // Therefore, manually stop any recursion that might be happening
  if(limps.size() == 10){
    N = 0;
  }
  if(N == 0){
    // done filling limps
    Ladder tempLadder(fReqs, stats);
    tempLadder.equipLimps(limps);
    tempLadder.heightZero_AvgQL();
    double tempAvgQL = tempLadder.avgQL();
    if(tempAvgQL > maxAvgQL){
      // changing this will let the top caller to know the implant it chose is important; the best found so far.
      maxAvgQL = tempAvgQL;
      // increasingAvgQL needs to be known in heightN; it means that some implant across all the available slots, can
      // increase the average QL if it (and subsequently min(N-1, 10 - limps.size()) others) are used to ladder.
      increasingAvgQL = true;
    }
    return;
  }
  // if we are at the bottom (where we start, when N is largest)
  // then must remember the limps we are trying, in order to store it permanently in limps vector
  if(N == bottom)
  {
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
      // choose implant vector
      Slot::size_type i;
      for(i = 0; i != it->size(); i++){
        int max = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
          if(stats.getAbiFromName((*it)[i][j].abi()) > stats.getAbiFromName((*it)[i][max].abi())){
            max = j;
          }
        }
        limps[limps.size() - 1] = (*it)[i][max];
        double oldMaxAvgQL = maxAvgQL;
        // RECURSE explore all N - 1 layers
        climb(ladderSlots, limps, maxAvgQL, increasingAvgQL, bottom, N - 1);
        // if maxAvgQL increased, this implant is the best found so far for the limp.size() - 1 # limp in limps
        if(oldMaxAvgQL != maxAvgQL){
          slotPosition = it;
          vectorPosition = i;
          indexPosition = max;
        }
      }
    }
    // add the best limp found... what if no improving imp was found?
    limps[limps.size() - 1] = (*slotPosition)[vectorPosition][indexPosition];
  }
  // intermediary position; add limp, recurse
  else{
    limps.push_back(Implant());
    vector<Slot>::const_iterator it;
    for(it = ladderSlots.begin(); it != ladderSlots.end(); it++){
      if(slotTaken(it->name(), limps)){
        continue;
      }
      Slot::size_type i;
      for(i = 0; i != it->size(); i++){
        int max = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
          if(stats.getAbiFromName((*it)[i][j].abi()) > stats.getAbiFromName((*it)[i][max].abi())){
            max = j;
          }
        }
        limps[limps.size() - 1] = (*it)[i][max];
        climb(ladderSlots, limps, maxAvgQL, increasingAvgQL, bottom, N - 1);
        // resize to size before recursive call
      }
    }
    // get rid of slot that was being temporarily used
    limps.resize(limps.size() - 1);
  }
}

// similar to height one solution, but before evaluating the effect of the current implant, ladder it up
// as much as possible directly. This can use at most 6 implants, 3 for treatment, and 3 for the ability.
// Don't allow trickle down implants; probably not worth it. Full QL 200 trickle imps give 16.5 treatment,
// good for 3 to 4 QLs, which is ~2 shining skill points.
void Ladder::heightOneLaddered(const std::vector<Slot>& ladderSlots)
{
  //
  double maxAvgQL = 0;
  bool increasingAvgQL = true;
  // contain the laddering imps used to equip each limp
  vector<Implant> limps;
  while(increasingAvgQL && limps.size() != 10){
    increasingAvgQL = false; 
    process.push_back(Config());
    limps.push_back(Implant());
    vector<Slot>::const_iterator slotPosition = ladderSlots.begin();
    int vectorPosition = 0;
    int indexPosition = 0;
    int limpQL = 0;
    Config helper;
    // choose limp:
    // choose slot
    vector<Slot>::const_iterator it;
    for(it = ladderSlots.begin(); it != ladderSlots.end(); it++){
      if(slotTaken(it->name(), limps)){
        continue;
      }
      // choose implant vector
      Slot::size_type i;
      for(i = 0; i != it->size(); i++){
        // choose implant
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
          Config helper2;
          int tempQL;
          // test implant
          Ladder tempLadder(fReqs, stats);
          tempLadder.equipLimpsLaddered(ladderSlots, limps, (*it)[i][j], helper2, tempQL);
          limps[limps.size() - 1] = (*it)[i][j];
          limps[limps.size() - 1].setQL(tempQL);
          Ladder tempLadder2(fReqs, stats);
          tempLadder2.equipLimpsLaddered(limps);
          tempLadder2.heightZero_AvgQL();
          double tempAvgQL = tempLadder2.avgQL();
          if(tempAvgQL > maxAvgQL){
            helper = helper2; 
            increasingAvgQL = true;
            maxAvgQL = tempAvgQL;
            slotPosition = it;
            vectorPosition = i;
            indexPosition = j;
            limpQL = tempQL; 
          }
        }
      }
    }
    process[process.size() - 1] = helper;
    limps[limps.size() - 1] = (*slotPosition)[vectorPosition][indexPosition]; 
    limps[limps.size() - 1].setQL(limpQL);
  }
  if(!increasingAvgQL){
    // didn't find a useful ladder imp last time, so remove
    limps.resize(limps.size() - 1);
    process.resize(process.size() - 1);
  }
  //printFlag = true;
  Ladder tempLadder(fReqs,stats);
  tempLadder.equipLimpsLaddered(limps);
  tempLadder.heightZero_AvgQL();
  process.push_back(tempLadder.process[0]);
  process.push_back(tempLadder.process[1]);
  stats = tempLadder.stats;
}

// this version equips limps, except for last, looks for available helper slots, finds them
// runs zero solver on those slots, equips them, and writes into QL the final QL of limps[limps.size() - 1]
void Ladder::equipLimpsLaddered(const vector<Slot>& ladderSlots, const vector<Implant>& limps, const Implant& limp, Config& helper, int& QL)
{
  for(vector<Implant>::size_type i = 0; i != limps.size() - 1; i++){
    stats.updateStats(limps[i], true, limps[i].ql());
  }

  // find ladder imps still available
  vector<string> takenSlots;
  bool progressMade = true;
  while(progressMade){
    progressMade = false;
    for(vector<Slot>::const_iterator it = ladderSlots.begin(); it != ladderSlots.end(); it++){
      if(slotTaken(it->name(), limps) || it->name() == limp.slot() || slotTaken(it->name(), takenSlots)){
        continue;
      }
      // find out from stats and limp what we are deficient in
      string limiting = stats.findLimiting(limp); 
      // search for an implant adding to the limiting factor within the slot:
      // potentials is a vector of at most 1 implant from each group within a slot; implants within the same slot group
      // have the same shi/bri/tre configuration, but different ability requirements. add the one for which our ability
      // is highest to potentials, then choose from potentials the for which our ability is highest
      vector<Implant> potentials;
      for(Slot::size_type i = 0; i != it->size(); i++){
        int bestInGroup = -1;
        if((*it)[i][0].shi() == limiting || (*it)[i][0].bri() == limiting || (*it)[i][0].fad() == limiting){
          bestInGroup = 0;
          for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); j++){
            if(stats.getAbiFromName((*it)[i][j].abi()) > stats.getAbiFromName((*it)[i][bestInGroup].abi())){
              bestInGroup = j;
            }
          }
          // an implant within this slot group adds to the limiting factor
          potentials.push_back((*it)[i][bestInGroup]);
        }
      }
      if(potentials.size()){
        progressMade = true;
        takenSlots.push_back(it->name());
        // find the best from potentials
        int maxAbi = 0;
        for(vector<Implant>::size_type j = 0; j != potentials.size(); j++){
          int tempAbi = stats.getAbiFromName(potentials[j].abi());
          if(tempAbi > maxAbi)
            maxAbi = tempAbi;
        }
        int bestIndex = 0;
        int buffingClusters = 0;
        // which implant is best? depends, as a guess, it's whatever requires the ability we have the most in,
        // with number of other non "shi", "bri", "fad" clusters breaking ties
        for(vector<Implant>::size_type j = 0; j != potentials.size(); j++){
          if(stats.getAbiFromName(potentials[j].abi())  == maxAbi){
            int tempBuffingClusters = 0;
            if(potentials[j].shi() != "shi")
              tempBuffingClusters++;
            if(potentials[j].bri() != "bri")
              tempBuffingClusters++;
            if(potentials[j].fad() != "fad")
              tempBuffingClusters++;
            if(tempBuffingClusters > buffingClusters){
              buffingClusters = tempBuffingClusters;
              bestIndex = j;
            }
          }
        }
        // add the implant
        int tempQL = stats.updateStats(potentials[bestIndex]);
        helper.updateConfig(potentials[bestIndex], tempQL);
      }
    }
  }
  QL = stats.updateStats(limp);
}

bool slotTaken(const string& str, const vector<string>& strs){
  for(vector<string>::const_iterator it = strs.begin(); it != strs.end(); it++){
    if(str == *it)
      return true;
  }
  return false;
}

// this version equips the limps, but uses the QL that they already possess
void Ladder::equipLimpsLaddered(const vector<Implant>& limps)
{
  // these already have QLs
  for(vector<Implant>::size_type i = 0; i != limps.size(); i++){
    stats.updateStats(limps[i], true, limps[i].ql());
    config.updateConfig(limps[i], limps[i].ql());
  }
}


vector<unsigned int> Ladder::ordering(bool compare(const vector<unsigned int>&, const vector<unsigned int>&, const Ladder&))
{
  vector<unsigned int> ordered;
  for(vector<Implant>::size_type i = 0; i != rlimps.size(); i++){
    ordered.push_back(i);
  }
  vector<unsigned int> partial;
  // puts optimal ordering in ordered vector<int>
  run_ordering(ordered, partial, compare);
  return ordered;
}

void Ladder::run_ordering(vector<unsigned int>& best, vector<unsigned int>& partial,
                          bool compare(const vector<unsigned int>&, const vector<unsigned int>&, const Ladder&))
{
  if(partial.size() == rlimps.size()){
    permcount++;
    if(!compare(best, partial, *this))
      best = partial;
    return;
  }
  // recursively evaluate all permutations of the implants that happen to be useful for laddering
  for(vector<Implant>::size_type i = 0; i != rlimps.size(); i++){
    bool avail = true;
    for(vector<unsigned int>::size_type j = 0; j != partial.size(); j++){
      if(partial[j] == i){
        avail = false;
        break;
      }
    }
    if(avail){
      vector<unsigned int>::size_type old_size = partial.size();
      partial.push_back(i);
      // RECURSE -- partial will be previous, plus next available
      run_ordering(best, partial, compare);
      // pass by reference, then resize when done to continue recursing at this level
      partial.resize(old_size);
    }
  }
}

bool compare_ordering_MaxQL(const vector<unsigned int>& best, const vector<unsigned int>& trial, const Ladder& l)
{
  // return true if best is better, else false
  // know: base stats, rlimps that the indices refer to 
  // constants for determining QLs/reqs/modifiers
  if(!best.size())
    return false;
  return find_MaxQL(best, l) >= find_MaxQL(trial, l);
}

bool compare_ordering_AvgQL(const vector<unsigned int>& best, const vector<unsigned int>& trial, const Ladder& l)
{
  if(!best.size())
    return false;
  return find_AvgQL(best, l) >= find_AvgQL(trial, l);
}

int find_MaxQL(const vector<unsigned int>& indices, const Ladder& l)
{
  int maxQL = 0;
  Stats cStats = l.stats; // current stats;
  for(vector<unsigned int>::size_type i = 0; i != indices.size(); i++){
    maxQL = max(maxQL, cStats.updateStats(l.rlimps[indices[i]]));
  }
  for(vector<unsigned int>::size_type i = 0; i != l.fReqs.size(); i++){
    if(!l.fReqs[i].ladder()){
     maxQL = max(maxQL, cStats.updateStats(l.fReqs[i]));
    }
  }
  return maxQL;
}

double find_AvgQL(const vector<unsigned int>& indices, const Ladder& l)
{
  double avgQL = 0;
  Stats cStats = l.stats;
  for(vector<unsigned int>::size_type i = 0; i != indices.size(); i++){
    avgQL += cStats.updateStats(l.rlimps[indices[i]]);
  }
  for(vector<unsigned int>::size_type i = 0; i != l.fReqs.size(); i++){
    if(!l.fReqs[i].ladder()){
     avgQL += cStats.updateStats(l.fReqs[i]);
    }
  }
  return avgQL/(l.fReqs.size());
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

bool slotTaken(const string& slotName, const vector<Implant>& imps)
{
  for(vector<Implant>::const_iterator it = imps.begin();
      it != imps.end(); it++){
    if(slotName == it->slot())
      return true;
  }
  if(slotName == "lwrist" || slotName == "rwrist" || slotName == "lhand"){
    return true;
  }
  return false;
}

void printImpVect(const vector<Implant>& imps){
  for(vector<Implant>::const_iterator i = imps.begin(); i != imps.end(); i++){
    i->out(cout); cout << endl;
  }
  cout << "done." << endl;
}

void Ladder::ladderedOut(std::ostream& out) const{
  for(vector<Config>::size_type i = 0; i != process.size() - 2; i++){
    if(process[i].notEmpty()){
      out << "Equip the intermediary ladder implants: " << endl;
      process[i].zeroOut(cout);
      out << endl;
    }
    out << "Equip the ladder implant: " << endl;
    process[process.size() - 2][process[process.size() - 2].getOrderedIndex(i)].out(cout);
    out << endl;
    out << endl;
  }
  out << "Setup before ladder implant reverse-removal: " << endl;
  process[process.size() - 2].zeroOut(cout);
  out << endl;
  out << "Final setup: " << endl;
  process[process.size() - 1].zeroOut(cout);
  out << "avgQL = " << process[process.size() - 1].avgQL() << endl;
  out << "maxQL = " << process[process.size() - 1].maxQL() << endl;
}


