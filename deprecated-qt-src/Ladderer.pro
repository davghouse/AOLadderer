#-------------------------------------------------
#
# Project created by QtCreator 2013-10-01T12:57:11
#
#-------------------------------------------------
 database.sources = "standard_implants.db"
 database.path = .
 DEPLOYMENT += database
QT       += core gui sql

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Ladderer
TEMPLATE = app
RC_FILE = ladder.rc


SOURCES +=\
    character_stats.cc \
    implant.cc \
    implant_configuration.cc \
    main.cc \
    mainwindow.cc \
    ladder.cc \
    ladder_helper.cc

HEADERS  += mainwindow.h \
    interpolation_coefficients.h \
    implant_configuration.h \
    implant.h \
    character_stats.h \
    ladder_slot.h \
    ladder.h \
    ladder_helper.h

FORMS    += mainwindow.ui

OTHER_FILES += \
    standard_implants.db \
    ladder_implants.txt
