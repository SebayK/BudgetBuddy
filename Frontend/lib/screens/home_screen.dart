import 'package:flutter/material.dart';
import '../screens/overview_screen.dart';
import '../screens/transactions_screen.dart';
import '../screens/budgets_screen.dart';
import '../screens/other_screen.dart';

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

  final List<Map<String, dynamic>> incomes = [
    {'name': 'Wynagrodzenie', 'amount': 3500},
    {'name': 'Zwrot podatku', 'amount': 700},
  ];
  final List<Map<String, dynamic>> expenses = [
    {'name': 'Zakupy spożywcze', 'amount': 250},
    {'name': 'Paliwo', 'amount': 150},
    {'name': 'Internet', 'amount': 90},
  ];

  late final List<Widget> _pages;

  @override
  void initState() {
    super.initState();
    _pages = [
      const OverviewScreen(), // ✅ Bez parametrów
      TransactionsScreen(
        incomes: incomes,
        expenses: expenses,
      ),
      const BudgetsScreen(),
      const OtherScreen(),
    ];
  }

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  IconData get themeIcon =>
      widget.isDarkMode ? Icons.dark_mode : Icons.light_mode;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('BudgetBuddy'),
        actions: [
          IconButton(
            icon: Icon(themeIcon),
            tooltip: 'Przełącz tryb jasny/ciemny',
            onPressed: widget.onToggleTheme,
          ),
          IconButton(
            icon: const Icon(Icons.logout),
            tooltip: 'Wyloguj się',
            onPressed: widget.onLogout,
          ),
        ],
      ),
      body: _pages[_selectedIndex],
      bottomNavigationBar: BottomNavigationBar(
        type: BottomNavigationBarType.fixed,
        currentIndex: _selectedIndex,
        onTap: _onItemTapped,
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.dashboard), label: 'Przegląd'),
          BottomNavigationBarItem(icon: Icon(Icons.swap_horiz), label: 'Transakcje'),
          BottomNavigationBarItem(icon: Icon(Icons.account_balance_wallet), label: 'Budżety'),
          BottomNavigationBarItem(icon: Icon(Icons.more_horiz), label: 'Inne'),
        ],
      ),
    );
  }
}
