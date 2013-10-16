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
  int get_ability_from_name(const std::string& abiName) const;

  // added for GUI
  void UpdateStats(const std::vector<int>& abisHolder, double treatmentHolder);
  int UpdateStats(const Implant& imp, bool inserting = true, int QL = 0);

private:
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  std::vector<int> abilities_;
  double treatment_;
};

inline int Stats::get_ability_from_name(const std::string& abiName) const
{
  return abilities_[ladder_helper::AbilityToInt(abiName)];
}

#endif // CHARACTER_STATS_H_
