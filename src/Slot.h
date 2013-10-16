#ifndef SLOT_H_
#define SLOT_H_

#include <vector>
#include <string>
#include "Implant.h"

 // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
class Slot{
public:
  typedef std::vector<std::vector<Implant> >::size_type size_type;
  typedef std::vector<std::vector<Implant> >::const_iterator const_iterator;

  Slot();
  Slot(std::string n) : n(n) {}

  void add(Implant i){ limps[limps.size() - 1].push_back(i); }
  void addVec(){ limps.push_back(std::vector<Implant>()); }
  const std::string& name() const { return n; }
  size_type size() const { return limps.size(); }

  const std::vector<Implant>& operator[](int i) const { return limps[i]; }

  const_iterator begin() const { return limps.begin(); }
  const_iterator end() const { return limps.end(); }

private:
  std::string n;
  // this slot has a vector of implant vectors.
  // implants within the same implant vector have the same cluster specification, but different abi reqs.
  std::vector<std::vector <Implant> > limps;
};

#endif // SLOT_H_
