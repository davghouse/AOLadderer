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

// Algorithm class, carries out the laddering process described in the readme.
class Ladder{
  friend class MainWindow;

public:
  typedef unsigned int uint;
  Ladder() {}
  Ladder(const ImplantConfiguration& required_config, const CharacterStats& stats);

  // Main algorithm, greedily selects the next best laddering implant until
  // there are no more laddering slots available, or there is no beneift to selecting
  // more. Uses HeightZero after selecting the next ladder implant to try.
  void HeightOne(const std::vector<LadderSlot>& ladder_slots);

  // Computes the average QL of a final implant configuration
  double AverageQL() const;

private:
  // Updates stats while equipping ladder implants, marking them as locked and
  // conditionally as removable, if there is a final implant occupying the slot.
  void EquipLadderImplants(const std::vector<Implant>& ladder_implants);

  // Finds any required ladder implants which can be equipped. Then,
  // finds the optimal ordering of equipping these ladder implants by
  // exploring all possibilities. Equips the implants in that order, then
  // equips any other required implants whose slots are empty. Then unequips
  // and replaces any non-required ladder implants in the way of a
  // required implant, reversing their order of insertion.
  void HeightZero();

  // Finds required implants useful for laddering whose slots are not already
  // occupied by other laddering implants.
  void FindRequiredLadderImplants();

  // Finds optimal (highest average QL) order of insertion of a set of implants,
  // storing the result in the order vector passed by reference.
  void Order(std::vector<uint>& order);

  // Runs Ordering.
  void RunOrder(std::vector<uint>& best, std::vector<uint>& current);

  // Compares two orderings to determine the better one, return true if the
  // best found so far is better than the one being considered.
  bool Compare(const std::vector<uint> & best, const std::vector<uint> & current) const;

  // Used by compare.
  double AverageQLFromOrdering(const std::vector<uint>& order) const;

  // Unequips any non-required, removable ladder implants in the reverse order
  // of their insertion, and equips the required final implant.
  void UnequipLadderImplants();

  // Disallow copy and assign
  Ladder(const Ladder&);
  void operator=(const Ladder&);

private:
  // Holds the laddering process, in HeightOne's case, just two steps long.
  std::vector<ImplantConfiguration> process_;

  // Config used by HeightZero. Initially, it holds the locked ladder implants supplied by
  // HeightOne. It becomes populated with required implants and forms the basis for GUI's
  // Step One output. UnequipLadderImplants traverses it, looking for locked and removable
  // implants, removing them, and putting the required final implant into a temporary config,
  // to be added to process_.
  ImplantConfiguration working_config_;

  // Found by FindRequiredLadderImplants for use in HeightZero.
  std::vector<Implant> required_ladder_implants_;

  // Final implant requirements.
  ImplantConfiguration required_config_;

  // Current stats.
  CharacterStats stats_; // current stats
};

#endif // LADDER_H_
