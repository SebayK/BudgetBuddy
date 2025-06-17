import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

// GŁÓWNY WIDOK TRANSAKCJI
class TransactionsScreen extends StatefulWidget {
  final List<Map<String, dynamic>> incomes;
  final List<Map<String, dynamic>> expenses;

  const TransactionsScreen({
    Key? key,
    required this.incomes,
    required this.expenses,
  }) : super(key: key);

  @override
  State<TransactionsScreen> createState() => _TransactionsScreenState();
}

class _TransactionsScreenState extends State<TransactionsScreen> {
  late List<Map<String, dynamic>> incomes;
  late List<Map<String, dynamic>> expenses;

  @override
  void initState() {
    super.initState();
    incomes = List<Map<String, dynamic>>.from(widget.incomes);
    expenses = List<Map<String, dynamic>>.from(widget.expenses);
  }

  void _addTransaction(Map<String, dynamic> transaction) {
    setState(() {
      if (transaction['type'] == 'Dochód') {
        incomes.add({'name': transaction['name'], 'amount': transaction['amount']});
      } else {
        expenses.add({'name': transaction['name'], 'amount': transaction['amount']});
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final bool isEmpty = incomes.isEmpty && expenses.isEmpty;

    return Scaffold(
      backgroundColor: theme.colorScheme.background,
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Transakcje',
                style: theme.textTheme.headlineLarge?.copyWith(
                  fontWeight: FontWeight.bold,
                  color: Colors.amber[800],
                ),
              ),
              const SizedBox(height: 24),
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
                            Icon(Icons.arrow_upward, color: Colors.green),
                            const SizedBox(height: 8),
                            Text('Dochody', style: theme.textTheme.titleMedium),
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
                            Icon(Icons.arrow_downward, color: Colors.red),
                            const SizedBox(height: 8),
                            Text('Wydatki', style: theme.textTheme.titleMedium),
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
              Row(
                children: [
                  Expanded(
                    child: Card(
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: const SizedBox(
                        height: 90,
                        child: Center(child: Text('Analiza\n(wkrótce)', textAlign: TextAlign.center)),
                      ),
                    ),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    child: Card(
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: const SizedBox(
                        height: 90,
                        child: Center(child: Text('Planowane\n(wkrótce)', textAlign: TextAlign.center)),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 32),
              Expanded(
                child: isEmpty
                    ? Center(
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(Icons.info_outline, size: 48, color: Colors.grey),
                      const SizedBox(height: 12),
                      Text('Nie znaleziono transakcji', style: theme.textTheme.titleMedium),
                      const SizedBox(height: 8),
                      Text(
                        'Kliknij przycisk "+", aby dodać nową transakcję',
                        style: theme.textTheme.bodyMedium,
                      ),
                    ],
                  ),
                )
                    : ListView(
                  children: [
                    if (incomes.isNotEmpty) ...[
                      Padding(
                        padding: const EdgeInsets.symmetric(vertical: 8),
                        child: Text(
                          'Dochody',
                          style: theme.textTheme.titleMedium?.copyWith(color: Colors.green[700]),
                        ),
                      ),
                      ...incomes.map(
                            (income) => ListTile(
                          leading: const Icon(Icons.add_circle, color: Colors.green),
                          title: Text(income['name']),
                          trailing: Text('${income['amount']} zł'),
                        ),
                      ),
                    ],
                    if (expenses.isNotEmpty) ...[
                      Padding(
                        padding: const EdgeInsets.symmetric(vertical: 8),
                        child: Text(
                          'Wydatki',
                          style: theme.textTheme.titleMedium?.copyWith(color: Colors.red[700]),
                        ),
                      ),
                      ...expenses.map(
                            (expense) => ListTile(
                          leading: const Icon(Icons.remove_circle, color: Colors.red),
                          title: Text(expense['name']),
                          trailing: Text('${expense['amount']} zł'),
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
        onPressed: () async {
          final newTransaction = await Navigator.of(context).push<Map<String, dynamic>>(
            MaterialPageRoute(
              builder: (context) => const AddTransactionScreen(),
            ),
          );
          if (newTransaction != null) {
            _addTransaction(newTransaction);
          }
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

// --------- FORMULARZ DODAWANIA TRANSAKCJI ---------
class AddTransactionScreen extends StatefulWidget {
  const AddTransactionScreen({Key? key}) : super(key: key);

  @override
  State<AddTransactionScreen> createState() => _AddTransactionScreenState();
}

class _AddTransactionScreenState extends State<AddTransactionScreen> {
  final _formKey = GlobalKey<FormState>();
  String _name = '';
  String _type = 'Wydatek';
  double _amount = 0;
  bool _loading = false;
  String? _error;

  Future<void> _submit() async {
    if (!_formKey.currentState!.validate()) return;
    _formKey.currentState!.save();

    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      final uri = Uri.parse('http://localhost:5000/transactions'); // <- zmień na swój endpoint
      final response = await http.post(
        uri,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'name': _name,
          'amount': _amount,
          'type': _type,
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        Navigator.of(context).pop({
          'name': _name,
          'amount': _amount,
          'type': _type,
        });
      } else {
        setState(() {
          _error = 'Błąd serwera: ${response.statusCode}';
        });
      }
    } catch (e) {
      setState(() {
        _error = 'Błąd połączenia: $e';
      });
    } finally {
      setState(() {
        _loading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Nowa transakcja')),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              DropdownButtonFormField<String>(
                value: _type,
                decoration: const InputDecoration(labelText: 'Typ transakcji'),
                items: ['Wydatek', 'Dochód']
                    .map((e) => DropdownMenuItem(value: e, child: Text(e)))
                    .toList(),
                onChanged: (v) => setState(() => _type = v ?? 'Wydatek'),
              ),
              TextFormField(
                decoration: const InputDecoration(labelText: 'Nazwa'),
                validator: (v) => v == null || v.isEmpty ? 'Podaj nazwę' : null,
                onSaved: (v) => _name = v ?? '',
              ),
              TextFormField(
                decoration: const InputDecoration(labelText: 'Kwota'),
                keyboardType: TextInputType.number,
                validator: (v) => v == null || double.tryParse(v) == null
                    ? 'Podaj poprawną kwotę'
                    : null,
                onSaved: (v) => _amount = double.tryParse(v ?? '0') ?? 0,
              ),
              const SizedBox(height: 24),
              if (_loading)
                const Center(child: CircularProgressIndicator())
              else
                ElevatedButton(
                  onPressed: _submit,
                  child: const Text('Dodaj transakcję'),
                ),
              if (_error != null) ...[
                const SizedBox(height: 12),
                Text(_error!, style: const TextStyle(color: Colors.red)),
              ]
            ],
          ),
        ),
      ),
    );
  }
}
