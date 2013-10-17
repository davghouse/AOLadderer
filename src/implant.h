#ifndef IMPLANT_H_
#define IMPLANT_H_

#include <string>
#include <fstream>

// Holds information about an implant.
class Implant{
  friend class MainWindow;

public:
  Implant();

  Implant(int ql, std::string slot_name, std::string ability_name,
          std::string shining_abbr, std::string bright_abbr,
          std::string faded_abbr, bool lock = false, bool remove = true);

  int ql() const { return ql_; }
  const std::string& slot_name() const { return slot_name_; }
  const std::string& ability_name() const { return ability_name_; }
  const std::string& shining_abbr() const { return shining_abbr_; }
  const std::string& bright_abbr() const { return bright_abbr_; }
  const std::string& faded_abbr() const { return faded_abbr_; }
  int slot_int() const { return slot_int_; }
  int ability_int() const { return ability_int_; }
  int shining_int() const { return shining_int_; }
  int bright_int() const { return bright_int_; }
  int faded_int() const { return faded_int_; }

  bool used_to_ladder() const{ return used_to_ladder_; }
  bool lock() const { return lock_; }
  bool remove() const { return remove_; }

  void set_ql(int ql){ ql_ = ql; }
  void set_remove(bool remove){ remove_ = remove; }

private:
  // Quality level.
  int ql_;
  
  // Slot that this implant occupies.
  std::string slot_name_;
  // Name of required ability; "abi" if the implant is empty
  std::string ability_name_;

  // Abbreviations for the clusters;
  // tre for treatment, str/agi/sta/int/sen/psy for abilities,
  // and shi/bri/fad to represent not used_to_ladder.
  // Used within the ladder algorithm.
  std::string shining_abbr_, bright_abbr_, faded_abbr_;

  // Full name for the clusters, used for GUI output.
  std::string shining_full_, bright_full_, faded_full_;
  
  // Indexing integers for quick access from CharacterStats and ImplantConfiguration objects.
  // Slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  // Skill order: strength, agility, stamina, intelligence, sense, psychic, treatment, else
  //              0         1        2        3             4      5        6          -1
  int slot_int_;
  int ability_int_;
  int shining_int_, bright_int_, faded_int_;

  // True if clusters contain treatment or one of the abilities.
  bool used_to_ladder_;
  // Prevents laddering implants from being moved during algorithm stages.
  bool lock_;
  //  Allows ladder implants to avoid reverse-removal if no final implant occupies their slot
  bool remove_;
};

#endif // IMPLANT_H_
