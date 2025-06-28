// lib/screens/home_screen.dart

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/auth_provider.dart';
import 'overview_screen.dart';
import 'transactions_screen.dart';  // ← względny import pliku w tym samym folderze
import 'budgets_screen.dart';
import 'other_screen.dart';

class HomeScreen extends StatefulWidget {
  final VoidCallback onLogout;
  final VoidCallback onToggleTheme;
  final bool isDarkMode;

  const HomeScreen({
    Key? key,
    required this.onLogout,
    required this.onToggleTheme,
    required this.isDarkMode,
  }) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _selectedIndex = 0;
  void _onItemTapped(int idx) => setState(() => _selectedIndex = idx);
  IconData get _themeIcon =>
      widget.isDarkMode ? Icons.dark_mode : Icons.light_mode;

  @override
  Widget build(BuildContext context) {
    final auth     = Provider.of<AuthProvider>(context);
    final userId   = auth.userId!;
    final budgetId = auth.budgetId ?? 0;

    final pages = <Widget>[
      const OverviewScreen(),
      TransactionsScreen(userId: userId, budgetId: budgetId),
      const BudgetsScreen(),
      const OtherScreen(),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text('BudgetBuddy'),
        actions: [
          IconButton(icon: Icon(_themeIcon), onPressed: widget.onToggleTheme),
          IconButton(icon: const Icon(Icons.logout), onPressed: widget.onLogout),
        ],
      ),
      body: pages[_selectedIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: _onItemTapped,
        type: BottomNavigationBarType.fixed,
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.dashboard), label: 'Przegląd'),
          BottomNavigationBarItem(icon: Icon(Icons.swap_horiz), label: 'Transakcje'),
          BottomNavigationBarItem(icon: Icon(Icons.account_balance_wallet), label: 'Konta'),
          BottomNavigationBarItem(icon: Icon(Icons.more_horiz), label: 'Inne'),
        ],
      ),
    );
  }
}
