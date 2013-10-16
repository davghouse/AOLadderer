#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "ladder_helper.h"
#include "QString"
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
  ui(new Ui::MainWindow)
{
  ui->setupUi(this);
  config_not_empty_ = false;
  // Database
  standard_implants_ = QSqlDatabase::addDatabase("QSQLITE");
  standard_implants_.setDatabaseName("standard_implants.db");
  standard_implants_.open();
  // Ladder implants
  std::ifstream in("ladder_implants.txt");
  string ladderSlot, abi, shi, bri, fad;
  while(in >> abi){
    if(abi == "$"){
      in >> ladderSlot;
      ladder_slots_.push_back(LadderSlot(ladderSlot));
    }
    else if(abi == "#")
      ladder_slots_[ladder_slots_.size() - 1].AddLadderImplantSubset();
    else{
      in >> shi >> bri >> fad;
      // add a ladder imp to the most recent slot
      ladder_slots_[ladder_slots_.size() - 1].AddLadderImplant(Implant(0, ladderSlot, abi, shi, bri, fad, true));
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

  // Shopping list
  connect(ui->Shining, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtShining(QModelIndex)));
  connect(ui->Bright, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtBright(QModelIndex)));
  connect(ui->Faded, SIGNAL(clicked(QModelIndex)), SLOT(ToggleBoughtFaded(QModelIndex)));
}

MainWindow::~MainWindow()
{
  delete ui;
  standard_implants_.close();
}

// member functions:

// some better way to do this... use table widget?
void MainWindow::GetConfiguration(ImplantConfiguration& requiredConfig)
{
  std::string shi, bri, fad;
  // head 0 Head
  shi = ui->headShi->currentText().toStdString();
  bri = ui->headBri->currentText().toStdString();
  fad = ui->headFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 0,"Head", shi, bri, fad);
  }
  // eye 1 Eye
  shi = ui->eyeShi->currentText().toStdString();
  bri = ui->eyeBri->currentText().toStdString();
  fad = ui->eyeFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 1,"Eye", shi, bri, fad);
  }
  // ear 2 Ear
  shi = ui->earShi->currentText().toStdString();
  bri = ui->earBri->currentText().toStdString();
  fad = ui->earFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig,2 ,"Ear", shi, bri, fad);
  }
  // chest 3 Chest
  shi = ui->chestShi->currentText().toStdString();
  bri = ui->chestBri->currentText().toStdString();
  fad = ui->chestFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 3,"Chest", shi, bri, fad);
  }
  // rarm 4 Right-Arm
  shi = ui->rarmShi->currentText().toStdString();
  bri = ui->rarmBri->currentText().toStdString();
  fad = ui->rarmFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 4,"Right-Arm", shi, bri, fad);
  }
  // larm 5 Left-Arm
  shi = ui->larmShi->currentText().toStdString();
  bri = ui->larmBri->currentText().toStdString();
  fad = ui->larmFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 5,"Left-Arm", shi, bri, fad);
  }
  // waist 6 Waist
  shi = ui->waistShi->currentText().toStdString();
  bri = ui->waistBri->currentText().toStdString();
  fad = ui->waistFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 6,"Waist", shi, bri, fad);
  }
  // rwrist 7 Right-Wrist
  shi = ui->rwristShi->currentText().toStdString();
  bri = ui->rwristBri->currentText().toStdString();
  fad = ui->rwristFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 7,"Right-Wrist", shi, bri, fad);
  }
  // lwrist 8 Left-Wrist
  shi = ui->lwristShi->currentText().toStdString();
  bri = ui->lwristBri->currentText().toStdString();
  fad = ui->lwristFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 8,"Left-Wrist", shi, bri, fad);
  }
  // leg 9 Leg
  shi = ui->legShi->currentText().toStdString();
  bri = ui->legBri->currentText().toStdString();
  fad = ui->legFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 9,"Leg", shi, bri, fad);
  }
  // rhand 10 Right-Hand
  shi = ui->rhandShi->currentText().toStdString();
  bri = ui->rhandBri->currentText().toStdString();
  fad = ui->rhandFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 10,"Right-Hand", shi, bri, fad);
  }
  // lhand 11 Left-Hand
  shi = ui->lhandShi->currentText().toStdString();
  bri = ui->lhandBri->currentText().toStdString();
  fad = ui->lhandFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 11,"Left-Hand", shi, bri, fad);
  }
  // feet 12 Feet
  shi = ui->feetShi->currentText().toStdString();
  bri = ui->feetBri->currentText().toStdString();
  fad = ui->feetFad->currentText().toStdString();
  if(shi == "") shi = "Empty"; if(bri == "") bri = "Empty"; if(fad == "") fad = "Empty";
  if(shi != "Empty" || bri != "Empty" || fad != "Empty"){
    GetConfigurationHelper(requiredConfig, 12,"Feet", shi, bri, fad);
  }
}

