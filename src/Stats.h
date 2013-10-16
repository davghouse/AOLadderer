#ifndef STATS_H_
#define STATS_H_

#include <iostream>
#include <string>
#include <fstream>
#include <algorithm>
#include <vector>
#include <math.h>
#include "Coefficients.h"
#include "Helper.h"
#include "Implant.h"

class Stats{
public:
  Stats();

  int str() const { return abis[0] ; }
  int agi() const { return abis[1]; }
  int sta() const { return abis[2]; }
  int intel() const { return abis[3]; }
  int sen() const { return abis[4]; }
  int psy() const { return abis[5]; }
  double tre() const { return treatment; }

  // this seems bad
  void str(int i) { abis[0] += i; }
  void agi(int i) { abis[1] += i; }
  void sta(int i) { abis[2] += i; }
  void intel(int i) { abis[3] += i; }
  void sen(int i) { abis[4] += i; }
  void psy(int i) { abis[5] += i; }
  void tre(double d) { treatment += d; }

  // get value of ability from name of ability
  int getAbiFromName(const std::string& abiName) const;

  // added for GUI
  void updateStats(const std::vector<int>& abisHolder, double treatmentHolder);
  int updateStats(const Implant& imp, bool inserting = true, int QL = 0);

private:
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  std::vector<int> abis; 
  double treatment;
};

inline int Stats::getAbiFromName(const std::string& abiName) const
{
  return abis[abiReqToInt(abiName)];
}

#endif // STATS_H_
