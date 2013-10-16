#ifndef MAINWINDOW_H_
#define MAINWINDOW_H_

#include <QMainWindow>
#include <QtSql/QSqlDatabase>
#include <QSqlQuery>
#include <QModelIndex>
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

  void RunHeightOne();

  void ToggleBoughtShining(QModelIndex);
  void ToggleBoughtBright(QModelIndex);
  void ToggleBoughtFaded(QModelIndex);

private:
  void GetConfiguration(ImplantConfiguration&);
  void GetConfigurationHelper(ImplantConfiguration&, int, const std::string&, std::string&, std::string&, std::string&);
  void GetStats(Stats&);
  void ShowHeightOne(const Ladder&);
  void ShowImplant(const Implant&, std::string&, std::string&, std::string&, int);

private:
  Ui::MainWindow *ui;
  QSqlDatabase standard_implants_;
  std::vector<LadderSlot> ladder_slots_;
  bool config_not_empty_;
};

#endif // MAINWINDOW_H_
