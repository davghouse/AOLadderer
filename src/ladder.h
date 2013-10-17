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
  Ladder(const ImplantConfiguration& required_config, const Stats& stats);
  void HeightOne(const std::vector<LadderSlot>& ladder_slots);
  double AverageQL() const;

private:
  void EquipLadderImplants(const std::vector<Implant>& ladder_implants);
  void HeightZero();
  void FindLadderImplants();
  void Ordering(std::vector<uint>& order);
  void RunOrdering(std::vector<uint>& best, std::vector<uint>& current);
  bool Compare(const std::vector<uint> & best, const std::vector<uint> & current) const;
  double AverageQLFromOrdering(const std::vector<uint>& order) const;
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
