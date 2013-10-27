#ifndef LADDER_SLOT_H_
#define LADDER_SLOT_H_

#include <vector>
#include <string>
#include "implant.h"

// Holds implants available to the algorithm for laddering for a specific slot.
// Contains a vector of implant vectors, each implant vector containing implants
// with different ability requirements but equal modifiers for treatment/abilities.
// Populated from ladder_implants.txt when the application is opened.
class LadderSlot{
public:
  typedef std::vector<std::vector<Implant> >::size_type size_type;
  typedef std::vector<std::vector<Implant> >::const_iterator const_iterator;

  LadderSlot();
  explicit LadderSlot(const std::string& n) : name_(n) {}

  void AddLadderImplant(const Implant& implant);
  void AddLadderImplantSubset(){ ladder_implants_.push_back(std::vector<Implant>()); }
  const std::string& name() const { return name_; }
  size_type size() const { return ladder_implants_.size(); }

  const std::vector<Implant>& operator[](int i) const { return ladder_implants_[i]; }

  const_iterator begin() const { return ladder_implants_.begin(); }
  const_iterator end() const { return ladder_implants_.end(); }

private:
  std::string name_;
  std::vector<std::vector <Implant> > ladder_implants_;
};

inline void LadderSlot::AddLadderImplant(const Implant& implant){
  ladder_implants_[ladder_implants_.size() - 1].push_back(implant);
  int index = ladder_implants_[ladder_implants_.size() - 1].size() - 1;
  ladder_implants_[ladder_implants_.size() - 1][index].set_lock(true);
}


#endif // LADDER_SLOT_H_
