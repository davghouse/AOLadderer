#include <algorithm>
#include <iostream>
#include "ladder.h"

using std::vector; using std::string;
using std::endl;
using std::max; using std::sort;
using namespace ladder_helper;

Ladder::Ladder(const ImplantConfiguration& required_config, const CharacterStats& stats)
  : required_config_(required_config), stats_(stats) {}

void Ladder::HeightTwo(const vector<LadderSlot> &ladder_slots)
{
  bool config_not_empty = false;
  Ladder ladder1(this->required_config_, this->stats_);
  ladder1.HeightOne(ladder_slots);
  // ladder1 has a process_, process_[0] contains all of the ladder implants.
  // The ladder implants are marked with a lock_ = true. Must construct a valid
  // required_config from these laddering implants.
  ImplantConfiguration configuration1;
  for(vector<int>::const_iterator it = ladder1.process_[0].order_.begin();
      it != ladder1.process_[0].order_.end(); ++it){
    Implant implant = ladder1.process_[0].config_[*it];
    if(implant.ability_name() != "abi" && implant.lock_){
      config_not_empty = true;
      Implant& imp = configuration1.config_[*it];
      imp = implant;
      ConvertLadderImplantToFullImplant(imp, true);
    }
  }
  if(config_not_empty){
    Ladder ladder2(configuration1, this->stats_);
    ladder2.HeightOne(ladder_slots);
    this->process_.push_back(ladder2.process_[0]);
    this->process_.push_back(ladder2.process_[1]);
    // Scrub the current implants in the process; convert them into full-fledged implants.
    for(int i = 0; i <= 1; ++i){
      for(vector<int>::const_iterator it = process_[i].order_.begin();
          it != process_[i].order_.end(); ++it){
        Implant& implant = process_[i].config_[*it];
        if(implant.ability_name() != "abi"){
          ConvertLadderImplantToFullImplant(implant, false);
        }
      }
    }
    // Add to our stats all the currently equipped implants; those in the 2nd process,
    // and those in the 1st process that aren't in slots occupied by the 2nd process.
    // Also, build up a list of the currently equipped implants.
    vector<Implant> equipped_implants;
    for(unsigned int i = 0; i < process_[0].size(); ++i){
      const Implant& implant1 = process_[0].config_[i];
      const Implant& implant2 = process_[1].config_[i];
      if(implant2.ability_name() != "abi"){
        stats_.UpdateStats(implant2, true, implant2.ql());
        equipped_implants.push_back(implant2);
      }
      else if(implant1.ability_name() != "abi"){
        stats_.UpdateStats(implant1, true, implant1.ql());
        equipped_implants.push_back(implant1);
      }
    }
    // Build up the list of required implants, in the order in which they're inserted.
    // ladder1 contains process[0], process[1].

    vector<Implant> required_implants;
    for(vector<int>::const_iterator it = ladder1.process_[0].order_.begin();
      it != ladder1.process_[0].order_.end(); ++it){
      const Implant& implant = ladder1.process_[0].config_[*it];
      if(implant.ability_name() != "abi" && !implant.lock_){
        required_implants.push_back(Implant(implant));
      }
    }
    for(vector<int>::const_iterator it = ladder1.process_[1].order_.begin();
        it != ladder1.process_[1].order_.end(); ++it){
      const Implant& implant = ladder1.process_[1].config_[*it];
      if(implant.ability_name() != "abi" && !implant.lock_){
        bool taken = false;
        for(unsigned int i = 0; i < required_implants.size(); ++i){
          if(required_implants[i].slot_int() == implant.slot_int()){
            taken = true;
            break;
          }
        }
        if(!taken){
          required_implants.push_back(Implant(implant));
        }
      }
    }
    // Now the implants need to be equipped.
    for(unsigned int i = 0; i < required_implants.size(); ++i){
      for(unsigned int j = 0; j < equipped_implants.size(); ++j){
        if(equipped_implants[j].slot_name() == required_implants[i].slot_name()){
          stats_.UpdateStats(equipped_implants[j], false);
          break;
        }
      }
      int ql = stats_.UpdateStats(required_implants[i]);
      required_implants[i].ql_ = ql;
    }
    process_.push_back(ImplantConfiguration());
    process_[2].config_ = required_implants;
    //this->process_ = ladder2.process_;
    //this->required_config_ = ladder2.required_config_;
    //this->working_config_ = ladder2.working_config_;
    //this->stats_ = ladder2.stats_;
    //this->required_ladder_implants_ = ladder2.required_ladder_implants_;

  }
  return;
}

