#ifndef IMPLANT_H_
#define IMPLANT_H_

#include <string>
#include <fstream>

class Implant{
  // added for GUI
  friend class MainWindow;

public:
  Implant();

  Implant(int ql, std::string slot, std::string ability_name_, std::string shining_abbr_, std::string bright_abbr_, std::string faded_abbr_, bool locked = false, bool removed = true);

  bool ladder() const{ return used_to_ladder_; }
  int ql() const { return ql_; }
  const std::string& slot() const { return slot_name_; }
  int slotI() const { return slot_int_; }
  const std::string& abi() const { return ability_name_; }
  int abiI() const { return ability_int_; }
  const std::string& shi() const { return shining_abbr_; }
  int shiI() const { return shining_int_; }
  const std::string& bri() const { return bright_abbr_; }
  int briI() const { return bright_int_; }
  const std::string& fad() const { return faded_abbr_; }
  int fadI() const { return faded_int_; }

  bool isLocked() const { return lock_; }
  bool mustRemove() const { return remove_; }

  void setQL(int QL){ ql_ = QL; }
  void setRemove(bool removeBool){ remove_ = removeBool; }

private:
  // quality_level_
  int ql_;
  
  std::string slot_name_;
  std::string ability_name_;
  std::string shining_abbr_, bright_abbr_, faded_abbr_;

  // added for GUI
  std::string shining_full_, bright_full_, faded_full_;
  
  // following integers are for indexing purposes, allowing quick access in containers like 
  // Stats and Config, where abilities and implants respectively are in a fixed order (by design).
  // they are named to correspond with the string variables above.
  // abi order: strength, agility, stamina, intelligence, sense, psychic
  //            0         1        2        3             4      5
  // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  int slot_int_;
  int ability_int_;
  int shining_int_, bright_int_, faded_int_;

  // if clusters contain tre or one of the abis
  bool used_to_ladder_;
  // used to prevent laddering implants from being moved
  bool lock_;
  // used to allow ladder implants to avoid removal if no final implant occupies their slot
  bool remove_;
};

#endif // IMPLANT_H_
