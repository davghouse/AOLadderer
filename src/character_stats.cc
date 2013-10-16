#include <math.h>
#include "character_stats.h"
#include "interpolation_coefficients.h"

using std::string; using std::vector;
using std::min;
using namespace interpolation_coefficients;

// anonymous namespace as only used in this .cpp file
namespace {

// rounding here is easy
inline int QLToTreatment(int QL)
{
  return QL*kTreReqM + kTreReqC + .5;
}

// rounding is detailed here - be really explicit to avoid errors
inline int TreatmentToQL(double tre)
{
  int QL = ceil((tre - kTreReqC)/kTreReqM);
  if(QLToTreatment(QL + 1) <= tre)
    return min(QL + 1, 200);
  if(QLToTreatment(QL) <= tre)
    return min(QL, 200);
  if(QLToTreatment(QL - 1) <= tre)
    return min(QL - 1, 200);
  if(QLToTreatment(QL - 2) <= tre)
    return min(QL - 2, 200);
  return min(QL - 3, 200);
}

inline int QLToAbility(int QL)
{
  return QL*kAbiReqM + kAbiReqC + .5;
}

inline int AbilityToQL(int abi)
{
  return std::min((abi - kAbiReqC)/kAbiReqM, 200);
}

// 0 - shining, 1 - bright, 2 - faded
inline int AbilityModifierFromQL(int type, int QL)
{
  if(type == 0)
    return kShiAbiM*QL + kShiAbiC + .5;
  if(type == 1)
    return kBriAbiM*QL + kBriAbiC + .5;
  return kFadAbiM*QL + kFadAbiC + .5;
}

inline int SkillModifierFromQL(int type, int QL)
{
  if(type == 0)
    return kShiSkiM*QL + kShiSkiC + .5;
  if(type == 1)
    return kBriSkiM*QL + kBriSkiC + .5;
  return kFadSkiM*QL + kFadSkiC + .5;
}

inline double TreatmentTrickleFromAbilityModifier(int abi, int abiMod)
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

void Stats::UpdateStats(const vector<int>& abisHolder, double treatmentHolder)
{
  for(std::vector<int>::size_type i = 0; i != 6; ++i)
    abilities_.push_back(abisHolder[i]);
  treatment_ = treatmentHolder;
}

int Stats::UpdateStats(const Implant& imp, bool inserting, int QL)
{
  if(inserting){
    // find out what QL imp to put in
    if(QL == 0){
      QL = std::min(TreatmentToQL(treatment_), AbilityToQL(abilities_[imp.ability_int()]));
    }
    // update stats
    if(imp.used_to_ladder()){
      if(imp.shining_abbr() != "shi"){
        if(imp.shining_int() <= 5 && imp.shining_int() >= 0){
          int abiMod = AbilityModifierFromQL(0,QL);
          abilities_[imp.shining_int()] += abiMod;
          treatment_ += TreatmentTrickleFromAbilityModifier(imp.shining_int(), abiMod);
        }
        else if(imp.shining_int() == 6)
          treatment_ += SkillModifierFromQL(0,QL);
      }
      if(imp.bright_abbr() != "bri"){
        if(imp.bright_int() <= 5 && imp.bright_int() >= 0){
          int abiMod = AbilityModifierFromQL(1,QL);
          abilities_[imp.bright_int()] += abiMod;
          treatment_ += TreatmentTrickleFromAbilityModifier(imp.bright_int(), abiMod);
        }
        else if(imp.bright_int() == 6)
          treatment_ += SkillModifierFromQL(1,QL);
      }
      if(imp.faded_abbr() != "fad"){
        if(imp.faded_int() <= 5 && imp.faded_int() >= 0){
          int abiMod = AbilityModifierFromQL(2,QL);
          abilities_[imp.faded_int()] += abiMod;
          treatment_ += TreatmentTrickleFromAbilityModifier(imp.faded_int(), abiMod);
        }
        else if(imp.faded_int() == 6)
          treatment_ += SkillModifierFromQL(2,QL);
      }
    }
    return QL;
  }
  else{
    int QL = imp.ql();
    if(imp.shining_abbr() != "shi"){
      if(imp.shining_int() <= 5 && imp.shining_int() >= 0){
        int abiMod = AbilityModifierFromQL(0, QL);
        abilities_[imp.shining_int()] -= abiMod;
        treatment_ -= TreatmentTrickleFromAbilityModifier(imp.shining_int(), abiMod);
      }
      else if(imp.shining_int() == 6)
        treatment_ -= SkillModifierFromQL(0,QL);
    }
    if(imp.bright_abbr() != "bri"){
      if(imp.bright_int() <= 5 && imp.bright_int() >= 0){
        int abiMod = AbilityModifierFromQL(1,QL);
        abilities_[imp.bright_int()] -= abiMod;
        treatment_ -= TreatmentTrickleFromAbilityModifier(imp.bright_int(), abiMod);
      }
      else if(imp.bright_int() == 6)
        treatment_ -= SkillModifierFromQL(1,QL);
    }
    if(imp.faded_abbr() != "fad"){
      if(imp.faded_int() <= 5 && imp.faded_int() >= 0){
        int abiMod = AbilityModifierFromQL(2,QL);
        abilities_[imp.faded_int()] -= abiMod;
        treatment_ -= TreatmentTrickleFromAbilityModifier(imp.faded_int(), abiMod);
      }
      else if(imp.faded_int() == 6)
        treatment_ -= SkillModifierFromQL(2,QL);
    }
  }
  return 0;
}

