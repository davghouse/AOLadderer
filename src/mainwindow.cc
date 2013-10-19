#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "ladder_helper.h"
#include "QFile"
#include "QMessageBox"
#include "QFileDialog"
#include <iostream>
#include <sstream>

using std::vector;
using std::string;
using std::cerr;
using std::endl;
using namespace ladder_helper;

struct ShoppingItem{
  std::string cluster_;
  int ql_;
  bool operator<(const ShoppingItem& r) const{ return cluster_ < r.cluster_; }
};

MainWindow::MainWindow(QWidget *parent) :
  QMainWindow(parent),
  ui(new Ui::MainWindow),
  current_file_("")
{
  ui->setupUi(this);
  config_not_empty_ = false;
  // Database
  standard_implants_ = QSqlDatabase::addDatabase("QSQLITE");
  standard_implants_.setDatabaseName("standard_implants.db");
  standard_implants_.open();
  // Ladder implants
  std::ifstream in("ladder_implants.txt");
  string ladder_slot, ability, shi, bri, fad;
  while(in >> ability){
    if(ability == "$"){
      in >> ladder_slot;
      ladder_slots_.push_back(LadderSlot(ladder_slot));
    }
    else if(ability == "#")
      ladder_slots_[ladder_slots_.size() - 1].AddLadderImplantSubset();
    else{
      in >> shi >> bri >> fad;
      // Add a ladder implant to the most recent slot
      ladder_slots_[ladder_slots_.size() - 1].AddLadderImplant(Implant(0, ladder_slot, ability, shi, bri, fad, true));
    }
  }
  // Buffs:
  connect(ui->SCE, SIGNAL(clicked(bool)), this, SLOT(ToggleSurgeryClinicEffect(bool)));
  connect(ui->SFA, SIGNAL(clicked(bool)), this, SLOT(ToggleSuperiorFirstAid(bool)));
  connect(ui->CM, SIGNAL(clicked(bool)), this, SLOT(ToggleCompositeMedical(bool)));
  connect(ui->CA, SIGNAL(clicked(bool)), this, SLOT(ToggleCompositeAttribute(bool)));
  connect(ui->ES, SIGNAL(clicked(bool)), this, SLOT(ToggleEnhancedSenses(bool)));
  connect(ui->FG, SIGNAL(clicked(bool)), this, SLOT(ToggleFelineGrace(bool)));
  connect(ui->EOB, SIGNAL(clicked(bool)), this, SLOT(ToggleEssenceOfBehemoth(bool)));
  connect(ui->ICRT, SIGNAL(clicked(bool)), this, SLOT(ToggleImprovedCutRedTape(bool)));
  connect(ui->G, SIGNAL(clicked(bool)), this, SLOT(ToggleGauntlet(bool)));
  connect(ui->EB, SIGNAL(clicked(bool)), this, SLOT(ToggleExtruderBar(bool)));

  connect(ui->IC, SIGNAL(clicked(bool)), this, SLOT(ToggleIronCircle(bool)));
  connect(ui->PS, SIGNAL(clicked(bool)), this, SLOT(ToggleProdigiousStrength(bool)));

  connect(ui->NS, SIGNAL(clicked(bool)), this, SLOT(ToggleNeuronalStimulator(bool)));
  connect(ui->OME, SIGNAL(clicked(bool)), this, SLOT(ToggleOdinsMissingEye(bool)));

  // Build:
  connect(ui->Build, SIGNAL(clicked()), SLOT(RunHeightOne()));

  // Shopping list:
  connect(ui->Shining, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtShining(QModelIndex)));
  connect(ui->Bright, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtBright(QModelIndex)));
  connect(ui->Faded, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtFaded(QModelIndex)));

  // File menu:
  connect(ui->actionOpen, SIGNAL(triggered()), SLOT(Open()));
  connect(ui->actionSave, SIGNAL(triggered()), SLOT(Save()));
  connect(ui->actionSave_as, SIGNAL(triggered()), SLOT(SaveAs()));
  connect(ui->actionExport_to_Auno, SIGNAL(triggered()), SLOT(ExportToAuno()));
}

MainWindow::~MainWindow()
{
  delete ui;
  standard_implants_.close();
}

// Member functions:

