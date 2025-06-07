import 'package:flutter/material.dart';
import 'screens/splash_screen.dart';
import 'screens/login_screen.dart';
import 'screens/register_screen.dart';
import 'screens/home_screen.dart';

void main() {
  runApp(const BudgetBuddyApp());
}

class BudgetBuddyApp extends StatefulWidget {
  const BudgetBuddyApp({Key? key}) : super(key: key);

  @override
  State<BudgetBuddyApp> createState() => _BudgetBuddyAppState();
}

class _BudgetBuddyAppState extends State<BudgetBuddyApp> {
  String? _token;
  bool _showRegister = false;
  bool _showSplash = true;
  bool _isDarkMode = false;

  void _toggleTheme() {
    setState(() {
      _isDarkMode = !_isDarkMode;
    });
  }

  void _onSplashComplete() {
    setState(() {
      _showSplash = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    final primaryColor = const Color(0xFFF7B733);
    final secondaryColor = const Color(0xFFFC4A1A);
    final backgroundColor = const Color(0xFFF9F0D6);
    final textColor = const Color(0xFF081C2C);

    Widget screen;

    if (_showSplash) {
      screen = SplashScreen(onAnimationComplete: _onSplashComplete);
    } else if (_token == null) {
      screen = _showRegister
          ? RegisterScreen(
        onRegisterSuccess: () => setState(() => _showRegister = false),
        onShowLogin: () => setState(() => _showRegister = false),
      )
          : LoginScreen(
        onLoginSuccess: (token) => setState(() {
          _token = token;
        }),
        onShowRegister: () => setState(() => _showRegister = true),
      );
    } else {
      screen = HomeScreen(
        onLogout: () {
          setState(() {
            _token = null;
          });
        },
        isDarkMode: _isDarkMode,
        onToggleTheme: _toggleTheme,
      );
    }

    return MaterialApp(
      title: 'BudgetBuddy',
      theme: ThemeData(
        brightness: Brightness.light,
        primaryColor: primaryColor,
        colorScheme: ColorScheme.light(
          primary: primaryColor,
          onPrimary: Colors.white,
          secondary: secondaryColor,
          onSecondary: Colors.white,
          background: backgroundColor,
          onBackground: textColor,
          surface: Colors.white,
          onSurface: textColor,
          error: Colors.red,
          onError: Colors.white,
        ),
        scaffoldBackgroundColor: backgroundColor,
        appBarTheme: AppBarTheme(
          backgroundColor: primaryColor,
          foregroundColor: Colors.white,
          iconTheme: const IconThemeData(color: Colors.white),
        ),
        bottomNavigationBarTheme: BottomNavigationBarThemeData(
          backgroundColor: primaryColor,
          selectedItemColor: Colors.white,
          unselectedItemColor: Colors.white70,
          showUnselectedLabels: true,
          selectedLabelStyle: const TextStyle(fontWeight: FontWeight.bold),
        ),
      ),
      darkTheme: ThemeData(
        brightness: Brightness.dark,
        primaryColor: primaryColor,
        colorScheme: ColorScheme.dark(
          primary: primaryColor,
          onPrimary: Colors.white,
          secondary: secondaryColor,
          onSecondary: Colors.white,
          background: Colors.black,
          onBackground: Colors.white,
          surface: Colors.grey[900]!,
          onSurface: Colors.white,
          error: Colors.red[700]!,
          onError: Colors.white,
        ),
        scaffoldBackgroundColor: Colors.black,
        appBarTheme: AppBarTheme(
          backgroundColor: primaryColor,
          foregroundColor: Colors.white,
          iconTheme: const IconThemeData(color: Colors.white),
        ),
        bottomNavigationBarTheme: BottomNavigationBarThemeData(
          backgroundColor: primaryColor,
          selectedItemColor: Colors.white,
          unselectedItemColor: Colors.white70,
          showUnselectedLabels: true,
          selectedLabelStyle: const TextStyle(fontWeight: FontWeight.bold),
        ),
      ),
      themeMode: _isDarkMode ? ThemeMode.dark : ThemeMode.light,
      home: screen,
    );
  }
}
