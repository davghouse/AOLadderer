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
  void HeightOne(const std::vector<LadderSlot>&);
  double AverageQL() const;

private:
  void EquipLadderImplants(const std::vector<Implant>&);
  void HeightZero();
  void FindLadderImplants();
  void Ordering(std::vector<uint>&);
  void RunOrdering(std::vector<uint>&, std::vector<uint>&);
  bool Compare(const std::vector<uint> &, const std::vector<uint> &) const;
  double AverageQLFromOrdering(const std::vector<uint> &) const;
  void UnequipLadderImplants();

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
