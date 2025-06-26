// lib/screens/add_transaction_screen.dart

import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';

import '../providers/auth_provider.dart';
import '../services/api_service.dart';

class AddTransactionScreen extends StatefulWidget {
  final String userId;
  final int budgetId;
  final String type; // 'income' lub 'expense'

  const AddTransactionScreen({
    Key? key,
    required this.userId,
    required this.budgetId,
    required this.type,
  }) : super(key: key);

  @override
  State<AddTransactionScreen> createState() => _AddTransactionScreenState();
}

class _AddTransactionScreenState extends State<AddTransactionScreen> {
  final _amountCtrl = TextEditingController();
  final _descrCtrl  = TextEditingController();
  final _dateCtrl   = TextEditingController();

  bool _isRecurring = false;
  bool _loading     = false;
  String? _error;

  List<Map<String, dynamic>> _categories = [];
  int? _selectedCategoryId;

  List<Map<String, dynamic>> _accounts = [];
  int? _selectedAccountId;

  @override
  void initState() {
    super.initState();
    _dateCtrl.text = DateTime.now().toIso8601String().split('T').first;
    _loadCategories();
    _loadAccountsWithTypes();
  }

  Future<List<Map<String, dynamic>>> _parseList(String body) async {
    final decoded = jsonDecode(body);
    if (decoded is List) return decoded.cast<Map<String, dynamic>>();
    if (decoded is Map && decoded.containsKey(r'$values')) {
      return (decoded[r'$values'] as List).cast<Map<String, dynamic>>();
    }
    throw Exception('Nieoczekiwany format odpowiedzi');
  }

  Future<void> _loadCategories() async {
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    final kind  = widget.type == 'income' ? 'Income' : 'Expense';
    final uri   = Uri.parse(
      '${ApiService().baseUrl}/api/Category?userId=${widget.userId}&type=$kind',
    );
    final resp = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (resp.statusCode == 200) {
      final list = await _parseList(resp.body);
      setState(() {
        _categories = list;
        // jeśli to pierwsze załadowanie, weź pierwszy
        if (_selectedCategoryId == null && list.isNotEmpty) {
          _selectedCategoryId = list.first['id'] as int;
        }
      });
    } else {
      setState(() => _error = 'Błąd ładowania kategorii (${resp.statusCode})');
    }
  }

