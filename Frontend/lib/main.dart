import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'providers/auth_provider.dart';
import 'services/api_service.dart';
import 'screens/splash_screen.dart';
import 'screens/login_screen.dart';
import 'screens/register_screen.dart';
import 'screens/welcome_screen.dart';
import 'screens/onboarding_wizard_screen.dart';
import 'screens/home_screen.dart';
import 'screens/add_category_screen.dart';

void main() {
  runApp(
    ChangeNotifierProvider(
      create: (_) => AuthProvider(),
      child: const BudgetBuddyApp(),
    ),
  );
}

class BudgetBuddyApp extends StatefulWidget {
  const BudgetBuddyApp({Key? key}) : super(key: key);

  @override
  State<BudgetBuddyApp> createState() => BudgetBuddyAppState();
}

class BudgetBuddyAppState extends State<BudgetBuddyApp> {
  bool isDarkMode = false;
  bool _showSplash = true;
  bool _isCheckingData = false;
  bool _hasUserData = false;
  bool _showRegister = false;
  bool _showWizard = false;

  final ApiService _apiService = ApiService();
  late AuthProvider _authProvider;
  String? _prevToken;

  String _userDataKey(String? userId) => 'hasUserData_${userId ?? ''}';

  @override
  void initState() {
    super.initState();
    _authProvider = Provider.of<AuthProvider>(context, listen: false);
    _authProvider.addListener(_onAuthStateChanged);
    _initializeApp();
  }

  void _onAuthStateChanged() {
    final newToken = _authProvider.token;
    if (newToken != null && newToken.isNotEmpty && newToken != _prevToken) {
      _prevToken = newToken;
      _initializeApp();
    } else if (newToken == null || newToken.isEmpty) {
      setState(() {
        _hasUserData = false;
        _showWizard = false;
      });
    }
  }

  Future<void> _initializeApp() async {
    await _authProvider.loadFromPrefs();

    final token = _authProvider.token;
    final userId = _authProvider.userId;
    final prefs = await SharedPreferences.getInstance();
    final key = _userDataKey(userId);
    final savedFlag = prefs.getBool(key) ?? false;

    setState(() {
      _showSplash = true;
      _isCheckingData = token != null && token.isNotEmpty;
    });

    if (token != null && token.isNotEmpty) {
      try {
        final hasData = await _apiService.checkUserHasData(token);
        await prefs.setBool(key, hasData);
        setState(() => _hasUserData = hasData);

        if (hasData && _authProvider.budgetId == null) {
          final budgets = await _apiService.getUserBudgets(token);
          if (budgets.isNotEmpty) {
            final firstId = budgets.first['id'] as int;
            await _authProvider.setBudgetId(firstId);
          }
        }
      } catch (_) {
        setState(() => _hasUserData = savedFlag);
      }
    } else {
      setState(() => _hasUserData = savedFlag);
    }

    await Future.delayed(const Duration(milliseconds: 800));
    setState(() {
      _showSplash = false;
      _isCheckingData = false;
    });
  }

  void _startWizard() => setState(() => _showWizard = true);

  Future<void> _finishWizard() async {
    final prefs = await SharedPreferences.getInstance();
    final key = _userDataKey(_authProvider.userId);
    await prefs.setBool(key, true);
    setState(() {
      _hasUserData = true;
      _showWizard = false;
      _isCheckingData = false;
    });
  }

  @override
  void dispose() {
    _authProvider.removeListener(_onAuthStateChanged);
    super.dispose();
  }

  void toggleTheme() => setState(() => isDarkMode = !isDarkMode);

  @override
  Widget build(BuildContext context) {
    final auth = Provider.of<AuthProvider>(context);
    final token = auth.token;

    Widget currentScreen;
    if (_showSplash) {
      currentScreen = SplashScreen(onAnimationComplete: () {});
    } else if (_isCheckingData) {
      currentScreen = const Scaffold(
        body: Center(child: CircularProgressIndicator()),
      );
    } else if (token == null || token.isEmpty) {
      currentScreen = _showRegister
          ? RegisterScreen(
        onRegisterSuccess: () =>
            setState(() => _showRegister = false),
        onShowLogin: () =>
            setState(() => _showRegister = false),
      )
          : LoginScreen(
        onShowRegister: () =>
            setState(() => _showRegister = true),
        onLoginSuccess: _initializeApp,
      );
    } else if (!_hasUserData) {
      currentScreen = !_showWizard
          ? WelcomeScreen(onStartWizard: _startWizard)
          : OnboardingWizardScreen(onFinish: _finishWizard);
    } else {
      currentScreen = HomeScreen(
        onLogout: () async {
          await auth.clearToken();
          setState(() {
            _hasUserData = false;
            _showWizard = false;
            _showRegister = false;
          });
        },
        isDarkMode: isDarkMode,
        onToggleTheme: toggleTheme,
      );
    }

    const primaryColor = Color(0xFFF7B733);
    const secondaryColor = Color(0xFFFC4A1A);
    const backgroundColor = Color(0xFFF9F0D6);
    const textColor = Color(0xFF081C2C);

    return MaterialApp(
      title: 'BudgetBuddy',
      theme: ThemeData(
        brightness: Brightness.light,
        primaryColor: primaryColor,
        colorScheme: const ColorScheme.light(
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
        appBarTheme:
        const AppBarTheme(backgroundColor: primaryColor, foregroundColor: Colors.white),
        bottomNavigationBarTheme: const BottomNavigationBarThemeData(
          backgroundColor: primaryColor,
          selectedItemColor: Colors.white,
          unselectedItemColor: Colors.white70,
        ),
      ),
      darkTheme: ThemeData(
        brightness: Brightness.dark,
        primaryColor: primaryColor,
        colorScheme: const ColorScheme.dark(
          primary: primaryColor,
          onPrimary: Colors.white,
          secondary: secondaryColor,
          onSecondary: Colors.white,
          background: Colors.black,
          onBackground: Colors.white,
          surface: Colors.grey,
          onSurface: Colors.white,
          error: Colors.red,
          onError: Colors.white,
        ),
        scaffoldBackgroundColor: Colors.black,
        appBarTheme:
        const AppBarTheme(backgroundColor: primaryColor, foregroundColor: Colors.white),
        bottomNavigationBarTheme: const BottomNavigationBarThemeData(
          backgroundColor: primaryColor,
          selectedItemColor: Colors.white,
          unselectedItemColor: Colors.white70,
        ),
      ),
      themeMode: isDarkMode ? ThemeMode.dark : ThemeMode.light,
      routes: {
        '/add-category': (context) {
          final type = ModalRoute.of(context)!.settings.arguments as String? ?? 'income';
          return AddCategoryScreen(initialType: type);
        },
      },
      home: currentScreen,
    );
  }
}