#include <math.h>
#include "character_stats.h"
#include "interpolation_coefficients.h"

using std::string; using std::vector;
using std::min;
using namespace interpolation_coefficients;

// anonymous namespace as only used in this .cpp file
namespace {

// rounding here is easy
inline int QLToTreatment(int ql)
{
  return ql*kTreReqM + kTreReqC + .5;
}

// rounding is detailed here - be really explicit to avoid errors
inline int TreatmentToQL(double treatment)
{
  int ql = ceil((treatment - kTreReqC)/kTreReqM);
  if(QLToTreatment(ql + 1) <= treatment)
    return min(ql + 1, 200);
  if(QLToTreatment(ql) <= treatment)
    return min(ql, 200);
  if(QLToTreatment(ql - 1) <= treatment)
    return min(ql - 1, 200);
  if(QLToTreatment(ql - 2) <= treatment)
    return min(ql - 2, 200);
  return min(ql - 3, 200);
}

inline int QLToAbility(int ql)
{
  return ql*kAbiReqM + kAbiReqC + .5;
}

inline int AbilityToQL(int ability_int)
{
  return std::min((ability_int - kAbiReqC)/kAbiReqM, 200);
}

// 0 - shining, 1 - bright, 2 - faded
inline int AbilityModifierFromQL(int type, int ql)
{
  if(type == 0)
    return kShiAbiM*ql + kShiAbiC + .5;
  if(type == 1)
    return kBriAbiM*ql + kBriAbiC + .5;
  return kFadAbiM*ql + kFadAbiC + .5;
}

inline int SkillModifierFromQL(int type, int ql)
{
  if(type == 0)
    return kShiSkiM*ql + kShiSkiC + .5;
  if(type == 1)
    return kBriSkiM*ql + kBriSkiC + .5;
  return kFadSkiM*ql + kFadSkiC + .5;
}

inline double TreatmentTrickleFromAbilityModifier(int ability_int, int ability_modifier)
{
  // treatment trickle: (.3*agi + .5*int + .2*sen)/4
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  if(ability_int == 1)
    return (.3*ability_modifier)/4;
  else if(ability_int == 3)
    return (.5*ability_modifier)/4;
  else if(ability_int == 4)
    return (.2*ability_modifier)/4;
  return 0;
}

} // namespace

Stats::Stats() : treatment_(0.0)
{
  abilities_.reserve(6);
}

void Stats::UpdateStats(const vector<int>& abilities, double treatment)
{
  for(std::vector<int>::size_type i = 0; i != 6; ++i)
    abilities_.push_back(abilities[i]);
  treatment_ = treatment;
}

int Stats::UpdateStats(const Implant& implant, bool inserting, int ql)
{
  if(inserting){
    // find out what ql implant to put in
    if(ql == 0){
      ql = std::min(TreatmentToQL(treatment_), AbilityToQL(abilities_[implant.ability_int()]));
    }
    // update stats
    if(implant.used_to_ladder()){
      if(implant.shining_abbr() != "shi"){
        if(implant.shining_int() <= 5 && implant.shining_int() >= 0){
          int ability_modifier = AbilityModifierFromQL(0,ql);
          abilities_[implant.shining_int()] += ability_modifier;
          treatment_ += TreatmentTrickleFromAbilityModifier(implant.shining_int(), ability_modifier);
        }
        else if(implant.shining_int() == 6)
          treatment_ += SkillModifierFromQL(0,ql);
      }
      if(implant.bright_abbr() != "bri"){
        if(implant.bright_int() <= 5 && implant.bright_int() >= 0){
          int ability_modifier = AbilityModifierFromQL(1,ql);
          abilities_[implant.bright_int()] += ability_modifier;
          treatment_ += TreatmentTrickleFromAbilityModifier(implant.bright_int(), ability_modifier);
        }
        else if(implant.bright_int() == 6)
          treatment_ += SkillModifierFromQL(1,ql);
      }
      if(implant.faded_abbr() != "fad"){
        if(implant.faded_int() <= 5 && implant.faded_int() >= 0){
          int ability_modifier = AbilityModifierFromQL(2,ql);
          abilities_[implant.faded_int()] += ability_modifier;
          treatment_ += TreatmentTrickleFromAbilityModifier(implant.faded_int(), ability_modifier);
        }
        else if(implant.faded_int() == 6)
          treatment_ += SkillModifierFromQL(2,ql);
      }
    }
    return ql;
  }
  else{
    int ql = implant.ql();
    if(implant.shining_abbr() != "shi"){
      if(implant.shining_int() <= 5 && implant.shining_int() >= 0){
        int ability_modifier = AbilityModifierFromQL(0, ql);
        abilities_[implant.shining_int()] -= ability_modifier;
        treatment_ -= TreatmentTrickleFromAbilityModifier(implant.shining_int(), ability_modifier);
      }
      else if(implant.shining_int() == 6)
        treatment_ -= SkillModifierFromQL(0,ql);
    }
    if(implant.bright_abbr() != "bri"){
      if(implant.bright_int() <= 5 && implant.bright_int() >= 0){
        int ability_modifier = AbilityModifierFromQL(1,ql);
        abilities_[implant.bright_int()] -= ability_modifier;
        treatment_ -= TreatmentTrickleFromAbilityModifier(implant.bright_int(), ability_modifier);
      }
      else if(implant.bright_int() == 6)
        treatment_ -= SkillModifierFromQL(1,ql);
    }
    if(implant.faded_abbr() != "fad"){
      if(implant.faded_int() <= 5 && implant.faded_int() >= 0){
        int ability_modifier = AbilityModifierFromQL(2,ql);
        abilities_[implant.faded_int()] -= ability_modifier;
        treatment_ -= TreatmentTrickleFromAbilityModifier(implant.faded_int(), ability_modifier);
      }
      else if(implant.faded_int() == 6)
        treatment_ -= SkillModifierFromQL(2,ql);
    }
  }
  return 0;
}

