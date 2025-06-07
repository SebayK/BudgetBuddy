import 'package:flutter/material.dart';

// Ekran Przegląd
class OverviewScreen extends StatelessWidget {
  const OverviewScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final accounts = [
      {'name': 'Konto domyślne', 'balance': 2500},
      {'name': 'Oszczędności', 'balance': 10000},
    ];

    final planned = <Map<String, dynamic>>[];
    final pending = <Map<String, dynamic>>[];

    final theme = Theme.of(context);
    final textStyleHeader =
    theme.textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold);
    final textStyleSectionTitle =
    theme.textTheme.titleMedium?.copyWith(fontWeight: FontWeight.w600);

    return Scaffold(
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('Przegląd', style: textStyleHeader),
                const SizedBox(height: 20),

                // Duża karta saldo kont
                Card(
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16),
                  ),
                  color: theme.colorScheme.surfaceVariant,
                  child: Padding(
                    padding: const EdgeInsets.all(24),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Salda kont (2025)', style: textStyleSectionTitle),
                        const SizedBox(height: 16),
                        ...accounts.map((acc) => Padding(
                          padding: const EdgeInsets.symmetric(vertical: 6),
                          child: Row(
                            mainAxisAlignment:
                            MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                acc['name'].toString(),
                                style: theme.textTheme.bodyLarge,
                              ),
                              Text(
                                '${acc['balance'].toString()} zł',
                                style: theme.textTheme.bodyLarge,
                              ),
                            ],
                          ),
                        )),
                      ],
                    ),
                  ),
                ),

                const SizedBox(height: 24),

                // Planowane i Oczekujące w kolumnie
                Column(
                  children: [
                    Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 140,
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text('Planowane', style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  planned.isEmpty
                                      ? 'Brak planowanych transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium
                                      ?.copyWith(fontStyle: FontStyle.italic),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              const Spacer(),
                            ],
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 16),
                    Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 140,
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text('Oczekujące', style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  pending.isEmpty
                                      ? 'Brak oczekujących transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium
                                      ?.copyWith(fontStyle: FontStyle.italic),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              const Spacer(),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ],
                ),

                const SizedBox(height: 50),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

// Ekran Transakcje (dochody + wydatki razem)
class TransactionsScreen extends StatelessWidget {
  const TransactionsScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final transactions = [
      {'type': 'Wydatek', 'name': 'Zakupy spożywcze', 'amount': 120},
      {'type': 'Wydatek', 'name': 'Kawa', 'amount': 15},
      {'type': 'Przychód', 'name': 'Wynagrodzenie', 'amount': 3500},
    ];

    final theme = Theme.of(context);

    return ListView(
      children: transactions
          .map((tx) => ListTile(
        leading: Icon(
            tx['type'].toString() == 'Wydatek'
                ? Icons.remove_circle
                : Icons.add_circle,
            color: tx['type'].toString() == 'Wydatek'
                ? Colors.red
                : Colors.green),
        title: Text(
          tx['name'].toString(),
          style: theme.textTheme.bodyLarge,
        ),
        trailing: Text(
          '${tx['amount']} zł',
          style: theme.textTheme.bodyLarge,
        ),
      ))
          .toList(),
    );
  }
}

// Ekran Budżety (planowane + dostępne)
class BudgetsScreen extends StatelessWidget {
  const BudgetsScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final budgets = [
      {'name': 'Planowane wydatki', 'amount': 2000},
      {'name': 'Dostępne środki', 'amount': 4500},
    ];

    final theme = Theme.of(context);

    return ListView(
      children: budgets
          .map((b) => ListTile(
        title: Text(
          b['name'].toString(),
          style: theme.textTheme.bodyLarge,
        ),
        trailing: Text(
          '${b['amount']} zł',
          style: theme.textTheme.bodyLarge,
        ),
      ))
          .toList(),
    );
  }
}

// Ekran Inne
class OtherScreen extends StatelessWidget {
  const OtherScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Center(
      child: Text(
        'Inne funkcje pojawią się tutaj',
        style: theme.textTheme.bodyLarge,
      ),
    );
  }
}

// Główny ekran z dolnym paskiem nawigacji
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

  late final List<Widget> _pages = const [
    OverviewScreen(),
    TransactionsScreen(),
    BudgetsScreen(),
    OtherScreen(),
  ];

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  IconData get themeIcon => widget.isDarkMode ? Icons.dark_mode : Icons.light_mode;

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
