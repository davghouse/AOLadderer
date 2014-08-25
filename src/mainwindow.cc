#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "ladder_helper.h"
#include "QFile"
#include "QMessageBox"
#include "QFileDialog"
#include "QDesktopServices"
#include "QUrl"
#include <algorithm>
#include <iostream>
#include <sstream>

using std::vector;
using std::string;
using std::cerr; using std::endl;
using std::find_if;
using namespace ladder_helper;


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
      ladder_slots_.back().AddLadderImplantSubset();
    else{
      in >> shi >> bri >> fad;
      // Add a ladder implant to the most recent slot
      ladder_slots_.back().AddLadderImplant(Implant(0, ladder_slot, ability, shi, bri, fad));
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
  connect(ui->Explosifs, SIGNAL(clicked(bool)), this, SLOT(ToggleExplosifs(bool)));

  connect(ui->IC, SIGNAL(clicked(bool)), this, SLOT(ToggleIronCircle(bool)));
  connect(ui->PS, SIGNAL(clicked(bool)), this, SLOT(ToggleProdigiousStrength(bool)));
  connect(ui->ICPS, SIGNAL(clicked(bool)), this, SLOT(ToggleICPS(bool)));

  connect(ui->NS, SIGNAL(clicked(bool)), this, SLOT(ToggleNeuronalStimulator(bool)));

  // Build:
  connect(ui->Build, SIGNAL(clicked()), SLOT(RunHeightOne()));
  connect(ui->Build_2, SIGNAL(clicked()), SLOT(RunHeightTwo()));

  // Shopping list:
  connect(ui->Shining, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtShining(QModelIndex)));
  connect(ui->Bright, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtBright(QModelIndex)));
  connect(ui->Faded, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtFaded(QModelIndex)));

  connect(ui->Shining_2, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtShining2(QModelIndex)));
  connect(ui->Bright_2, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtBright2(QModelIndex)));
  connect(ui->Faded_2, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtFaded2(QModelIndex)));

  // File menu:
  connect(ui->actionOpen, SIGNAL(triggered()), SLOT(Open()));
  connect(ui->actionSave, SIGNAL(triggered()), SLOT(Save()));
  connect(ui->actionSave_as, SIGNAL(triggered()), SLOT(SaveAs()));
  connect(ui->actionExport_to_Auno, SIGNAL(triggered()), SLOT(ExportToAuno()));
  connect(ui->actionExport_to_Auno_2, SIGNAL(triggered()), SLOT(ExportToAuno2()));
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
  std::string query_begin = "SELECT req, aoid FROM implants WHERE slot='" + slot + "'";
  string query_text = query_begin + " AND Shining='"+shi+"' AND Bright='"+bri+"' AND Faded='"+fad+"'";
  QSqlQuery query;
  query.exec(QString::fromStdString(query_text));
  query.next();
  required_config.config_[i].ability_name_ = AbilityFullToAbbr(query.value(0).toString().toStdString());
  required_config.config_[i].ability_int_ = AbilityToInt(required_config.config_[i].ability_name_);
  required_config.config_[i].aoid_ = query.value(1).toInt();
  bool used_to_ladder = false;
  if(AbilityToIntAndAbbreviate(shi) >= 0){
    required_config.config_[i].shining_abbr_ = shi;
    required_config.config_[i].shining_int_ = ClusterToInt(required_config.config_[i].shining_abbr_);
    used_to_ladder = true;
  }
  if(AbilityToIntAndAbbreviate(bri) >= 0){
    required_config.config_[i].bright_abbr_ = bri;
    required_config.config_[i].bright_int_ = ClusterToInt(required_config.config_[i].bright_abbr_);
    used_to_ladder = true;
  }
  if(AbilityToIntAndAbbreviate(fad) >= 0){
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

void MainWindow::ShowHeightTwo(const Ladder & ladder)
{
  // For shopping list
  vector<ShoppingItem> shining_shopping, bright_shopping, faded_shopping;
  for(int i = 0; i <= 1; ++i){
    for(vector<int>::const_iterator it = ladder.process_[i].order_.begin();
        it != ladder.process_[i].order_.end(); ++it){
      const Implant& implant = ladder.process_[i].config_[*it];
      if(implant.ability_name() != "abi"){
        if(i == 1 && ladder.process_[1].config_[*it] == ladder.process_[0].config_[*it])
        {
          continue;
        }
        ShowImplant2(implant, i + 1);
        std::string shi = implant.shining_full_;
        std::string bri = implant.bright_full_;
        std::string fad = implant.faded_full_;
        if(shi != "Empty"){
          ShoppingItem t;
          t.cluster_ = shi; t.ql_ = implant.ql();
          shining_shopping.push_back(t);
        }
        if(bri != "Empty"){
          ShoppingItem t;
          t.cluster_ = bri; t.ql_ = implant.ql();
          bright_shopping.push_back(t);
        }
        if(fad != "Empty"){
          ShoppingItem t;
          t.cluster_ = fad; t.ql_ = implant.ql();
          faded_shopping.push_back(t);
        }
      }
    }
  }
  for(unsigned int i = 0; i < ladder.process_[2].config_.size(); ++i){
    const Implant& implant = ladder.process_[2].config_[i];
    ShowImplant2(implant, 3);
    std::string shi = implant.shining_full_;
    std::string bri = implant.bright_full_;
    std::string fad = implant.faded_full_;
    if(shi != "Empty"){
      ShoppingItem t;
      t.cluster_ = shi; t.ql_ = implant.ql();
      shining_shopping.push_back(t);
    }
    if(bri != "Empty"){
      ShoppingItem t;
      t.cluster_ = bri; t.ql_ = implant.ql();
      bright_shopping.push_back(t);
    }
    if(fad != "Empty"){
      ShoppingItem t;
      t.cluster_ = fad; t.ql_ = implant.ql();
      faded_shopping.push_back(t);
    }
  }
  ui->avgQLSpinBox_2->setValue(ladder.AverageQL());
  // Shopping tab:
  std::sort(shining_shopping.begin(), shining_shopping.end());
  std::sort(bright_shopping.begin(), bright_shopping.end());
  std::sort(faded_shopping.begin(), faded_shopping.end());
  for(vector<ShoppingItem>::iterator it = shining_shopping.begin(); it != shining_shopping.end(); ++it){
    int cluster_ql = .86*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Shining_2->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = bright_shopping.begin(); it != bright_shopping.end(); ++it){
    int cluster_ql = .84*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Bright_2->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = faded_shopping.begin(); it != faded_shopping.end(); ++it){
    int cluster_ql = .82*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Faded_2->addItem(QString::fromStdString(t.str()));
  }
}

void MainWindow::ShowHeightOne(const Ladder & ladder)
{
  // For shopping list
  vector<ShoppingItem> shining_shopping, bright_shopping, faded_shopping;
  // Results tab:
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
  // Step Two:
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
  // Shopping tab:
  std::sort(shining_shopping.begin(), shining_shopping.end());
  std::sort(bright_shopping.begin(), bright_shopping.end());
  std::sort(faded_shopping.begin(), faded_shopping.end());
  for(vector<ShoppingItem>::iterator it = shining_shopping.begin(); it != shining_shopping.end(); ++it){
    int cluster_ql = .86*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Shining->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = bright_shopping.begin(); it != bright_shopping.end(); ++it){
    int cluster_ql = .84*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Bright->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = faded_shopping.begin(); it != faded_shopping.end(); ++it){
    int cluster_ql = .82*(it->ql_) + .01;
    std::stringstream t;
    t << cluster_ql << " " << it->cluster_;
    ui->Faded->addItem(QString::fromStdString(t.str()));
  }
}

void MainWindow::ShowImplant2(const Implant& implant, int step)
{
  std::string shi, bri, fad;
  shi = implant.shining_full_;
  bri = implant.bright_full_;
  fad = implant.faded_full_;
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
    ui->stepOne_2->addItem(QString::fromStdString(temp));
  }
  if(step == 2){
    ui->stepTwo_2->addItem(QString::fromStdString(temp));
  }
  if(step == 3){
    ui->stepThree_2->addItem(QString::fromStdString(temp));
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

void MainWindow::RunHeightTwo()
{
  ui->stepOne_2->clear();
  ui->stepTwo_2->clear();
  ui->stepThree_2->clear();
  ui->avgQLSpinBox_2->setValue(0);
  ui->Shining_2->clear();
  ui->Bright_2->clear();
  ui->Faded_2->clear();
  config_not_empty_ = false;
  ImplantConfiguration required_config;
  CharacterStats base_stats;
  GetConfiguration(required_config);
  GetStats(base_stats);
  if(config_not_empty_){
    Ladder ladder(required_config, base_stats);
#ifndef QT_NO_CURSOR
    QApplication::setOverrideCursor(Qt::WaitCursor);
#endif
    ladder.HeightTwo(ladder_slots_);
    if(ladder.process_.size() == 3){
      ShowHeightTwo(ladder);
    }
#ifndef QT_NO_CURSOR
    QApplication::restoreOverrideCursor();
#endif
    if(ladder.process_.size() == 3){
      final_config_2 = ladder.process_[2];
      CreateAunoLink2(ladder.process_[2].config_);
    }
    ui->tabWidget->setCurrentWidget(ui->resultsTab2);
  }
}

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
  ImplantConfiguration required_config;
  CharacterStats base_stats;
  GetConfiguration(required_config);
  GetStats(base_stats);
  if(config_not_empty_){
    Ladder ladder(required_config, base_stats);
#ifndef QT_NO_CURSOR
    QApplication::setOverrideCursor(Qt::WaitCursor);
#endif
    ladder.HeightOne(ladder_slots_);
    ShowHeightOne(ladder);
#ifndef QT_NO_CURSOR
    QApplication::restoreOverrideCursor();
#endif
    if(ladder.process_.size() == 2)
      final_config_ = ladder.process_[1];
    CreateAunoLink();
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

void MainWindow::ToggleExplosifs(bool add)
{
  int val = add ? 20 : -20;
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
  if(add && ui->ICPS->isChecked())
  {
    ui->ICPS->setChecked(false);
    ToggleICPS(false);
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
  if(add && ui->ICPS->isChecked()){
    ui->ICPS->setChecked(false);
    ToggleICPS(false);
  }
}

void MainWindow::ToggleICPS(bool add)
{
  int val_str = add ? 40 : -40;
  int val_sta = add ? 20 : -20;
  ui->Strength->setValue(ui->Strength->value() + val_str);
  ui->Stamina->setValue(ui->Stamina->value() + val_sta);
  if(add && ui->IC->isChecked()){
    ui->IC->setChecked(false);
    ToggleIronCircle(false);
  }
  if(add && ui->PS->isChecked()){
    ui->PS->setChecked(false);
    ToggleProdigiousStrength(false);
  }
}

void MainWindow::ToggleNeuronalStimulator(bool add)
{
  int val = add ? 20 : -20;
  double trickle = (.5*val)/4;
  ui->Intelligence->setValue(ui->Intelligence->value() + val);
  ui->Psychic->setValue(ui->Psychic->value() + val);
  ui->Treatment->setValue(ui->Treatment->value() + trickle);
}

// Shopping list:
void MainWindow::ToggleBoughtShining(QModelIndex i)
{
  QString temp = ui->Shining->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Shining->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Shining->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtBright(QModelIndex i)
{
  QString temp = ui->Bright->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Bright->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Bright->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtFaded(QModelIndex i)
{
  QString temp = ui->Faded->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Faded->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Faded->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtShining2(QModelIndex i)
{
  QString temp = ui->Shining_2->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Shining_2->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Shining_2->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtBright2(QModelIndex i)
{
  QString temp = ui->Bright_2->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Bright_2->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Bright_2->item(i.row())->setText(temp);
  }
}

void MainWindow::ToggleBoughtFaded2(QModelIndex i)
{
  QString temp = ui->Faded_2->item(i.row())->text();
  if(temp[0] == '-'){
    temp = temp.right(temp.size() - 1);
    ui->Faded_2->item(i.row())->setText(temp);
  }
  else{
    temp.prepend('-');
    ui->Faded_2->item(i.row())->setText(temp);
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

void MainWindow::ExportToAuno2()
{
  if(!auno_link_2.isEmpty())
    QDesktopServices::openUrl(QUrl(auno_link_2));
}
void MainWindow::ExportToAuno()
{
  if(!auno_link_.isEmpty())
    QDesktopServices::openUrl(QUrl(auno_link_));
}

// File menu helper functions:
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
  LoadAunoLink(in);
  if(!in.atEnd()){
    LoadResultsTab2(in);
    LoadShoppingTab2(in);
    LoadAunoLink2(in);
  }
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
  SaveAunoLink(out);
  SaveResultsTab2(out);
  SaveShoppingTab2(out);
  SaveAunoLink2(out);
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
  // Backwards compatibility
  out << "2" << " ";
  out << (ui->Explosifs->isChecked() ? "1" : "0") << " ";
  out << (ui->ICPS->isChecked() ? "1" : "0") << " ";
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
  in >> i;
  // Backwards compatibility
  if(i == 2)
  {
    in >> i; ui->Explosifs->setChecked(i);
    in >> i; ui->ICPS->setChecked(i);
  }

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

void MainWindow::SaveResultsTab2(QTextStream& out)
{
  out << "\n" << QString::number(ui->avgQLSpinBox_2->value()) << "\n";

  out << "Step One 2:\n";
  for(int i = 0; i != ui->stepOne_2->count(); ++i)
    out << ui->stepOne_2->item(i)->text() << "\n";
  out << "Step Two 2:\n";
  for(int i = 0; i != ui->stepTwo_2->count(); ++i)
    out << ui->stepTwo_2->item(i)->text() << "\n";;
  out << "Step Three 2:\n";
  for(int i = 0; i != ui->stepThree_2->count(); ++i)
    out << ui->stepThree_2->item(i)->text() << "\n";;
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

void MainWindow::LoadResultsTab2(QTextStream& in)
{
    double d;
    in >> d;
    ui->avgQLSpinBox_2->setValue(d);

    // Clear current results tab
    ui->stepOne_2->clear();
    ui->stepTwo_2->clear();
    ui->stepThree_2->clear();

    in.readLine(); // Remove trailing newline.
    QString line = in.readLine(); // Line should now be "Step One:"
    line = in.readLine();
    bool on_step_one = true;
    bool on_step_two = false;
    bool on_step_three = false;
    while(line != "Shining 2:"){
      if(line == "Step Two 2:"){
        on_step_one = false;
        on_step_two = true;
        on_step_three = false;
        line = in.readLine();
        continue;
      }
      else if(line == "Step Three 2:"){
        on_step_one = false;
        on_step_two = false;
        on_step_three = true;
        line = in.readLine();
        continue;
      }
      if(on_step_one)
        ui->stepOne_2->addItem(line);
      else if(on_step_two)
        ui->stepTwo_2->addItem(line);
      else if(on_step_three)
        ui->stepThree_2->addItem(line);
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

void MainWindow::SaveShoppingTab2(QTextStream& out)
{
  out << "Shining 2:\n";
  for(int i = 0; i != ui->Shining_2->count(); ++i)
    out << ui->Shining_2->item(i)->text() << "\n";
  out << "Bright 2:\n";
  for(int i = 0; i != ui->Bright_2->count(); ++i)
    out << ui->Bright_2->item(i)->text() << "\n";;
  // Avoid trailing newline
  out << "Faded 2:";
  for(int i = 0; i != ui->Faded_2->count(); ++i)
    out << "\n" << ui->Faded_2->item(i)->text();
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
    if(line == "Auno link(s):" || line == "Auno link:")
      break;
    ui->Faded->addItem(line);
  }
}

void MainWindow::LoadShoppingTab2(QTextStream& in)
{
  // Clear current shopping tab
  ui->Shining_2->clear();
  ui->Bright_2->clear();
  ui->Faded_2->clear();

  QString line = in.readLine();
  while(line != "Bright 2:"){
    ui->Shining_2->addItem(line);
    line = in.readLine();
  }
  line = in.readLine();
  while(line != "Faded 2:"){
    ui->Bright_2->addItem(line);
    line = in.readLine();
  }
  while(!in.atEnd()){
    line = in.readLine();
    if(line == "Auno link(s):" || line == "Auno link:")
      break;
    ui->Faded_2->addItem(line);
  }
}

void MainWindow::SaveAunoLink2(QTextStream& out)
{
  if(auno_link_2.isEmpty())
    return;
  out << "\n" << "Auno link:";
  out << "\n" << auno_link_2;

}

void MainWindow::SaveAunoLink(QTextStream& out)
{
  if(auno_link_.isEmpty())
    return;
  out << "\n" << "Auno link:";
  out << "\n" << auno_link_;

}

void MainWindow::LoadAunoLink2(QTextStream& in)
{
  if(!in.atEnd())
     auno_link_2 = in.readLine();
}

void MainWindow::LoadAunoLink(QTextStream& in)
{
  if(!in.atEnd())
     auno_link_ = in.readLine();
}

void MainWindow::CreateAunoLink2(const vector<Implant>& implants)
{
  QString s = "http://auno.org/ao/equip.php?noedit=1";
  for(unsigned int i = 0; i < implants.size(); ++i){
    const Implant& implant = implants[i];
    if(implant.ability_name() != "abi" && implant.ql() > 0 && implant.remove()){
      s.append("&id3-");
      QString temp = ConvertSlotToAuno(implant.slot_name_);
      s.append(temp);
      s.append("=");
      s.append(QString::number(implant.aoid_));
      s.append("&ql3-");
      s.append(temp);
      s.append("=");
      s.append(QString::number(implant.ql_));
    }
  }
  auno_link_2 = s;
}

void MainWindow::CreateAunoLink()
{
  QString s = "http://auno.org/ao/equip.php?noedit=1";
  if(config_not_empty_){
    for(vector<int>::const_iterator it = final_config_.order_.begin();
        it != final_config_.order_.end(); ++it){
      Implant implant = final_config_.config_[*it];
      if(implant.ability_name() != "abi" && implant.ql() > 0 && implant.remove()){
        s.append("&id3-");
        QString temp = ConvertSlotToAuno(implant.slot_name_);
        s.append(temp);
        s.append("=");
        s.append(QString::number(implant.aoid_));
        s.append("&ql3-");
        s.append(temp);
        s.append("=");
        s.append(QString::number(implant.ql_));
      }
    }
  }
  auno_link_ = s;
}

// Auno uses powers of 2 to differentiate implant slots, from 2 to 8196.
QString MainWindow::ConvertSlotToAuno(const std::string& slot_name)
{
  std::string slot_name_full = SlotAbbrToFull(slot_name);
  if(slot_name_full == "Eye")
    return "2";
  if(slot_name_full == "Head")
    return "4";
  if(slot_name_full == "Ear")
    return "8";
  if(slot_name_full == "Right-Arm")
    return "16";
  if(slot_name_full == "Chest")
    return "32";
  if(slot_name_full == "Left-Arm")
    return "64";
  if(slot_name_full == "Right-Wrist")
    return "128";
  if(slot_name_full == "Waist")
    return "256";
  if(slot_name_full == "Left-Wrist")
    return "512";
  if(slot_name_full == "Right-Hand")
    return "1024";
  if(slot_name_full == "Leg")
    return "2048";
  if(slot_name_full == "Left-Hand")
     return "4096";
  if(slot_name_full == "Feet")
     return "8192";
  return "0";
}