void Ladder::ConvertLadderImplantToFullImplant(Implant& imp, bool resetQl)
{
  if(resetQl){
    imp.set_ql(0);
  }
  imp.set_lock(false);
  imp.set_remove(true);
  std::string query_text = "SELECT shining, bright, faded FROM implants ";
  query_text += "WHERE slot='" + SlotAbbrToFull(imp.slot_name()) + "' ";
  query_text += "and req='" + AbilityAndTreatmentAbbrToFull(imp.ability_name()) + "' ";
  if(imp.shining_abbr() != "shi"){
    query_text += "and Shining='" + AbilityAndTreatmentAbbrToFull(imp.shining_abbr()) + "' ";
  }
  if(imp.bright_abbr() != "bri"){
    query_text += "and Bright='" + AbilityAndTreatmentAbbrToFull(imp.bright_abbr()) + "' ";
  }
  if(imp.faded_abbr() != "fad"){
    query_text += "and Faded='" + AbilityAndTreatmentAbbrToFull(imp.faded_abbr()) + "' ";
  }
  QSqlQuery q;
  q.exec(QString::fromStdString(query_text));
  int most_empties = -1;
  std::string shi, bri, fad;
  while(q.next()){
    int emptyCount = 0;
    string s = q.value(0).toString().toStdString();
    string b = q.value(1).toString().toStdString();
    string f = q.value(2).toString().toStdString();
    if(s == "Empty") ++emptyCount;
    if(b == "Empty") ++emptyCount;
    if(f == "Empty") ++emptyCount;
    if(emptyCount > most_empties || (emptyCount == most_empties && s == "Empty")){
      most_empties = emptyCount;
      shi = s;
      bri = b;
      fad = f;
    }
  }
  imp.shining_full_ = shi;
  imp.bright_full_ = bri;
  imp.faded_full_ = fad;
  imp.shining_int_ = ClusterToInt(imp.shining_abbr_);
  imp.bright_int_ = ClusterToInt(imp.bright_abbr_);
  imp.faded_int_ = ClusterToInt(imp.faded_abbr_);

  std::string query_begin = "SELECT aoid FROM implants WHERE slot='" + SlotAbbrToFull(imp.slot_name()) + "'";
  string query_text_2 = query_begin + " AND Shining='"+shi+"' AND Bright='"+bri+"' AND Faded='"+fad+"'";
  QSqlQuery q2;
  q2.exec(QString::fromStdString(query_text_2));
  q2.next();
  imp.aoid_ = q2.value(0).toInt();
}

