#include <math.h>
#include "character_stats.h"
#include "interpolation_coefficients.h"

using std::string; using std::vector;
using std::min;
using namespace interpolation_coefficients;

// anonymous namespace as only used in this .cpp file
namespace {

// rounding here is easy
inline int QLToTre(int QL)
{
  return QL*kTreReqM + kTreReqC + .5;
}

// rounding is detailed here - be really explicit to avoid errors
inline int treToQL(double tre)
{
  int QL = ceil((tre - kTreReqC)/kTreReqM);
  if(QLToTre(QL + 1) <= tre)
    return min(QL + 1, 200);
  if(QLToTre(QL) <= tre)
    return min(QL, 200);
  if(QLToTre(QL - 1) <= tre)
    return min(QL - 1, 200);
  if(QLToTre(QL - 2) <= tre)
    return min(QL - 2, 200);
  return min(QL - 3, 200);
}

inline int QLToAbi(int QL)
{
  return QL*kAbiReqM + kAbiReqC + .5;
}

inline int abiToQL(int abi)
{
  return std::min((abi - kAbiReqC)/kAbiReqM, 200);
}

// 0 - shining, 1 - bright, 2 - faded
inline int abiModFromQL(int type, int QL)
{
  if(type == 0)
    return kShiAbiM*QL + kShiAbiC + .5;
  if(type == 1)
    return kBriAbiM*QL + kBriAbiC + .5;
  return kFadAbiM*QL + kFadAbiC + .5;
}

inline int skiModFromQL(int type, int QL)
{
  if(type == 0)
    return kShiSkiM*QL + kShiSkiC + .5;
  if(type == 1)
    return kBriSkiM*QL + kBriSkiC + .5;
  return kFadSkiM*QL + kFadSkiC + .5;
}

inline double treTrickleFromAbiMod(int abi, int abiMod)
{
  // treatment trickle: (.3*agi + .5*int + .2*sen)/4
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  if(abi == 1)
    return (.3*abiMod)/4;
  else if(abi == 3)
    return (.5*abiMod)/4;
  else if(abi == 4)
    return (.2*abiMod)/4;
  return 0;
}

} // namespace

Stats::Stats() : treatment_(0.0)
{
  abilities_.reserve(6);
}

void Stats::updateStats(const vector<int>& abisHolder, double treatmentHolder)
{
  for(std::vector<int>::size_type i = 0; i != 6; ++i)
    abilities_.push_back(abisHolder[i]);
  treatment_ = treatmentHolder;
}

int Stats::updateStats(const Implant& imp, bool inserting, int QL)
{
  if(inserting){
    // find out what QL imp to put in
    if(QL == 0){
      QL = std::min(treToQL(treatment_), abiToQL(abilities_[imp.abiI()]));
    }
    // update stats
    if(imp.ladder()){
      if(imp.shi() != "shi"){
        if(imp.shiI() <= 5 && imp.shiI() >= 0){
          int abiMod = abiModFromQL(0,QL);
          abilities_[imp.shiI()] += abiMod;
          treatment_ += treTrickleFromAbiMod(imp.shiI(), abiMod);
        }
        else if(imp.shiI() == 6)
          treatment_ += skiModFromQL(0,QL);
      }
      if(imp.bri() != "bri"){
        if(imp.briI() <= 5 && imp.briI() >= 0){
          int abiMod = abiModFromQL(1,QL);
          abilities_[imp.briI()] += abiMod;
          treatment_ += treTrickleFromAbiMod(imp.briI(), abiMod);
        }
        else if(imp.briI() == 6)
          treatment_ += skiModFromQL(1,QL);
      }
      if(imp.fad() != "fad"){
        if(imp.fadI() <= 5 && imp.fadI() >= 0){
          int abiMod = abiModFromQL(2,QL);
          abilities_[imp.fadI()] += abiMod;
          treatment_ += treTrickleFromAbiMod(imp.fadI(), abiMod);
        }
        else if(imp.fadI() == 6)
          treatment_ += skiModFromQL(2,QL);
      }
    }
    return QL;
  }
  else{
    int QL = imp.ql();
    if(imp.shi() != "shi"){
      if(imp.shiI() <= 5 && imp.shiI() >= 0){
        int abiMod = abiModFromQL(0, QL);
        abilities_[imp.shiI()] -= abiMod;
        treatment_ -= treTrickleFromAbiMod(imp.shiI(), abiMod);
      }
      else if(imp.shiI() == 6)
        treatment_ -= skiModFromQL(0,QL);
    }
    if(imp.bri() != "bri"){
      if(imp.briI() <= 5 && imp.briI() >= 0){
        int abiMod = abiModFromQL(1,QL);
        abilities_[imp.briI()] -= abiMod;
        treatment_ -= treTrickleFromAbiMod(imp.briI(), abiMod);
      }
      else if(imp.briI() == 6)
        treatment_ -= skiModFromQL(1,QL);
    }
    if(imp.fad() != "fad"){
      if(imp.fadI() <= 5 && imp.fadI() >= 0){
        int abiMod = abiModFromQL(2,QL);
        abilities_[imp.fadI()] -= abiMod;
        treatment_ -= treTrickleFromAbiMod(imp.fadI(), abiMod);
      }
      else if(imp.fadI() == 6)
        treatment_ -= skiModFromQL(2,QL);
    }
  }
  return 0;
}

