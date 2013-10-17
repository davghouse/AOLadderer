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

class Stats{
public:
  Stats();

  // get value of ability from name of ability
  int ability_from_name(const std::string& ability_name) const;

  // added for GUI
  void UpdateStats(const std::vector<int>& abilities, double treatment);
  int UpdateStats(const Implant& implant, bool inserting = true, int ql = 0);

private:
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  std::vector<int> abilities_;
  double treatment_;
};

// could put in class definition
inline int Stats::ability_from_name(const std::string& ability_name) const
{
  return abilities_[ladder_helper::AbilityToInt(ability_name)];
}

#endif // CHARACTER_STATS_H_