void MainWindow::GetConfigurationHelper(ImplantConfiguration& requiredConfig, int i, const string& slot, string& shi, string& bri, string& fad)
{
  config_not_empty_ = true;
  requiredConfig.config_[i].shining_full_ = shi;
  requiredConfig.config_[i].bright_full_ = bri;
  requiredConfig.config_[i].faded_full_ = fad;
  std::string qBegin = "SELECT req FROM implants WHERE slot='" + slot + "'";
  string qText = qBegin + " AND Shining='"+shi+"' AND Bright='"+bri+"' AND Faded='"+fad+"'";
  QSqlQuery q;
  q.exec(QString::fromStdString(qText));
  q.next();
  requiredConfig.config_[i].ability_name_ = AbilityFullToAbbr(q.value(0).toString().toStdString());
  requiredConfig.config_[i].ability_int_ = AbilityToInt(requiredConfig.config_[i].ability_name_);
  bool usedToLadder = false;
  if(AbilityToIntAndMakeFull(shi) >= 0){
    requiredConfig.config_[i].shining_abbr_ = shi;
    requiredConfig.config_[i].shining_int_ = ClusterToInt(requiredConfig.config_[i].shining_abbr_);
    usedToLadder = true;
  }
  if(AbilityToIntAndMakeFull(bri) >= 0){
    requiredConfig.config_[i].bright_abbr_ = bri;
    requiredConfig.config_[i].bright_int_ = ClusterToInt(requiredConfig.config_[i].bright_abbr_);
    usedToLadder = true;
  }
  if(AbilityToIntAndMakeFull(fad) >= 0){
    requiredConfig.config_[i].faded_abbr_ = fad;
    requiredConfig.config_[i].faded_int_ = ClusterToInt(requiredConfig.config_[i].faded_abbr_);
    usedToLadder = true;
  }
  requiredConfig.config_[i].used_to_ladder_ = usedToLadder;
}

void MainWindow::GetStats(Stats& baseStats)
{
  vector<int> abisHolder;
  abisHolder.resize(6);
  abisHolder[0] = ui->Strength->value();
  abisHolder[1] = ui->Agility->value();
  abisHolder[2] = ui->Stamina->value();
  abisHolder[3] = ui->Intelligence->value();
  abisHolder[4] = ui->Sense->value();
  abisHolder[5] = ui->Psychic->value();
  baseStats.UpdateStats(abisHolder, ui->Treatment->value());
}

// slots:
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
  Stats baseStats;
  GetConfiguration(requiredConfig);
  GetStats(baseStats);
  // height one
  if(config_not_empty_){
    Ladder ladder(requiredConfig,baseStats);
    ladder.HeightOne(ladder_slots_);
    ShowHeightOne(ladder);
    ui->tabWidget->setCurrentWidget(ui->resultsTab);
  }
}


