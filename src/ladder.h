#ifndef LADDER_H_
#define LADDER_H_

#include <fstream>
#include <string>
#include <vector>
#include "interpolation_coefficients.h"
#include "implant_configuration.h"
#include "implant.h"
#include "ladder_slot.h"
#include "character_stats.h"

class Ladder{
  friend class MainWindow;

public:
  typedef unsigned int uint;

  Ladder() {}
  Ladder(const ImplantConfiguration&, const Stats&);
  void heightOne(const std::vector<LadderSlot>&);
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
  std::vector<ImplantConfiguration> process_; // laddering process
  ImplantConfiguration working_config_;

  std::vector<Implant> required_ladder_implants_; // required implants useful for laddering.

  ImplantConfiguration required_config_; // final implant requirements
  Stats stats_; // current stats
};

#endif // LADDER_H_
