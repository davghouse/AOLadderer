#ifndef LADDER_SLOT_H_
#define LADDER_SLOT_H_

#include <vector>
#include <string>
#include "implant.h"

 // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
class LadderSlot{
public:
  typedef std::vector<std::vector<Implant> >::size_type size_type;
  typedef std::vector<std::vector<Implant> >::const_iterator const_iterator;

  LadderSlot();
  explicit LadderSlot(std::string n) : name_(n) {}

  void AddLadderImplant(Implant i){ ladder_implants_[ladder_implants_.size() - 1].push_back(i); }
  void AddLadderImplantSubset(){ ladder_implants_.push_back(std::vector<Implant>()); }
  const std::string& name() const { return name_; }
  size_type size() const { return ladder_implants_.size(); }

  const std::vector<Implant>& operator[](int i) const { return ladder_implants_[i]; }

  const_iterator begin() const { return ladder_implants_.begin(); }
  const_iterator end() const { return ladder_implants_.end(); }

private:
  std::string name_;
  // this slot has a vector of implant vectors.
  // implants within the same implant vector have the same cluster specification, but different abi reqs.
  std::vector<std::vector <Implant> > ladder_implants_;
};

#endif // LADDER_SLOT_H_
