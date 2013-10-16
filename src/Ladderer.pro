#-------------------------------------------------
#
# Project created by QtCreator 2013-10-01T12:57:11
#
#-------------------------------------------------
 database.sources = "StandardImplants.db"
 database.path = .
 DEPLOYMENT += database
QT       += core gui sql

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Ladderer
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    Stats.cpp \
    Ladder.cpp \
    Helper.cpp \
    Config.cpp \
    Implant.cpp

HEADERS  += mainwindow.h \
    Stats.h \
    Slot.h \
    Ladder.h \
    Implant.h \
    Helper.h \
    Config.h \
    Coefficients.h

FORMS    += mainwindow.ui

OTHER_FILES += \
    StandardImplants.db \
    ladderImplants.txt
