#ifndef MAINWINDOW_H_
#define MAINWINDOW_H_

#include <QMainWindow>
#include <QtSql/QSqlDatabase>
#include <QSqlQuery>
#include <QModelIndex>
#include "QString"
#include "QTextStream"
#include <vector>
#include "ladder.h"

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

  void ToggleIronCircle(bool);
  void ToggleProdigiousStrength(bool);

  void ToggleNeuronalStimulator(bool);
  void ToggleOdinsMissingEye(bool);

  // Build:
  void RunHeightOne();

  // Shopping list:
  void ToggleBoughtShining(QModelIndex);
  void ToggleBoughtBright(QModelIndex);
  void ToggleBoughtFaded(QModelIndex);

  // File menu:
  void Open();
  void Save();
  void SaveAs();
  void ExportToAuno();

private:
  // File menu:
  void SaveFile(QString&);
  void LoadFile(QString&);
  void SaveBuildTab(QTextStream&);
  void LoadBuildTab(QTextStream&);
  void SaveResultsTab(QTextStream&);
  void LoadResultsTab(QTextStream&);
  void SaveShoppingTab(QTextStream&);
  void LoadShoppingTab(QTextStream&);

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
  // Does the conversion.
  void GetConfigurationHelper(ImplantConfiguration& required_config,
                              int i, const std::string& slot, std::string& shi,
                              std::string& bri, std::string& fad);
  void GetStats(CharacterStats& base_stats);
  // Fills in the other two tabs of the GUI, taking special care to
  // print out the dashed separator line.
  void ShowHeightOne(const Ladder& ladder);
  void ShowImplant(const Implant& implant, std::string& shi,
                   std::string& bri, std::string& fad, int step);

private:
  Ui::MainWindow *ui;
  QSqlDatabase standard_implants_;
  std::vector<LadderSlot> ladder_slots_;
  bool config_not_empty_;
  QString current_file_;
};

#endif // MAINWINDOW_H_
