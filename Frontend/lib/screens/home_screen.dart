import 'package:flutter/material.dart';

class HomeScreen extends StatelessWidget {
  final VoidCallback onShowExpenses;
  final VoidCallback onAddExpense;
  final VoidCallback onShowIncomes;
  final VoidCallback onAddIncome;
  final VoidCallback onShowAccounts;
  final VoidCallback onAddAccount;
  final VoidCallback onShowGoals;
  final VoidCallback onAddGoal;
  final VoidCallback onAddInvoice;
  final VoidCallback onShowCategories;
  final VoidCallback onLogout;

  const HomeScreen({
    Key? key,
    required this.onShowExpenses,
    required this.onAddExpense,
    required this.onShowIncomes,
    required this.onAddIncome,
    required this.onShowAccounts,
    required this.onAddAccount,
    required this.onShowGoals,
    required this.onAddGoal,
    required this.onAddInvoice,
    required this.onShowCategories,
    required this.onLogout,
  }) : super(key: key);

  Widget buildSectionTitle(String text) => Padding(
        padding: const EdgeInsets.only(top: 16.0, bottom: 8.0),
        child: Text(
          text,
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
      );

  Widget buildButton(
      {required String label, required IconData icon, required VoidCallback onTap}) {
    return Expanded(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 6.0),
        child: ElevatedButton.icon(
          style: ElevatedButton.styleFrom(
            padding: const EdgeInsets.symmetric(vertical: 12),
            backgroundColor: Colors.blue[50],
            foregroundColor: Colors.blue[900],
            elevation: 0,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            textStyle: const TextStyle(fontWeight: FontWeight.w600, fontSize: 14),
          ),
          onPressed: onTap,
          icon: Icon(icon, size: 22),
          label: Text(label),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('BudgetBuddy'),
        actions: [
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 8.0),
            child: ElevatedButton.icon(
              onPressed: onLogout,
              icon: const Icon(Icons.logout, color: Colors.white),
              label: const Text(
                'Wyloguj się',
                style: TextStyle(
                  color: Colors.white, fontWeight: FontWeight.bold,
                  fontSize: 16,
                ),
              ),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.red,
                foregroundColor: Colors.white,
                elevation: 0,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(30),
                ),
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
                textStyle: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
              ),
            ),
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Wydatki
            buildSectionTitle("Wydatki"),
            Row(
              children: [
                buildButton(
                  label: "Dodaj wydatek",
                  icon: Icons.add_shopping_cart,
                  onTap: onAddExpense,
                ),
                buildButton(
                  label: "Pokaż wydatki",
                  icon: Icons.list_alt,
                  onTap: onShowExpenses,
                ),
              ],
            ),

            // Przychody
            buildSectionTitle("Przychody"),
            Row(
              children: [
                buildButton(
                  label: "Dodaj przychód",
                  icon: Icons.add_card,
                  onTap: onAddIncome,
                ),
                buildButton(
                  label: "Pokaż przychody",
                  icon: Icons.attach_money,
                  onTap: onShowIncomes,
                ),
              ],
            ),

            // Konta
            buildSectionTitle("Konta"),
            Row(
              children: [
                buildButton(
                  label: "Moje konta",
                  icon: Icons.account_balance_wallet,
                  onTap: onShowAccounts,
                ),
                buildButton(
                  label: "Dodaj konto",
                  icon: Icons.account_balance,
                  onTap: onAddAccount,
                ),
              ],
            ),

            // Cele
            buildSectionTitle("Cele oszczędnościowe"),
            Row(
              children: [
                buildButton(
                  label: "Moje cele",
                  icon: Icons.flag,
                  onTap: onShowGoals,
                ),
                buildButton(
                  label: "Dodaj cel",
                  icon: Icons.add_task,
                  onTap: onAddGoal,
                ),
              ],
            ),

            // Inne
            buildSectionTitle("Inne"),
            Row(
              children: [
                buildButton(
                  label: "Dodaj fakturę",
                  icon: Icons.receipt_long,
                  onTap: onAddInvoice,
                ),
                buildButton(
                  label: "Kategorie",
                  icon: Icons.category,
                  onTap: onShowCategories,
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
