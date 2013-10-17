#include <math.h>
#include "character_stats.h"
#include "interpolation_coefficients.h"

using std::string; using std::vector;
using std::min;
using namespace interpolation_coefficients;

// Anonymous namespace with inline functions as only used in this .cpp file
// Has functions for determining requirements and modifiers from implant QLs.
// See interpolation_coefficients.h
namespace {

// Treatment requirements round up.
inline int QLToTreatment(int ql)
{
  return ql*kTreReqM + kTreReqC + .5;
}

// Be careful with rounding errors here;
// Find the ql close to the true ql that can be used, then compare nearby
// qls until the first one requiring <= the given treatment is found.
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

// kAbiReqM, kAbiReqC are integers, so no rounding necessary here.
inline int QLToAbility(int ql)
{
  return ql*kAbiReqM + kAbiReqC;
}

// Odd ability values yield fractional qls which are truncated correctly.
inline int AbilityToQL(int ability_modifier)
{
  return std::min((ability_modifier - kAbiReqC)/kAbiReqM, 200);
}

// Uses type to determine which interpolation_coefficients to use
// 0 - shining, 1 - bright, 2 - faded
inline int AbilityModifierFromQL(int type, int ql)
{
  if(type == 0)
    return kShiAbiM*ql + kShiAbiC + .5;
  if(type == 1)
    return kBriAbiM*ql + kBriAbiC + .5;
  return kFadAbiM*ql + kFadAbiC + .5;
}

// Uses type to determine which interpolation_coefficients to use
// 0 - shining, 1 - bright, 2 - faded
inline int SkillModifierFromQL(int type, int ql)
{
  if(type == 0)
    return kShiSkiM*ql + kShiSkiC + .5;
  if(type == 1)
    return kBriSkiM*ql + kBriSkiC + .5;
  return kFadSkiM*ql + kFadSkiC + .5;
}

// Treatment trickle-down follows the following formula:
// .3*agi + .5*int + .2*sen
// where agi, int, sen are abilities modifiers.
// ability_int determines what ability is being modified; see character_stats.h
inline double TreatmentTrickleFromAbilityModifier(int ability_int, int ability_modifier)
{
  if(ability_int == 1)
    return (.3*ability_modifier)/4;
  else if(ability_int == 3)
    return (.5*ability_modifier)/4;
  else if(ability_int == 4)
    return (.2*ability_modifier)/4;
  return 0;
}

} // namespace

CharacterStats::CharacterStats() : treatment_(0.0)
{
  abilities_.reserve(6);
}

void CharacterStats::UpdateStats(const vector<int>& abilities, double treatment)
{
  for(std::vector<int>::size_type i = 0; i != 6; ++i)
    abilities_.push_back(abilities[i]);
  treatment_ = treatment;
}

int CharacterStats::UpdateStats(const Implant& implant, bool inserting, int ql)
{
  // If inserting, add to abilities and treatment.
  if(inserting){
    if(ql == 0){
      // Determines implant ql by using helper functions in this .cc file
      // implant.ability_int() returns the int corresponding to the ability the implant requires.
      ql = std::min(TreatmentToQL(treatment_), AbilityToQL(abilities_[implant.ability_int()]));
    }
    if(implant.used_to_ladder()){
      // If shining cluster is important for laddering:
      if(implant.shining_abbr() != "shi"){
        // If shining cluster adds to an ability:
        if(implant.shining_int() <= 5 && implant.shining_int() >= 0){
          int ability_modifier = AbilityModifierFromQL(0,ql);
          abilities_[implant.shining_int()] += ability_modifier;
          treatment_ += TreatmentTrickleFromAbilityModifier(implant.shining_int(), ability_modifier);
        }
        // Else, if it adds to treatment:
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
  // If removing, subtract from abilities and treatment.
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

