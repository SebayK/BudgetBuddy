import 'package:flutter/material.dart';

class BudgetsScreen extends StatefulWidget {
  const BudgetsScreen({Key? key}) : super(key: key);

  @override
  State<BudgetsScreen> createState() => _BudgetsScreenState();
}

class _BudgetsScreenState extends State<BudgetsScreen> {
  final List<Map<String, dynamic>> _accounts = [
    {'name': 'Konto domyślne', 'type': 'Oszczędnościowe', 'currency': 'PLN'},
  ];

  void _showAddAccountDialog() {
    final _nameController = TextEditingController();
    final _typeController = TextEditingController();
    final _currencyController = TextEditingController();

    showDialog(
      context: context,
      barrierDismissible: true,
      builder: (BuildContext context) {
        return AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
          title: const Text('Dodaj konto'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: _nameController,
                  decoration: const InputDecoration(labelText: 'Nazwa konta'),
                ),
                TextField(
                  controller: _typeController,
                  decoration: const InputDecoration(labelText: 'Typ konta'),
                ),
                TextField(
                  controller: _currencyController,
                  decoration: const InputDecoration(labelText: 'Waluta'),
                ),
              ],
            ),
          ),
          actions: [
            TextButton(
              child: const Text('Anuluj'),
              onPressed: () => Navigator.of(context).pop(),
            ),
            ElevatedButton(
              child: const Text('Dodaj'),
              onPressed: () {
                setState(() {
                  _accounts.add({
                    'name': _nameController.text,
                    'type': _typeController.text,
                    'currency': _currencyController.text,
                  });
                });
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      body: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          Text(
            'Twoje konta',
            style: theme.textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 16),
          ..._accounts.map((acc) => Card(
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            child: ListTile(
              title: Text(acc['name']),
              subtitle: Text('${acc['type']} • ${acc['currency']}'),
              leading: const Icon(Icons.account_balance_wallet_outlined),
            ),
          )),
        ],
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: _showAddAccountDialog,
        icon: const Icon(Icons.add),
        label: const Text('Dodaj konto'),
        backgroundColor: Colors.amber,
        foregroundColor: Colors.black,
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
    );
  }
}