#ifndef GUARD_Ladder_h_
#define GUARD_Ladder_h_

#include <fstream>
#include <string>
#include <vector>
#include "Coefficients.h"
#include "Config.h"
#include "Implant.h"
#include "Slot.h"
#include "Stats.h"

typedef unsigned int uint;

// height zero algorithms: use no intermediary implants. insert implants matching the requirement in the optimal order, accounting for their abi/tre modifiers.
// height one algorithms: use intermediary laddering implants. find intermediary implant that allows for highest avg QL if final implants were added immediately afterwards. pick it, then choose next intermediary implant, and so on, until no more slots remain, or adding the next intermediary implant does not increasing the avg QL of final config. when determining avg QL, remove intermediary implants in reverse order (LIFO).  
// height one algorithms: think about laddering up the intermediary implants before inserting them. seems easy, perhaps, to ladder up a specific implant using <= 6 implants, depending on how many available, 3 for treatment, 3 for ability
// Possible to make a general height solver, using recursion
// Idea: at each step, use recursive solver until no more gain, some steps require only height one, some require more but not locked into any one height.
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
bool compare(const std::vector<uint>&, const std::vector<uint>&, const Ladder&);
double find_AvgQL(const std::vector<uint>&, const Ladder&);

#endif