void Ladder::HeightOne(const vector<LadderSlot>& ladder_slots)
{
  double max_avg_ql = 0;
  bool increasing_avg_ql = true;
  vector<Implant> ladder_implants;
  // Look for ladder implants.
  while(increasing_avg_ql && ladder_implants.size() != 10){
    // Load stats that would result from equipping the current ladder implants into a new stats object.
    CharacterStats laddered_stats(stats_);
    UpdateStatsFromLadderImplants(ladder_implants, laddered_stats);
    increasing_avg_ql = false;
    ladder_implants.push_back(Implant());
    vector<LadderSlot>::const_iterator slot_pos = ladder_slots.begin();
    int subset_pos = 0;
    int implant_pos = 0;
    // Choose ladder implant:
    // Choose slot.
    vector<LadderSlot>::const_iterator it;
    for(it = ladder_slots.begin(); it != ladder_slots.end(); ++it){
      if(SlotTaken(it->name(), ladder_implants)){
        continue;
      }
      // Determine if this slot has a final implant occupying it.
      bool remove = true;
      if(required_config_[SlotNameToInt(it->name())].ability_name() == "abi"){
        remove = false;
      }
      // Choose implant subset within slot (subset: same important clusters, different ability req.)
      LadderSlot::size_type i;
      for(i = 0; i != it->size(); ++i){
        // Choose implant for which our ability is highest.
        int best = 0;
        for(vector<Implant>::size_type j = 0; j != (*it)[i].size(); ++j){
          if(laddered_stats.ability_from_name((*it)[i][j].ability_name()) > laddered_stats.ability_from_name((*it)[i][best].ability_name())){
            best = j;
          }
        }
        // Test implant.
        ladder_implants.back() = (*it)[i][best];
        ladder_implants.back().set_remove(remove);
        Ladder trial_ladder(required_config_, stats_);
        trial_ladder.EquipLadderImplants(ladder_implants);
        trial_ladder.HeightZero();
        double trial_avg_ql = trial_ladder.AverageQL();
        // + .01 added because optimizations in release mode were causing rounding problems.
        if(trial_avg_ql > max_avg_ql + .01){
          increasing_avg_ql = true;
          max_avg_ql = trial_avg_ql;
          slot_pos = it;
          subset_pos = i;
          implant_pos = best;
        }
        else if(abs(trial_avg_ql - max_avg_ql) < .01 && (max_avg_ql < 200) &&
                laddered_stats.get_treatment() < 951)
        {
          // They're close; go with the one with the higher treatment cluster, else do nothing.
          int trial_score = 0;
          int max_score = 0;
          if(ladder_implants.back().shining_abbr_ == "tre")
          {
            trial_score = 3;
          }
          else if(ladder_implants.back().bright_abbr_ == "tre")
          {
            trial_score = 2;
          }
          else if(ladder_implants.back().faded_abbr_ == "tre")
          {
            trial_score = 1;
          }
          if((*slot_pos)[subset_pos][implant_pos].shining_abbr_ == "tre")
          {
            max_score = 3;
          }
          if((*slot_pos)[subset_pos][implant_pos].bright_abbr_ == "tre")
          {
            max_score = 2;
          }
          if((*slot_pos)[subset_pos][implant_pos].faded_abbr_ == "tre")
          {
            max_score = 1;
          }
          if(trial_score > max_score)
          {
            increasing_avg_ql = true;
            max_avg_ql = trial_avg_ql;
            slot_pos = it;
            subset_pos = i;
            implant_pos = best;
          }
        }
      }
    }
    ladder_implants.back() = (*slot_pos)[subset_pos][implant_pos];
    if(required_config_[SlotNameToInt(slot_pos->name())].ability_name() == "abi"){
      ladder_implants.back().set_remove(false);
    }
  }
  if(!increasing_avg_ql){
    // Didn't find a useful ladder implant last time, so remove last added.
    ladder_implants.pop_back();
  }

  Ladder final_ladder(required_config_,stats_);
  final_ladder.EquipLadderImplants(ladder_implants);
  final_ladder.HeightZero();
  process_ = final_ladder.process_;
  // Makes output easier.
  if(process_.size() == 1){
    process_.push_back(process_[0]);
  }
  stats_ = final_ladder.stats_;
}


void Ladder::EquipLadderImplants(const vector<Implant>& ladder_implants)
{
  for(vector<Implant>::size_type i = 0; i != ladder_implants.size(); ++i){
    int slot_int = ladder_implants[i].slot_int();
    int ql = stats_.UpdateStats(ladder_implants[i]);
    working_config_.UpdateConfig(ladder_implants[i]);
    working_config_[slot_int].set_ql(ql);
    // Lock the implant (close the slot). To be removed during UnequipLadderImplants.
    working_config_[slot_int].set_lock(true);
  }
}

void Ladder::UpdateStatsFromLadderImplants(const std::vector<Implant> &ladder_implants, CharacterStats& stats)
{
  for(vector<Implant>::size_type i = 0; i != ladder_implants.size(); ++i){
    stats.UpdateStats(ladder_implants[i]);
  }
}