// Some better way to do this... use table widget?
void MainWindow::GetConfiguration(ImplantConfiguration& required_config)
{
  std::string shi, bri, fad;
  // head 0 Head
  shi = ui->headShi->currentText().toStdString();
  bri = ui->headBri->currentText().toStdString();
  fad = ui->headFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 0,"Head", shi, bri, fad);
  }
  // eye 1 Eye
  shi = ui->eyeShi->currentText().toStdString();
  bri = ui->eyeBri->currentText().toStdString();
  fad = ui->eyeFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 1,"Eye", shi, bri, fad);
  }
  // ear 2 Ear
  shi = ui->earShi->currentText().toStdString();
  bri = ui->earBri->currentText().toStdString();
  fad = ui->earFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config,2 ,"Ear", shi, bri, fad);
  }
  // chest 3 Chest
  shi = ui->chestShi->currentText().toStdString();
  bri = ui->chestBri->currentText().toStdString();
  fad = ui->chestFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 3,"Chest", shi, bri, fad);
  }
  // rarm 4 Right-Arm
  shi = ui->rarmShi->currentText().toStdString();
  bri = ui->rarmBri->currentText().toStdString();
  fad = ui->rarmFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 4,"Right-Arm", shi, bri, fad);
  }
  // larm 5 Left-Arm
  shi = ui->larmShi->currentText().toStdString();
  bri = ui->larmBri->currentText().toStdString();
  fad = ui->larmFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 5,"Left-Arm", shi, bri, fad);
  }
  // waist 6 Waist
  shi = ui->waistShi->currentText().toStdString();
  bri = ui->waistBri->currentText().toStdString();
  fad = ui->waistFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 6,"Waist", shi, bri, fad);
  }
  // rwrist 7 Right-Wrist
  shi = ui->rwristShi->currentText().toStdString();
  bri = ui->rwristBri->currentText().toStdString();
  fad = ui->rwristFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 7,"Right-Wrist", shi, bri, fad);
  }
  // lwrist 8 Left-Wrist
  shi = ui->lwristShi->currentText().toStdString();
  bri = ui->lwristBri->currentText().toStdString();
  fad = ui->lwristFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 8,"Left-Wrist", shi, bri, fad);
  }
  // leg 9 Leg
  shi = ui->legShi->currentText().toStdString();
  bri = ui->legBri->currentText().toStdString();
  fad = ui->legFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 9,"Leg", shi, bri, fad);
  }
  // rhand 10 Right-Hand
  shi = ui->rhandShi->currentText().toStdString();
  bri = ui->rhandBri->currentText().toStdString();
  fad = ui->rhandFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 10,"Right-Hand", shi, bri, fad);
  }
  // lhand 11 Left-Hand
  shi = ui->lhandShi->currentText().toStdString();
  bri = ui->lhandBri->currentText().toStdString();
  fad = ui->lhandFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 11,"Left-Hand", shi, bri, fad);
  }
  // feet 12 Feet
  shi = ui->feetShi->currentText().toStdString();
  bri = ui->feetBri->currentText().toStdString();
  fad = ui->feetFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(required_config, 12,"Feet", shi, bri, fad);
  }
}

void MainWindow::GetConfigurationHelper(ImplantConfiguration& required_config, int i,
                                        const string& slot, string& shi, string& bri, string& fad)
{
  config_not_empty_ = true;
  required_config.config_[i].shining_full_ = shi;
  required_config.config_[i].bright_full_ = bri;
  required_config.config_[i].faded_full_ = fad;
  std::string query_begin = "SELECT req FROM implants WHERE slot='" + slot + "'";
  string query_text = query_begin + " AND Shining='"+shi+"' AND Bright='"+bri+"' AND Faded='"+fad+"'";
  QSqlQuery query;
  query.exec(QString::fromStdString(query_text));
  query.next();
  required_config.config_[i].ability_name_ = AbilityFullToAbbr(query.value(0).toString().toStdString());
  required_config.config_[i].ability_int_ = AbilityToInt(required_config.config_[i].ability_name_);
  bool used_to_ladder = false;
  if(AbilityToIntAndMakeFull(shi) >= 0){
    required_config.config_[i].shining_abbr_ = shi;
    required_config.config_[i].shining_int_ = ClusterToInt(required_config.config_[i].shining_abbr_);
    used_to_ladder = true;
  }
  if(AbilityToIntAndMakeFull(bri) >= 0){
    required_config.config_[i].bright_abbr_ = bri;
    required_config.config_[i].bright_int_ = ClusterToInt(required_config.config_[i].bright_abbr_);
    used_to_ladder = true;
  }
  if(AbilityToIntAndMakeFull(fad) >= 0){
    required_config.config_[i].faded_abbr_ = fad;
    required_config.config_[i].faded_int_ = ClusterToInt(required_config.config_[i].faded_abbr_);
    used_to_ladder = true;
  }
  required_config.config_[i].used_to_ladder_ = used_to_ladder;
}

