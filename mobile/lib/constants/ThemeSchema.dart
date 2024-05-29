import 'package:flutter/material.dart';

ThemeData themeSchema = ThemeData(
  brightness: Brightness.light,
  useMaterial3: true,

  appBarTheme: const AppBarTheme(
    backgroundColor: Colors.blue,
    shadowColor: Colors.transparent,
    elevation: 0.0,
    centerTitle: true
  ),

  colorScheme: ColorScheme.fromSeed(seedColor: Colors.blue),
);
