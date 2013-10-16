#ifndef LADDER_H_
#define LADDER_H_

#include <fstream>
#include <string>
#include <vector>
#include "Coefficients.h"
#include "Config.h"
#include "Implant.h"
#include "Slot.h"
#include "Stats.h"


class Ladder{
public:
  friend class MainWindow;
  typedef unsigned int uint;

  Ladder() {}
  Ladder(const Config&, const Stats&);
  void heightOne(const std::vector<Slot>&);
  double avgQL() const;
  
private:
  void equipLimps(const std::vector<Implant>&);
  void heightZero();
  void findLimps();
  void ordering(std::vector<uint>&);
  void run_ordering(std::vector<uint>&, std::vector<uint>&);
  bool compare(const std::vector<uint> &, const std::vector<uint> &) const;
  double find_AvgQL(const std::vector<uint> &) const;
  void unequipLimps();

  // Disallow copy and assign
  Ladder(const Ladder&);
  void operator=(const Ladder&);

private:
  std::vector<Config> process; // laddering process 
  Config config; // working config

  std::vector<Implant> rlimps; // required implants useful for laddering.

  Config fReqs; // final implant requirements 
  Stats stats; // current stats
};

#endif // LADDER_H_
