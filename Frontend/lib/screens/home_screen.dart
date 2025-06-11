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
                          padding:
                          const EdgeInsets.symmetric(vertical: 6),
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
                              Text('Planowane',
                                  style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  planned.isEmpty
                                      ? 'Brak planowanych transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium
                                      ?.copyWith(
                                      fontStyle: FontStyle.italic),
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
                              Text('Oczekujące',
                                  style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  pending.isEmpty
                                      ? 'Brak oczekujących transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium
                                      ?.copyWith(
                                      fontStyle: FontStyle.italic),
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

// Rozbudowany Ekran Transakcje
class TransactionsScreen extends StatelessWidget {
  final List<Map<String, dynamic>> incomes;
  final List<Map<String, dynamic>> expenses;
  final bool isEmpty;

  TransactionsScreen({
    Key? key,
    this.incomes = const [],
    this.expenses = const [],
  })  : isEmpty = incomes.isEmpty && expenses.isEmpty,
        super(key: key);

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      backgroundColor: theme.colorScheme.background,
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Nagłówek
              Text(
                'Transakcje',
                style: theme.textTheme.headlineLarge?.copyWith(
                  fontWeight: FontWeight.bold,
                  color: Colors.amber[800],
                ),
              ),
              const SizedBox(height: 24),

              // Podział na Dochody i Wydatki
              Row(
                children: [
                  Expanded(
                    child: Card(
                      color: Colors.green[100],
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(16),
                      ),
                      child: Padding(
                        padding: const EdgeInsets.all(16),
                        child: Column(
                          children: [
                            Icon(Icons.arrow_downward, color: Colors.green),
                            const SizedBox(height: 8),
                            Text(
                              'Dochody',
                              style: theme.textTheme.titleMedium,
                            ),
                            const SizedBox(height: 8),
                            Text(
                              incomes.isNotEmpty
                                  ? '${incomes.fold(0, (sum, i) => sum + (i['amount'] as int))} zł'
                                  : '0 zł',
                              style: theme.textTheme.headlineSmall,
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    child: Card(
                      color: Colors.red[100],
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(16),
                      ),
                      child: Padding(
                        padding: const EdgeInsets.all(16),
                        child: Column(
                          children: [
                            Icon(Icons.arrow_upward, color: Colors.red),
                            const SizedBox(height: 8),
                            Text(
                              'Wydatki',
                              style: theme.textTheme.titleMedium,
                            ),
                            const SizedBox(height: 8),
                            Text(
                              expenses.isNotEmpty
                                  ? '${expenses.fold(0, (sum, i) => sum + (i['amount'] as int))} zł'
                                  : '0 zł',
                              style: theme.textTheme.headlineSmall,
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 24),

              // Analiza i Planowane
              Row(
                children: [
                  Expanded(
                    child: Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 90,
                        child: Center(
                          child: Text(
                            'Analiza\n(wkrótce)',
                            style: theme.textTheme.bodyMedium,
                            textAlign: TextAlign.center,
                          ),
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    child: Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 90,
                        child: Center(
                          child: Text(
                            'Planowane\n(wkrótce)',
                            style: theme.textTheme.bodyMedium,
                            textAlign: TextAlign.center,
                          ),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 32),

              // Lista transakcji lub pusty stan
              Expanded(
                child: isEmpty
                    ? Center(
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.info_outline,
                          size: 48, color: Colors.grey),
                      const SizedBox(height: 12),
                      Text(
                        'Nie znaleziono transakcji',
                        style: theme.textTheme.titleMedium,
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'Kliknij przycisk "+", aby dodać nową transakcję',
                        style: theme.textTheme.bodyMedium,
                      ),
                      const SizedBox(height: 20),
                      ElevatedButton.icon(
                        onPressed: () {
                          // Dodaj nawigację do dodawania transakcji
                        },
                        icon: const Icon(Icons.add),
                        label: const Text('Nowa transakcja'),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: Colors.amber,
                          foregroundColor: Colors.black,
                          textStyle: const TextStyle(fontSize: 18),
                          padding: const EdgeInsets.symmetric(
                              horizontal: 24, vertical: 12),
                          shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(12)),
                        ),
                      ),
                    ],
                  ),
                )
                    : ListView(
                  children: [
                    if (incomes.isNotEmpty) ...[
                      Padding(
                        padding:
                        const EdgeInsets.symmetric(vertical: 8),
                        child: Text(
                          'Dochody',
                          style: theme.textTheme.titleMedium
                              ?.copyWith(color: Colors.green[700]),
                        ),
                      ),
                      ...incomes.map(
                            (income) => ListTile(
                          leading: const Icon(Icons.add_circle,
                              color: Colors.green),
                          title: Text(income['name']),
                          trailing:
                          Text('${income['amount']} zł'),
                        ),
                      ),
                    ],
                    if (expenses.isNotEmpty) ...[
                      Padding(
                        padding:
                        const EdgeInsets.symmetric(vertical: 8),
                        child: Text(
                          'Wydatki',
                          style: theme.textTheme.titleMedium
                              ?.copyWith(color: Colors.red[700]),
                        ),
                      ),
                      ...expenses.map(
                            (expense) => ListTile(
                          leading: const Icon(Icons.remove_circle,
                              color: Colors.red),
                          title: Text(expense['name']),
                          trailing:
                          Text('${expense['amount']} zł'),
                        ),
                      ),
                    ],
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          // Dodaj przejście do ekranu dodawania transakcji
        },
        icon: const Icon(Icons.add),
        label: const Text('Nowa transakcja'),
        backgroundColor: Colors.amber,
        foregroundColor: Colors.black,
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
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

  // Przykładowe dane (możesz później podmienić na rzeczywiste)
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
      const OverviewScreen(),
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
          BottomNavigationBarItem(
              icon: Icon(Icons.dashboard), label: 'Przegląd'),
          BottomNavigationBarItem(
              icon: Icon(Icons.swap_horiz), label: 'Transakcje'),
          BottomNavigationBarItem(
              icon: Icon(Icons.account_balance_wallet), label: 'Budżety'),
          BottomNavigationBarItem(
              icon: Icon(Icons.more_horiz), label: 'Inne'),
        ],
      ),
    );
  }
}
