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

  int str() const { return abilities_[0] ; }
  int agi() const { return abilities_[1]; }
  int sta() const { return abilities_[2]; }
  int intel() const { return abilities_[3]; }
  int sen() const { return abilities_[4]; }
  int psy() const { return abilities_[5]; }
  double tre() const { return treatment_; }

  // this seems bad
  void str(int i) { abilities_[0] += i; }
  void agi(int i) { abilities_[1] += i; }
  void sta(int i) { abilities_[2] += i; }
  void intel(int i) { abilities_[3] += i; }
  void sen(int i) { abilities_[4] += i; }
  void psy(int i) { abilities_[5] += i; }
  void tre(double d) { treatment_ += d; }

  // get value of ability from name of ability
  int getAbiFromName(const std::string& abiName) const;

  // added for GUI
  void updateStats(const std::vector<int>& abisHolder, double treatmentHolder);
  int updateStats(const Implant& imp, bool inserting = true, int QL = 0);

private:
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  std::vector<int> abilities_;
  double treatment_;
};

inline int Stats::getAbiFromName(const std::string& abiName) const
{
  return abilities_[ladder_helper::abiReqToInt(abiName)];
}

#endif // CHARACTER_STATS_H_