void MainWindow::ShowHeightOne(const Ladder & ladder)
{

  // for shopping list
  vector<ShoppingItem> shining, bright, faded;
  // Step One
  bool doneWithLaddering = false;
  int firstAfterDash = 0;
  for(vector<int>::const_iterator it = ladder.process_[0].order_.begin();
      it != ladder.process_[0].order_.end(); ++it){
    Implant imp = ladder.process_[0].config_[*it];
    if(imp.ability_name() != "abi" && imp.ql() > 0){
      // ladder implant
      if(imp.lock_){
        std::string qText = "SELECT shining, bright, faded FROM implants ";
        qText += "WHERE slot='" + SlotAbbrToFull(imp.slot_name()) + "' ";
        qText += "and req='" + AbilityAndTreatmentAbbrToFull(imp.ability_name()) + "' ";
        if(imp.shining_abbr() != "shi"){
          qText += "and Shining='" + AbilityAndTreatmentAbbrToFull(imp.shining_abbr()) + "' ";
        }
        if(imp.bright_abbr() != "bri"){
          qText += "and Bright='" + AbilityAndTreatmentAbbrToFull(imp.bright_abbr()) + "' ";
        }
        if(imp.faded_abbr() != "fad"){
          qText += "and Faded='" + AbilityAndTreatmentAbbrToFull(imp.faded_abbr()) + "' ";
        }
        QSqlQuery q;
        q.exec(QString::fromStdString(qText));
        int mostEmpties = -1;
        std::string shi, bri, fad;
        while(q.next()){
          int emptyCount = 0;
          string s = q.value(0).toString().toStdString();
          string b = q.value(1).toString().toStdString();
          string f = q.value(2).toString().toStdString();
          if(s == "Empty") ++emptyCount;
          if(b == "Empty") ++emptyCount;
          if(f == "Empty") ++emptyCount;
          if(emptyCount > mostEmpties || (emptyCount == mostEmpties && s == "Empty")){
            mostEmpties = emptyCount;
            shi = s;
            bri = b;
            fad = f;
          }
        }
        ShowImplant(imp, shi, bri, fad, 1);
        if(shi != "-----"){
          ShoppingItem t;
          t.cluster_ = shi; t.ql_ = imp.ql();
          shining.push_back(t);
        }
        if(bri != "-----"){
          ShoppingItem t;
          t.cluster_ = bri; t.ql_ = imp.ql();
          bright.push_back(t);
        }
        if(fad != "-----"){
          ShoppingItem t;
          t.cluster_ = fad; t.ql_ = imp.ql();
          faded.push_back(t);
        }
      }
      // required implant
      else{
        if(!doneWithLaddering){
          doneWithLaddering = true;
          ui->stepOne->addItem(QString::fromStdString(std::string(77, '-')));
          firstAfterDash = *it;
        }
        std::string shi, bri, fad;
        shi = ladder.required_config_[*it].shining_full_;
        bri = ladder.required_config_[*it].bright_full_;
        fad = ladder.required_config_[*it].faded_full_;
        ShowImplant(imp, shi, bri, fad, 1);
      }
    }
    ui->avgQLSpinBox->setValue(ladder.AverageQL());
  }
  if(!doneWithLaddering){
    ui->stepOne->addItem(QString::fromStdString(std::string(77, '-')));
  }
  // Step Two
  for(vector<int>::const_iterator it = ladder.process_[1].order_.begin();
      it != ladder.process_[1].order_.end(); ++it){
    Implant imp = ladder.process_[1].config_[*it];
    if(imp.ability_name() != "abi" && imp.ql() > 0 && imp.remove()){
      if(doneWithLaddering){
        if(*it == firstAfterDash){
          ui->stepTwo->addItem(QString::fromStdString(std::string(77, '-')));
        }
      }
      std::string shi, bri, fad;
      shi = ladder.required_config_[*it].shining_full_;
      bri = ladder.required_config_[*it].bright_full_;
      fad = ladder.required_config_[*it].faded_full_;
      ShowImplant(imp, shi, bri, fad, 2);
      if(shi != "-----"){
        ShoppingItem t;
        t.cluster_ = shi; t.ql_ = imp.ql();
        shining.push_back(t);
      }
      if(bri != "-----"){
        ShoppingItem t;
        t.cluster_ = bri; t.ql_ = imp.ql();
        bright.push_back(t);
      }
      if(fad != "-----"){
        ShoppingItem t;
        t.cluster_ = fad; t.ql_ = imp.ql();
        faded.push_back(t);
      }
    }
  }
  if(!doneWithLaddering)
    ui->stepTwo->addItem(QString::fromStdString(std::string(77, '-')));
  std::sort(shining.begin(), shining.end());
  std::sort(bright.begin(), bright.end());
  std::sort(faded.begin(), faded.end());
  for(vector<ShoppingItem>::iterator it = shining.begin(); it != shining.end(); ++it){
    int clusterQL = .86*(it->ql_) + .01;
    std::stringstream t;
    t << "QL " << clusterQL << " " << it->cluster_;
    ui->Shining->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = bright.begin(); it != bright.end(); ++it){
    int clusterQL = .84*(it->ql_) + .01;
    std::stringstream t;
    t << "QL " << clusterQL << " " << it->cluster_;
    ui->Bright->addItem(QString::fromStdString(t.str()));
  }
  for(vector<ShoppingItem>::iterator it = faded.begin(); it != faded.end(); ++it){
    int clusterQL = .82*(it->ql_) + .01;
    std::stringstream t;
    t << "QL " << clusterQL << " " << it->cluster_;
    ui->Faded->addItem(QString::fromStdString(t.str()));
  }
}

void MainWindow::ShowImplant(const Implant& imp, std::string& shi, std::string& bri, std::string& fad, int step)
{
  if(shi == "Empty") shi = "-----";
  if(bri == "Empty") bri = "-----";
  if(fad == "Empty") fad = "-----";
  string temp;
  temp += "[" + imp.ability_name() + "] ";
  std::stringstream ss;
  ss << imp.ql();
  temp += "QL " + ss.str() + " " + SlotAbbrToFull(imp.slot_name()) + ":";
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

