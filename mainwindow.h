#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QtSql/QSqlDatabase>
#include <QSqlQuery>
#include <QModelIndex>
#include <vector>
#include "Ladder.h"

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
  void buffSCE(bool);
  void buffSFA(bool);
  void buffCM(bool);
  void buffCA(bool);
  void buffES(bool);
  void buffFG(bool);
  void buffEOB(bool);
  void buffICRT(bool);
  void buffG(bool);
  void buffEB(bool);

  void buffIC(bool);
  void buffPS(bool);

  void buffNS(bool);
  void buffOME(bool);

  void runHeightOneLaddered();

  void markAsBoughtShining(QModelIndex);
  void markAsBoughtBright(QModelIndex);
  void markAsBoughtFaded(QModelIndex);

private:
  void getConfig(Config&);
  void getStats(Stats&);
  void getConfigHelper(Config&, int, const std::string&, std::string&, std::string&, std::string&);
  void showHeightOne(const Ladder&);
  void showImplant(const Implant&, std::string&, std::string&, std::string&, int);

private:
  Ui::MainWindow *ui;
  QSqlDatabase implantDB;
  std::vector<Slot> ladderSlots;
  bool configNotEmpty;
};

#endif // MAINWINDOW_H
