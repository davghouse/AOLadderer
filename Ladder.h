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

// height zero algorithms: use no intermediary implants. insert implants matching the requirement in the optimal order, accounting for their abi/tre modifiers.
// height one algorithms: use intermediary laddering implants. find intermediary implant that allows for highest avg QL if final implants were added immediately afterwards. pick it, then choose next intermediary implant, and so on, until no more slots remain, or adding the next intermediary implant does not increasing the avg QL of final config. when determining avg QL, remove intermediary implants in reverse order (LIFO).  
// height one algorithms: think about laddering up the intermediary implants before inserting them. seems easy, perhaps, to ladder up a specific implant using <= 6 implants, depending on how many available, 3 for treatment, 3 for ability
// Possible to make a general height solver, using recursion
// Idea: at each step, use recursive solver until no more gain, some steps require only height one, some require more but not locked into any one height.
class Ladder{
  friend class MainWindow;
  friend bool compare_ordering_MaxQL(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&);
  friend bool compare_ordering_AvgQL(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&);
  friend int find_MaxQL(const std::vector<unsigned int>&, const Ladder&);
  friend double find_AvgQL(const std::vector<unsigned int>&, const Ladder&);
public:
  Ladder() {}
  Ladder(const Config&, const Stats&);
  void out(std::ostream&) const;
  void zeroOut(std::ostream&) const;
  void ladderedOut(std::ostream&) const;
  void heightZero_MaxQL();
  void heightZero_AvgQL();
  void heightOne(const std::vector<Slot>&);
  void heightTwo(const std::vector<Slot>&);
  void heightN(const std::vector<Slot>&, int);
  void climb(const std::vector<Slot>&, std::vector<Implant>&, double&, bool&, int, int);
  void heightOneLaddered(const std::vector<Slot>&);
  double avgQL() const;
  void equipLimps(const std::vector<Implant>&);
  void equipLimpsLaddered(const std::vector<Slot>&, const std::vector<Implant>&, const Implant&, Config&, int&);
  void equipLimpsLaddered(const std::vector<Implant>&);
  void unequipLimps();
  
private:
  void findLimps();
  std::vector<unsigned int> ordering(bool compare(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&));
  void run_ordering(std::vector<unsigned int>&, std::vector<unsigned int>&,
                    bool compare(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&));
private:
  std::vector<Config> process; // laddering process 
  Config config; // working config

  std::vector<Implant> rlimps; // required implants useful for laddering.

  Config fReqs; // final implant requirements 
  Stats stats; // current stats
};

bool compare_ordering_MaxQL(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&);
bool compare_ordering_AvgQL(const std::vector<unsigned int>&, const std::vector<unsigned int>&, const Ladder&);
int find_MaxQL(const std::vector<unsigned int>&, const Ladder&);
double find_AvgQL(const std::vector<unsigned int>&, const Ladder&);
bool slotTaken(const std::string&, const std::vector<Implant>&);
bool slotTaken(const std::string&, const std::vector<std::string>&);
void printImpVect(const std::vector<Implant>&);

#endif