// When this function is called, EquipLadderImplants has already been called from HeightOne,
// populating this Ladder's working_config_ with the accessory ladder implants.
void Ladder::HeightZero()
{
  FindRequiredLadderImplants();
  vector<uint> ordered;
  // Find optimal ordering of the required ladder implants.
  Order(ordered);
  // Equip those implants.
  for(vector<uint>::size_type i = 0; i != ordered.size(); ++i){
    int j = ordered[i];
    int slot_int = required_ladder_implants_[j].slot_int();
    int ql = stats_.UpdateStats(required_ladder_implants_[j]);
    working_config_.UpdateConfig(required_ladder_implants_[j]);
    working_config_[slot_int].set_ql(ql);
  }
  // Equip the remaining required implants whose slots are unoccupied.
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder() && !working_config_[i].lock()){
      int slot_int = required_config_[i].slot_int();
      int ql = stats_.UpdateStats(required_config_[i]);
      working_config_.UpdateConfig(required_config_[i]);
      working_config_[slot_int].set_ql(ql);
    }
  }
  UnequipLadderImplants(); // And equip the required implants that go in their spots.
}

void Ladder::FindRequiredLadderImplants()
{
  for(ImplantConfiguration::size_type i = 0; i != required_config_.size(); ++i)
    // Implant must be required, used for laddering, and slot must be free.
    if(required_config_[i].used_to_ladder() && !working_config_[i].lock())
      required_ladder_implants_.push_back(required_config_[i]);
}

void Ladder::Order(vector<uint>& order)
{
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i)
    order.push_back(i);
  vector<uint> current;
  RunOrder(order, current);
}

void Ladder::RunOrder(vector<uint>& best, vector<uint>& current)
{
  if(current.size() == required_ladder_implants_.size()){
    if(!Compare(best, current))
      best = current;
    return;
  }
  // Recursively evaluate all permutations of the implants that happen to be useful for laddering.
  for(vector<Implant>::size_type i = 0; i != required_ladder_implants_.size(); ++i){
    bool slot_empty = true;
    for(vector<uint>::size_type j = 0; j != current.size(); ++j){
      if(current[j] == i){
        slot_empty = false;
        break;
      }
    }
    if(slot_empty){
      current.push_back(i);
      // RECURSE -- current becomes current plus next available.
      RunOrder(best, current);
      // Pass by reference, then pop_back when done to continue recursing at this level.
      current.pop_back();
    }
  }
}

inline bool Ladder::Compare(const vector<uint>& best, const vector<uint>& current) const
{
  if(!best.size())
    return false;
  return AverageQLFromOrdering(best) >= AverageQLFromOrdering(current);
}

double Ladder::AverageQLFromOrdering(const vector<uint>& order) const
{
  double avg_ql = 0;
  CharacterStats temporary_stats = stats_;
  for(vector<uint>::size_type i = 0; i != order.size(); ++i){
    avg_ql += temporary_stats.UpdateStats(required_ladder_implants_[order[i]]);
  }
  for(vector<uint>::size_type i = 0; i != required_config_.size(); ++i){
    if(!required_config_[i].used_to_ladder()){
     avg_ql += temporary_stats.UpdateStats(required_config_[i]);
    }
  }
  return avg_ql/(required_config_.size());
}

void Ladder::UnequipLadderImplants()
{
  process_.push_back(working_config_);
  bool ladder_implant_found = false;
  ImplantConfiguration temp_config;
  // Go through the config, removing ladder implants in reverse order, putting in the
  // required implant immediately. Ladder implants not in the way of a final implant should
  // not be touched.
  for(vector<int>::const_reverse_iterator ri = working_config_.rbegin(); ri != working_config_.rend(); ++ri){
    if(working_config_[*ri].lock() && working_config_[*ri].remove()){
      ladder_implant_found = true;
      // remove it
      stats_.UpdateStats(working_config_[*ri], false);
      // Update stats/config with required implant.
      int slot_int = required_config_[*ri].slot_int();
      int ql = stats_.UpdateStats(required_config_[*ri]);
      temp_config.UpdateConfig(required_config_[*ri]);
      temp_config[slot_int].set_ql(ql);
    }
  }
  // Include the implants equipped previously for use in GUI's Step Two output.
  if(ladder_implant_found){
    for(vector<int>::const_iterator it = working_config_.begin(); it != working_config_.end(); ++it){
      if(!working_config_[*it].lock())
        temp_config.UpdateConfig(working_config_[*it]);
    }
    // ?Take outside of if statement and remove process.push_back from HeightOne?
    process_.push_back(temp_config);
  }
}

double Ladder::AverageQL() const
{
  if(!process_.size())
    return 0;
  return process_.back().AverageQL();
}
