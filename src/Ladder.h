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

typedef unsigned int uint;

class Ladder{
  friend class MainWindow;
  friend bool compare(const std::vector<uint>&, const std::vector<uint>&, const Ladder&);
  friend double find_AvgQL(const std::vector<uint>&, const Ladder&);

public:
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
  void unequipLimps();

private:
  std::vector<Config> process; // laddering process 
  Config config; // working config

  std::vector<Implant> rlimps; // required implants useful for laddering.

  Config fReqs; // final implant requirements 
  Stats stats; // current stats
};

bool slotTaken(const std::string&, const std::vector<Implant>&);
double find_AvgQL(const std::vector<uint>&, const Ladder&);
bool compare(const std::vector<uint>&, const std::vector<uint>&, const Ladder&);

#endif // LADDER_H_
