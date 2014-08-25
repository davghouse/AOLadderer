#ifndef MAINWINDOW_H_
#define MAINWINDOW_H_


#include "ladder.h"

struct AunoItem;

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
  Q_OBJECT

public:
  explicit MainWindow(QWidget *parent = 0);
  ~MainWindow();

private slots:
  // Buffs:
  void ToggleSurgeryClinicEffect(bool);
  void ToggleSuperiorFirstAid(bool);
  void ToggleCompositeMedical(bool);
  void ToggleCompositeAttribute(bool);
  void ToggleEnhancedSenses(bool);
  void ToggleFelineGrace(bool);
  void ToggleEssenceOfBehemoth(bool);
  void ToggleImprovedCutRedTape(bool);
  void ToggleGauntlet(bool);
  void ToggleExtruderBar(bool);
  void ToggleExplosifs(bool);

  void ToggleIronCircle(bool);
  void ToggleProdigiousStrength(bool);
  void ToggleICPS(bool);

  void ToggleNeuronalStimulator(bool);

  // Build:
  void RunHeightOne();
  void RunHeightTwo();

  // Shopping list:
  void ToggleBoughtShining(QModelIndex);
  void ToggleBoughtBright(QModelIndex);
  void ToggleBoughtFaded(QModelIndex);
  void ToggleBoughtShining2(QModelIndex);
  void ToggleBoughtBright2(QModelIndex);
  void ToggleBoughtFaded2(QModelIndex);

  // File menu:
  void Open();
  void Save();
  void SaveAs();
  void ExportToAuno();
  void ExportToAuno2();

private:
  // File menu helper functions:
  void SaveFile(QString&);
  void LoadFile(QString&);
  void SaveBuildTab(QTextStream&);
  void LoadBuildTab(QTextStream&);
  void SaveResultsTab(QTextStream&);
  void SaveResultsTab2(QTextStream&);
  void LoadResultsTab(QTextStream&);
  void LoadResultsTab2(QTextStream&);
  void SaveShoppingTab(QTextStream&);
  void SaveShoppingTab2(QTextStream&);
  void LoadShoppingTab(QTextStream&);
  void LoadShoppingTab2(QTextStream&);
  void SaveAunoLink(QTextStream&);
  void SaveAunoLink2(QTextStream&);
  void LoadAunoLink(QTextStream&);
  void LoadAunoLink2(QTextStream&);

  // Creates Auno.org link to most recently built/loaded configuration.
  void CreateAunoLink();
  void CreateAunoLink2(const std::vector<Implant>& implants);
  static QString ConvertSlotToAuno(const std::string&);

private:
  // Gets the configuration selected and converts it into a form usable by
  // the HeightOne algorithm. This conversion is a carry-over from when the
  // programming was command-line based and implant configurations were supplied
  // in a minimal form, only specifying ability requirements and clusters which
  // were important for laddering.
  // The old form was, for example:
  // head int int bri fad
  // eye  agi shi tre fad
  //  .
  //  .
  //  .
  // feet agi shi agi fad
  void GetConfiguration(ImplantConfiguration& required_config);
  // Performs the mentioned conversion.
  void GetConfigurationHelper(ImplantConfiguration& required_config,
                              int i, const std::string& slot, std::string& shi,
                              std::string& bri, std::string& fad);
  void GetStats(CharacterStats& base_stats);
  // Fills in the other two tabs of the GUI, taking special care to
  // print out the dashed separator line.
  void ShowHeightOne(const Ladder& ladder);
  void ShowHeightTwo(const Ladder& ladder);
  void ShowImplant(const Implant& implant, std::string& shi,
                   std::string& bri, std::string& fad, int step);
  void ShowImplant2(const Implant& implant, int step);

private:
  Ui::MainWindow *ui;
  // From standard_implants.db
  QSqlDatabase standard_implants_;
  // From ladder_implants.txt
  std::vector<LadderSlot> ladder_slots_;
  bool config_not_empty_;
  QString current_file_;
  // Final implant configuration found by the most recent build
  ImplantConfiguration final_config_;
  ImplantConfiguration final_config_2;
  QString auno_link_;
  QString auno_link_2;
};

// Holds information for displaying items in the Shopping tab.
struct ShoppingItem{
  std::string cluster_;
  int ql_;
  bool operator<(const ShoppingItem& r) const{ return cluster_ < r.cluster_; }
};

#endif // MAINWINDOW_H_