  Future<void> _addCategoryDialog() async {
    final nameCtrl = TextEditingController();
    final token    = Provider.of<AuthProvider>(context, listen: false).token!;
    final type     = widget.type == 'income' ? 'Income' : 'Expense';

    await showDialog<int>(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Nowa kategoria'),
        content: TextField(
          controller: nameCtrl,
          decoration: const InputDecoration(labelText: 'Nazwa kategorii'),
        ),
        actions: [
          TextButton(onPressed: () => Navigator.of(ctx).pop(), child: const Text('Anuluj')),
          ElevatedButton(
            onPressed: () async {
              final name = nameCtrl.text.trim();
              if (name.isEmpty) return;
              final uri = Uri.parse('${ApiService().baseUrl}/api/Category');
              final body = {'name': name, 'type': type, 'userId': widget.userId};
              final r = await http.post(
                uri,
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer $token'
                },
                body: jsonEncode(body),
              );
              if (r.statusCode == 200 || r.statusCode == 201) {
                // spróbuj odczytać nowo utworzone ID z odpowiedzi
                final created = jsonDecode(r.body);
                final newId = created['id'] is int
                    ? created['id'] as int
                    : int.tryParse('${created['id']}') ?? -1;
                Navigator.of(ctx).pop(newId);
              } else {
                // zamknij dialog i pokaż błąd
                Navigator.of(ctx).pop();
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text('Nie udało się dodać kategorii (${r.statusCode})')),
                );
              }
            },
            child: const Text('Zapisz'),
          ),
        ],
      ),
    ).then((newId) async {
      if (newId != null && newId > 0) {
        // przeładuj listę i ustaw nowo dodaną kategorię jako wybraną
        await _loadCategories();
        setState(() {
          _selectedCategoryId = newId;
        });
      }
    });
  }

  Future<void> _loadAccountsWithTypes() async {
    final token = Provider.of<AuthProvider>(context, listen: false).token!;

    // najpierw typy kont
    final rTypes = await http.get(
      Uri.parse('${ApiService().baseUrl}/api/AccountType'),
      headers: {'Authorization': 'Bearer $token'},
    );
    final typeList = rTypes.statusCode == 200
        ? await _parseList(rTypes.body)
        : <Map<String, dynamic>>[];
    final typeMap = {
      for (var t in typeList) t['id'] as int: t['name']?.toString() ?? 'Konto'
    };

    // potem konta
    final rAcc = await http.get(
      Uri.parse('${ApiService().baseUrl}/api/Account'),
      headers: {'Authorization': 'Bearer $token'},
    );
    if (rAcc.statusCode == 200) {
      final rawAccounts = await _parseList(rAcc.body);
      final built = rawAccounts.map((a) {
        final id     = a['id'] as int;
        final typeId = a['accountTypesId'] as int;
        // wyświetlamy TYLKO nazwę typu konta, bez numeru
        final name   = typeMap[typeId]!;
        return {'id': id, 'displayName': name};
      }).toList();
      setState(() {
        _accounts = built;
        if (_selectedAccountId == null && built.isNotEmpty) {
          _selectedAccountId = built.first['id'] as int;
        }
      });
    } else {
      setState(() => _error = 'Błąd ładowania kont (${rAcc.statusCode})');
    }
  }

  Future<void> _submit() async {
    setState(() { _loading = true; _error = null; });
    final amt = double.tryParse(_amountCtrl.text.trim());
    if (amt == null || _selectedCategoryId == null || _selectedAccountId == null) {
      setState(() {
        _error = 'Podaj kwotę, kategorię i konto';
        _loading = false;
      });
      return;
    }
    final token    = Provider.of<AuthProvider>(context, listen: false).token!;
    final endpoint = widget.type == 'income' ? 'Income' : 'Expense';
    final uri      = Uri.parse('${ApiService().baseUrl}/api/$endpoint');
    final body     = {
      'amount': amt,
      'description': _descrCtrl.text.trim(),
      'date': _dateCtrl.text,
      'userId': widget.userId,
      'budgetId': widget.budgetId,
      'isRecurring': _isRecurring,
      'recurrenceInterval': null,
      'nextOccurrenceDate': null,
      'categoryId': _selectedCategoryId,
      'accountId': _selectedAccountId,
      'invoiceId': 0,
    };
    final resp = await http.post(
      uri,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token'
      },
      body: jsonEncode(body),
    );
    if (resp.statusCode == 200 || resp.statusCode == 201) {
      Navigator.of(context).pop(true);
    } else {
      setState(() => _error = 'Błąd zapisu (${resp.statusCode})');
    }
    setState(() => _loading = false);
  }

  @override
  Widget build(BuildContext ctx) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          widget.type == 'income' ? 'Dodaj przychód' : 'Dodaj wydatek',
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            TextField(
              controller: _amountCtrl,
              decoration: const InputDecoration(labelText: 'Kwota'),
              keyboardType: TextInputType.number,
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _descrCtrl,
              decoration: const InputDecoration(labelText: 'Opis (opcjonalny)'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _dateCtrl,
              readOnly: true,
              decoration: const InputDecoration(labelText: 'Data'),
              onTap: () async {
                final d = await showDatePicker(
                  context: context,
                  initialDate: DateTime.parse(_dateCtrl.text),
                  firstDate: DateTime(2020),
                  lastDate: DateTime(2100),
                );
                if (d != null) {
                  _dateCtrl.text = d.toIso8601String().split('T').first;
                }
              },
            ),
            const SizedBox(height: 24),
            // dropdown kategorii
            DropdownButtonFormField<int>(
              decoration: const InputDecoration(labelText: 'Kategoria'),
              items: _categories.map((c) {
                return DropdownMenuItem<int>(
                  value: c['id'] as int,
                  child: Text(c['name']?.toString() ?? '—'),
                );
              }).toList(),
              value: _selectedCategoryId,
              onChanged: (v) => setState(() => _selectedCategoryId = v),
            ),
            Align(
              alignment: Alignment.centerLeft,
              child: TextButton.icon(
                icon: const Icon(Icons.category_outlined),
                label: const Text('+ Dodaj kategorię'),
                style: TextButton.styleFrom(foregroundColor: Colors.orange),
                onPressed: _addCategoryDialog,
              ),
            ),
            const SizedBox(height: 24),
            // dropdown kont
            if (_accounts.isEmpty) ...[
              const Text(
                'Nie masz jeszcze kont. Dodaj konto w zakładce Konta.',
                style: TextStyle(color: Colors.red),
              ),
            ] else ...[
              DropdownButtonFormField<int>(
                decoration: const InputDecoration(labelText: 'Konto'),
                items: _accounts.map((a) {
                  return DropdownMenuItem<int>(
                    value: a['id'] as int,
                    child: Text(a['displayName'] as String),
                  );
                }).toList(),
                value: _selectedAccountId,
                onChanged: (v) => setState(() => _selectedAccountId = v),
              ),
            ],
            const SizedBox(height: 24),
            SwitchListTile(
              title: const Text('Cykliczna transakcja'),
              value: _isRecurring,
              onChanged: (v) => setState(() => _isRecurring = v),
            ),
            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: _loading ? null : _submit,
              child: _loading
                  ? const SizedBox(
                width: 24, height: 24,
                child: CircularProgressIndicator(strokeWidth: 2),
              )
                  : const Text('Dodaj'),
            ),
            if (_error != null) ...[
              const SizedBox(height: 12),
              Text(_error!, style: const TextStyle(color: Colors.red)),
            ],
          ],
        ),
      ),
    );
  }
}