void MainWindow::GetStats(CharacterStats& base_stats)
{
  vector<int> abilities;
  abilities.resize(6);
  abilities[0] = ui->Strength->value();
  abilities[1] = ui->Agility->value();
  abilities[2] = ui->Stamina->value();
  abilities[3] = ui->Intelligence->value();
  abilities[4] = ui->Sense->value();
  abilities[5] = ui->Psychic->value();
  base_stats.UpdateStats(abilities, ui->Treatment->value());
}

void MainWindow::ShowHeightOne(const Ladder & ladder)
{
  // For shopping list
  vector<ShoppingItem> shining_shopping, bright_shopping, faded_shopping;
<<<<<<< HEAD
  // Step tab:
=======
  // Results tab:
>>>>>>> origin/develop
  // Step One
  bool equipped_required_implant_in_step_one = false;
  int first_after_dash = 0;
  for(vector<int>::const_iterator it = ladder.process_[0].order_.begin();
      it != ladder.process_[0].order_.end(); ++it){
    Implant implant = ladder.process_[0].config_[*it];
    if(implant.ability_name() != "abi" && implant.ql() > 0){
      // Ladder implant; figure out how to construct it.
      if(implant.lock_){
        std::string query_text = "SELECT shining, bright, faded FROM implants ";
        query_text += "WHERE slot='" + SlotAbbrToFull(implant.slot_name()) + "' ";
        query_text += "and req='" + AbilityAndTreatmentAbbrToFull(implant.ability_name()) + "' ";
        if(implant.shining_abbr() != "shi"){
          query_text += "and Shining='" + AbilityAndTreatmentAbbrToFull(implant.shining_abbr()) + "' ";
        }
        if(implant.bright_abbr() != "bri"){
          query_text += "and Bright='" + AbilityAndTreatmentAbbrToFull(implant.bright_abbr()) + "' ";
        }
        if(implant.faded_abbr() != "fad"){
          query_text += "and Faded='" + AbilityAndTreatmentAbbrToFull(implant.faded_abbr()) + "' ";
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
        ShowImplant(implant, shi, bri, fad, 1);
        if(shi != "-----"){
          ShoppingItem t;
          t.cluster_ = shi; t.ql_ = implant.ql();
          shining_shopping.push_back(t);
        }
        if(bri != "-----"){
          ShoppingItem t;
          t.cluster_ = bri; t.ql_ = implant.ql();
          bright_shopping.push_back(t);
        }
        if(fad != "-----"){
          ShoppingItem t;
          t.cluster_ = fad; t.ql_ = implant.ql();
          faded_shopping.push_back(t);
        }
      }
      // Required implant -- already know how to construct it.
      else{
        if(!equipped_required_implant_in_step_one){
          equipped_required_implant_in_step_one = true;
          ui->stepOne->addItem(QString::fromStdString(std::string(77, '-')));
          first_after_dash = *it;
        }
        std::string shi, bri, fad;
        shi = ladder.required_config_[*it].shining_full_;
        bri = ladder.required_config_[*it].bright_full_;
        fad = ladder.required_config_[*it].faded_full_;
        ShowImplant(implant, shi, bri, fad, 1);
      }
    }
    ui->avgQLSpinBox->setValue(ladder.AverageQL());
  }
  if(!equipped_required_implant_in_step_one){
    ui->stepOne->addItem(QString::fromStdString(std::string(77, '-')));
  }
  // Step Two
  for(vector<int>::const_iterator it = ladder.process_[1].order_.begin();
      it != ladder.process_[1].order_.end(); ++it){
    Implant implant = ladder.process_[1].config_[*it];
    if(implant.ability_name() != "abi" && implant.ql() > 0 && implant.remove()){
      if(equipped_required_implant_in_step_one){
        if(*it == first_after_dash){
          ui->stepTwo->addItem(QString::fromStdString(std::string(77, '-')));
        }
      }
      std::string shi, bri, fad;
      shi = ladder.required_config_[*it].shining_full_;
      bri = ladder.required_config_[*it].bright_full_;
      fad = ladder.required_config_[*it].faded_full_;
      ShowImplant(implant, shi, bri, fad, 2);
      if(shi != "-----"){
        ShoppingItem t;
        t.cluster_ = shi; t.ql_ = implant.ql();
        shining_shopping.push_back(t);
      }
      if(bri != "-----"){
        ShoppingItem t;
        t.cluster_ = bri; t.ql_ = implant.ql();
        bright_shopping.push_back(t);
      }
      if(fad != "-----"){
        ShoppingItem t;
        t.cluster_ = fad; t.ql_ = implant.ql();
        faded_shopping.push_back(t);
      }
    }
  }
  if(!equipped_required_implant_in_step_one)
    ui->stepTwo->addItem(QString::fromStdString(std::string(77, '-')));
<<<<<<< HEAD
  // Shopping list tab:
=======
  // Shopping tab:
>>>>>>> origin/develop
  std::sort(shining_shopping.begin(), shining_shopping.end());
  std::sort(bright_shopping.begin(), bright_shopping.end());
  std::sort(faded_shopping.begin(), faded_shopping.end());
  for(vector<ShoppingItem>::iterator it = shining_shopping.begin(); it != shining_shopping.end(); ++it){
    int cluster_ql = .86*(it->ql_) + .01;
    std::stringstream t;
    t << "ql " << cluster_ql << " " << it->cluster_;
    ui->Shining->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = bright_shopping.begin(); it != bright_shopping.end(); ++it){
    int cluster_ql = .84*(it->ql_) + .01;
    std::stringstream t;
    t << "ql " << cluster_ql << " " << it->cluster_;
    ui->Bright->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = faded_shopping.begin(); it != faded_shopping.end(); ++it){
    int cluster_ql = .82*(it->ql_) + .01;
    std::stringstream t;
    t << "ql " << cluster_ql << " " << it->cluster_;
    ui->Faded->addItem(QString::fromStdString(t.str()));
  }
}

void MainWindow::ShowImplant(const Implant& implant, std::string& shi, std::string& bri, std::string& fad, int step)
{
  if(shi == "Empty") shi = "-----";
  if(bri == "Empty") bri = "-----";
  if(fad == "Empty") fad = "-----";
  string temp;
  temp += "[" + implant.ability_name() + "] ";
  std::stringstream ss;
  ss << implant.ql();
  temp += "ql " + ss.str() + " " + SlotAbbrToFull(implant.slot_name()) + ":";
  temp += std::string(26 - temp.size(), ' ');
  temp += shi + "," + std::string(16 - shi.size(), ' ');
  temp += bri + "," + std::string(16 - bri.size(), ' ');
  temp += fad;
  if(step == 1){
    ui->stepOne->addItem(QString::fromStdString(temp));
  }
  if(step == 2){
    ui->stepTwo->addItem(QString::fromStdString(temp));
  }
}

// Slots:

// Build:
void MainWindow::RunHeightOne()
{
  ui->stepOne->clear();
  ui->stepTwo->clear();
  ui->avgQLSpinBox->setValue(0);
  ui->Shining->clear();
  ui->Bright->clear();
  ui->Faded->clear();
  config_not_empty_ = false;
  ImplantConfiguration requiredConfig;
  CharacterStats baseStats;
  GetConfiguration(requiredConfig);
  GetStats(baseStats);
  // height one
  if(config_not_empty_){
    Ladder ladder(requiredConfig,baseStats);
#ifndef QT_NO_CURSOR
    QApplication::setOverrideCursor(Qt::WaitCursor);
#endif
    ladder.HeightOne(ladder_slots_);
    ShowHeightOne(ladder);
#ifndef QT_NO_CURSOR
    QApplication::restoreOverrideCursor();
#endif
    ui->tabWidget->setCurrentWidget(ui->resultsTab);
  }
}


// Buffs:
void MainWindow::ToggleSurgeryClinicEffect(bool add)
{
  int val = add ? 100 : -100;
  ui->Treatment->setValue(ui->Treatment->value() + val);
}

void MainWindow::ToggleSuperiorFirstAid(bool add)
{
  int val = add ? 80 : -80;
  ui->Treatment->setValue(ui->Treatment->value() + val);
}

void MainWindow::ToggleCompositeMedical(bool add)
{
  int val = add ? 20 : -20;
  ui->Treatment->setValue(ui->Treatment->value() + val);
}

void MainWindow::ToggleCompositeAttribute(bool add)
{
  int val = add ? 12 : -12;
                   // agi      int     sen
  double trickle = (.3*val+ .5*val+ .2*val)/4;
  ui->Agility->setValue(ui->Agility->value() + val);
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  ui->Sense->setValue(ui->Sense->value() + val);
  ui->Stamina->setValue(ui->Stamina->value() + val);
  ui->Strength->setValue(ui->Strength->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleEnhancedSenses(bool add)
{
  int val = add ? 15 : -15;
  double trickle = (.2*val)/4;
  ui->Sense->setValue(ui->Sense->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleFelineGrace(bool add)
{
  int val = add ? 25 : -25;
  double trickle = (.3*val)/4;
  ui->Agility->setValue(ui->Agility->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleEssenceOfBehemoth(bool add)
{
  int val = add ? 27 : -27;
  ui->Stamina->setValue(ui->Stamina->value() + val);
  ui->Strength->setValue(ui->Strength->value() + val);
}

void MainWindow::ToggleImprovedCutRedTape(bool add)
{
  int val = add ? 3 : -3;
  double trickle = (.5*val)/4;
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleGauntlet(bool add)
{
  int val = add ? 10 : -10;
  double trickle = (.3*val+ .5*val+ .2*val)/4;
  ui->Agility->setValue(ui->Agility->value() + val);
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  ui->Sense->setValue(ui->Sense->value() + val);
  ui->Stamina->setValue(ui->Stamina->value() + val);
  ui->Strength->setValue(ui->Strength->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleExtruderBar(bool add)
{
  int val = add ? 2 : -2;
  double trickle = (.3*val + .5*val + .2*val)/4;
  ui->Agility->setValue(ui->Agility->value() + val);
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  ui->Sense->setValue(ui->Sense->value() + val);
  ui->Stamina->setValue(ui->Stamina->value() + val);
  ui->Strength->setValue(ui->Strength->value() + val);

  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleIronCircle(bool add)
{
  int val = add ? 20 : -20;
  ui->Stamina->setValue(ui->Stamina->value() + val);
  ui->Strength->setValue(ui->Strength->value() + val);
  if(add && ui->PS->isChecked()){
    ui->PS->setChecked(false);
    ToggleProdigiousStrength(false);
  }
}

void MainWindow::ToggleProdigiousStrength(bool add)
{
  int val = add ? 40 : -40;
  ui->Strength->setValue(ui->Strength->value() + val);
  if(add && ui->IC->isChecked()){
    ui->IC->setChecked(false);
    ToggleIronCircle(false);
  }
}

void MainWindow::ToggleNeuronalStimulator(bool add)
{
  int val = add ? 20 : -20;
  double trickle = (.5*val)/4;
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  if(add && ui->OME->isChecked()){
    ui->OME->setChecked(false);
    ToggleOdinsMissingEye(false);
  }
  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

void MainWindow::ToggleOdinsMissingEye(bool add)
{
  int val = add ? 40 : -40;
  double trickle = (.5*val)/4;
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  if(add && ui->NS->isChecked()){
    ui->NS->setChecked(false);
    ToggleNeuronalStimulator(false);
  }
  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

// Shopping list:
void MainWindow::ToggleBoughtShining(QModelIndex i)
{
  QString temp = ui->Shining->item(i.row())->text();
  if(temp[0] == '~'){
    temp = temp.right(temp.size() - 1);
    ui->Shining->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('~');
    ui->Shining->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtBright(QModelIndex i)
{
  QString temp = ui->Bright->item(i.row())->text();
  if(temp[0] == '~'){
    temp = temp.right(temp.size() - 1);
    ui->Bright->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('~');
    ui->Bright->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtFaded(QModelIndex i)
{
  QString temp = ui->Faded->item(i.row())->text();
  if(temp[0] == '~'){
    temp = temp.right(temp.size() - 1);
    ui->Faded->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('~');
    ui->Faded->item(i.row())->setText(temp);
  }
}

// File menu slots:
void MainWindow::Open()
{
  QString file_name = QFileDialog::getOpenFileName(this);
  if(!file_name.isEmpty())
    LoadFile(file_name);
}

void MainWindow::Save()
{
  if(current_file_.isEmpty())
    return SaveAs();
  return SaveFile(current_file_);
}

void MainWindow::SaveAs()
{
  QString file_name = QFileDialog::getSaveFileName(this, "", "",
                                                   tr("ao-ladderer (*.lad);;All Files (*)"));
  if(file_name.isEmpty())
    return;
  SaveFile(file_name);
}

void MainWindow::ExportToAuno()
{

}

// File menu functions:
void MainWindow::LoadFile(QString& file_name)
{
  QFile file(file_name);
  if(!file.open(QFile::ReadOnly | QFile::Text)){
    QMessageBox::warning(this, tr("ao-ladderer"),
                         tr("Cannot read this file %1:\n%2.").arg(file_name).arg(file.errorString()));
    return;
  }
  QTextStream in(&file);
  LoadBuildTab(in);
  LoadResultsTab(in);
  LoadShoppingTab(in);
  current_file_ = file_name;
  ui->statusBar->showMessage(tr("File loaded"), 1000);
}

void MainWindow::SaveFile(QString& file_name)
{
  QFile file(file_name);
  if(!file.open(QFile::WriteOnly | QFile::Text)){
    QMessageBox::warning(this, tr("ao-ladderer"),
                         tr("Cannot write this file %1:\n%2.").arg(file_name).arg(file.errorString()));
    return;
  }
  QTextStream out(&file);
  SaveBuildTab(out);
  SaveResultsTab(out);
  SaveShoppingTab(out);
  current_file_ = file_name;
  ui->statusBar->showMessage(tr("File saved"), 1000);
}

void MainWindow::SaveBuildTab(QTextStream& out)
{
  // Implant configuration:
  // head
  out << QString::number(ui->headShi->currentIndex()) << " ";
  out << QString::number(ui->headBri->currentIndex()) << " ";
  out << QString::number(ui->headFad->currentIndex()) << "\n";
  // eye
  out << QString::number(ui->eyeShi->currentIndex()) << " ";
  out << QString::number(ui->eyeBri->currentIndex()) << " ";
  out << QString::number(ui->eyeFad->currentIndex()) << "\n";
  // ear
  out << QString::number(ui->earShi->currentIndex()) << " ";
  out << QString::number(ui->earBri->currentIndex()) << " ";
  out << QString::number(ui->earFad->currentIndex()) << "\n";
  // chest
  out << QString::number(ui->chestShi->currentIndex()) << " ";
  out << QString::number(ui->chestBri->currentIndex()) << " ";
  out << QString::number(ui->chestFad->currentIndex()) << "\n";
  // rarm
  out << QString::number(ui->rarmShi->currentIndex()) << " ";
  out << QString::number(ui->rarmBri->currentIndex()) << " ";
  out << QString::number(ui->rarmFad->currentIndex()) << "\n";
  // larm
  out << QString::number(ui->larmShi->currentIndex()) << " ";
  out << QString::number(ui->larmBri->currentIndex()) << " ";
  out << QString::number(ui->larmFad->currentIndex()) << "\n";
  // waist
  out << QString::number(ui->waistShi->currentIndex()) << " ";
  out << QString::number(ui->waistBri->currentIndex()) << " ";
  out << QString::number(ui->waistFad->currentIndex()) << "\n";
  // rwrist
  out << QString::number(ui->rwristShi->currentIndex()) << " ";
  out << QString::number(ui->rwristBri->currentIndex()) << " ";
  out << QString::number(ui->rwristFad->currentIndex()) << "\n";
  // lwrist
  out << QString::number(ui->lwristShi->currentIndex()) << " ";
  out << QString::number(ui->lwristBri->currentIndex()) << " ";
  out << QString::number(ui->lwristFad->currentIndex()) << "\n";
  // leg
  out << QString::number(ui->legShi->currentIndex()) << " ";
  out << QString::number(ui->legBri->currentIndex()) << " ";
  out << QString::number(ui->legFad->currentIndex()) << "\n";
  // rhand
  out << QString::number(ui->rhandShi->currentIndex()) << " ";
  out << QString::number(ui->rhandBri->currentIndex()) << " ";
  out << QString::number(ui->rhandFad->currentIndex()) << "\n";
  // lhand
  out << QString::number(ui->lhandShi->currentIndex()) << " ";
  out << QString::number(ui->lhandBri->currentIndex()) << " ";
  out << QString::number(ui->lhandFad->currentIndex()) << "\n";
  // feet
  out << QString::number(ui->feetShi->currentIndex()) << " ";
  out << QString::number(ui->feetBri->currentIndex()) << " ";
  out << QString::number(ui->feetFad->currentIndex()) << "\n";

  // Buffs:
  out << (ui->SCE->isChecked() ? "1" : "0") << " ";
  out << (ui->SFA->isChecked() ? "1" : "0") << " ";
  out << (ui->CM->isChecked() ? "1" : "0") << " ";
  out << (ui->CA->isChecked() ? "1" : "0") << " ";
  out << (ui->ES->isChecked() ? "1" : "0") << " ";
  out << (ui->FG->isChecked() ? "1" : "0") << " ";
  out << (ui->EOB->isChecked() ? "1" : "0") << " ";
  out << (ui->ICRT->isChecked() ? "1" : "0") << " ";
  out << (ui->G->isChecked() ? "1" : "0") << " ";
  out << (ui->EB->isChecked() ? "1" : "0") << " ";
  out << (ui->IC->isChecked() ? "1" : "0") << " ";
  out << (ui->PS->isChecked() ? "1" : "0") << " ";
  out << (ui->NS->isChecked() ? "1" : "0") << " ";
  out << (ui->OME->isChecked() ? "1" : "0") << " ";
  out << "\n";

  // Abilities and Treatment:
  out << QString::number(ui->Strength->value()) << " ";
  out << QString::number(ui->Agility->value()) << " ";
  out << "\n";
  out << QString::number(ui->Stamina->value()) << " ";
  out << QString::number(ui->Intelligence->value()) << " ";
  out << "\n";
  out << QString::number(ui->Sense->value()) << " ";
  out << QString::number(ui->Psychic->value()) << " ";
  out << "\n";
  out << QString::number(ui->Treatment->value()) << " ";
  out << "\n";
}

void MainWindow::LoadBuildTab(QTextStream& in)
{
  int i;
  // head
  in >> i; ui->headShi->setCurrentIndex(i);
  in >> i; ui->headBri->setCurrentIndex(i);
  in >> i; ui->headFad->setCurrentIndex(i);
  // eye
  in >> i; ui->eyeShi->setCurrentIndex(i);
  in >> i; ui->eyeBri->setCurrentIndex(i);
  in >> i; ui->eyeFad->setCurrentIndex(i);
  // ear
  in >> i; ui->earShi->setCurrentIndex(i);
  in >> i; ui->earBri->setCurrentIndex(i);
  in >> i; ui->earFad->setCurrentIndex(i);
  // chest
  in >> i; ui->chestShi->setCurrentIndex(i);
  in >> i; ui->chestBri->setCurrentIndex(i);
  in >> i; ui->chestFad->setCurrentIndex(i);
  // rarm
  in >> i; ui->rarmShi->setCurrentIndex(i);
  in >> i; ui->rarmBri->setCurrentIndex(i);
  in >> i; ui->rarmFad->setCurrentIndex(i);
  // larm
  in >> i; ui->larmShi->setCurrentIndex(i);
  in >> i; ui->larmBri->setCurrentIndex(i);
  in >> i; ui->larmFad->setCurrentIndex(i);
  // waist
  in >> i; ui->waistShi->setCurrentIndex(i);
  in >> i; ui->waistBri->setCurrentIndex(i);
  in >> i; ui->waistFad->setCurrentIndex(i);
  // rwrist
  in >> i; ui->rwristShi->setCurrentIndex(i);
  in >> i; ui->rwristBri->setCurrentIndex(i);
  in >> i; ui->rwristFad->setCurrentIndex(i);
  // lwrist
  in >> i; ui->lwristShi->setCurrentIndex(i);
  in >> i; ui->lwristBri->setCurrentIndex(i);
  in >> i; ui->lwristFad->setCurrentIndex(i);
  // leg
  in >> i; ui->legShi->setCurrentIndex(i);
  in >> i; ui->legBri->setCurrentIndex(i);
  in >> i; ui->legFad->setCurrentIndex(i);
  // rhand
  in >> i; ui->rhandShi->setCurrentIndex(i);
  in >> i; ui->rhandBri->setCurrentIndex(i);
  in >> i; ui->rhandFad->setCurrentIndex(i);
  // lhand
  in >> i; ui->lhandShi->setCurrentIndex(i);
  in >> i; ui->lhandBri->setCurrentIndex(i);
  in >> i; ui->lhandFad->setCurrentIndex(i);
  // feet
  in >> i; ui->feetShi->setCurrentIndex(i);
  in >> i; ui->feetBri->setCurrentIndex(i);
  in >> i; ui->feetFad->setCurrentIndex(i);

  // Buffs:
  in >> i; ui->SCE->setChecked(i);
  in >> i; ui->SFA->setChecked(i);
  in >> i; ui->CM->setChecked(i);
  in >> i; ui->CA->setChecked(i);
  in >> i; ui->ES->setChecked(i);
  in >> i; ui->FG->setChecked(i);
  in >> i; ui->EOB->setChecked(i);
  in >> i; ui->ICRT->setChecked(i);
  in >> i; ui->G->setChecked(i);
  in >> i; ui->EB->setChecked(i);
  in >> i; ui->IC->setChecked(i);
  in >> i; ui->PS->setChecked(i);
  in >> i; ui->NS->setChecked(i);
  in >> i; ui->OME->setChecked(i);

  // Abilities and Treatment:
  in >> i; ui->Strength->setValue(i);
  in >> i; ui->Agility->setValue(i);
  in >> i; ui->Stamina->setValue(i);
  in >> i; ui->Intelligence->setValue(i);
  in >> i; ui->Sense->setValue(i);
  in >> i; ui->Psychic->setValue(i);
  double d;
  in >> d; ui->Treatment->setValue(d);

}

void MainWindow::SaveResultsTab(QTextStream& out)
{
  out << QString::number(ui->avgQLSpinBox->value()) << "\n";

  out << "Step One:\n";
  for(int i = 0; i != ui->stepOne->count(); ++i)
    out << ui->stepOne->item(i)->text() << "\n";
  out << "Step Two:\n";
  for(int i = 0; i != ui->stepTwo->count(); ++i)
    out << ui->stepTwo->item(i)->text() << "\n";;

}

void MainWindow::LoadResultsTab(QTextStream& in)
{
  double d;
  in >> d;
  ui->avgQLSpinBox->setValue(d);

  // Clear current results tab
  ui->stepOne->clear();
  ui->stepTwo->clear();

  in.readLine(); // Remove trailing newline.
  QString line = in.readLine(); // Line should now be "Step One:"
  line = in.readLine();
  bool on_step_one = true;
  while(line != "Shining:"){
    if(line == "Step Two:"){
      on_step_one = false;
      line = in.readLine();
      continue;
    }
    if(on_step_one)
      ui->stepOne->addItem(line);
    else
      ui->stepTwo->addItem(line);
    line  = in.readLine();
  }
}

void MainWindow::SaveShoppingTab(QTextStream& out)
{
  out << "Shining:\n";
  for(int i = 0; i != ui->Shining->count(); ++i)
    out << ui->Shining->item(i)->text() << "\n";
  out << "Bright:\n";
  for(int i = 0; i != ui->Bright->count(); ++i)
    out << ui->Bright->item(i)->text() << "\n";;
  // Avoid trailing newline
  out << "Faded:";
  for(int i = 0; i != ui->Faded->count(); ++i)
    out << "\n" << ui->Faded->item(i)->text();
}

void MainWindow::LoadShoppingTab(QTextStream& in)
{
  // Clear current shopping tab
  ui->Shining->clear();
  ui->Bright->clear();
  ui->Faded->clear();

  QString line = in.readLine();
  while(line != "Bright:"){
    ui->Shining->addItem(line);
    line = in.readLine();
  }
  line = in.readLine();
  while(line != "Faded:"){
    ui->Bright->addItem(line);
    line = in.readLine();
  }
  while(!in.atEnd()){
    line = in.readLine();
    ui->Faded->addItem(line);
  }
}
