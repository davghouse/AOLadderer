#ifndef CHARACTER_STATS_H_
#define CHARACTER_STATS_H_

#include <iostream>
#include <string>
#include <fstream>
#include <algorithm>
#include <vector>
#include <math.h>
#include "ladder_helper.h"
#include "implant.h"

// Holds the six abilities and treatment for a character and allows updating
// those skills by equipping or unequipping implants.
class CharacterStats{
public:
  // Reserves size for holding the abilities.
  CharacterStats();

  CharacterStats(const CharacterStats& stats);

  // Gets value of ability from name of ability.
  int ability_from_name(const std::string& ability_name) const;
  int get_treatment() const;

  // Replaces current stats with given stats.
  void UpdateStats(const std::vector<int>& abilities, double treatment);

  // Updates stats by inserting or removing a given implant. If inserting
  // and the given ql is 0, figures out what ql to insert, and returns this value.
  int UpdateStats(const Implant& implant, bool inserting = true, int ql = 0);

private:
  // Holds the six abilities in the following order:
  // Strength, Agility, Stamina, Intelligence, Sense, Psychic
  // 0         1        2        3             4      5
  std::vector<int> abilities_;
  double treatment_;
};

inline int CharacterStats::ability_from_name(const std::string& ability_name) const
{
  return abilities_[ladder_helper::AbilityToInt(ability_name)];
}

inline int CharacterStats::get_treatment() const
{
 return treatment_;
}

#endif // CHARACTER_STATS_H_
