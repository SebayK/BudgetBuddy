// lib/screens/transactions_screen.dart

import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';

import '../providers/auth_provider.dart';
import '../services/api_service.dart';
import 'add_transaction_screen.dart';

class TransactionsScreen extends StatefulWidget {
  final String userId;
  final int budgetId;

  const TransactionsScreen({
    Key? key,
    required this.userId,
    required this.budgetId,
  }) : super(key: key);

  @override
  State<TransactionsScreen> createState() => _TransactionsScreenState();
}

class _TransactionsScreenState extends State<TransactionsScreen> {
  Map<int, String> _accountNames      = {};
  Map<int, String> _incomeCategories  = {};
  Map<int, String> _expenseCategories = {};

  @override
  void initState() {
    super.initState();
    _loadAccountNames();
    _loadCategoriesByType();
  }

  Future<List<Map<String, dynamic>>> _normalize(dynamic json) async {
    if (json is List) return json.cast<Map<String, dynamic>>();
    if (json is Map) {
      for (var v in json.values) {
        if (v is List) return v.cast<Map<String, dynamic>>();
      }
      return [json.cast<String, dynamic>()];
    }
    return [];
  }

  /// 1) Ładujemy nazwy kont (TUTAJ POKAZUJEMY TYLKO TYP, NIE NUMER)
  Future<void> _loadAccountNames() async {
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    // pobierz typy kont
    final rTypes = await http.get(
      Uri.parse('${ApiService().baseUrl}/api/AccountType'),
      headers: {'Authorization': 'Bearer $token'},
    );
    final types = rTypes.statusCode == 200
        ? await _normalize(jsonDecode(rTypes.body))
        : <Map<String, dynamic>>[];
    final typeMap = {for (var t in types) t['id'] as int: t['name'].toString()};

    // pobierz wszystkie konta
    final rAcc = await http.get(
      Uri.parse('${ApiService().baseUrl}/api/Account'),
      headers: {'Authorization': 'Bearer $token'},
    );
    if (rAcc.statusCode == 200) {
      final accs = await _normalize(jsonDecode(rAcc.body));
      setState(() {
        // zamiast "Typ (#nr)" robimy tylko "Typ"
        _accountNames = {
          for (var a in accs)
            a['id'] as int: typeMap[a['accountTypesId'] as int] ?? 'Konto'
        };
      });
    }
  }

  /// 2) Ładujemy kategorie z podziałem na Income i Expense
  Future<void> _loadCategoriesByType() async {
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    // Income
    final rInc = await http.get(
      Uri.parse(
          '${ApiService().baseUrl}/api/Category?userId=${widget.userId}&type=Income'
      ),
      headers: {'Authorization': 'Bearer $token'},
    );
    if (rInc.statusCode == 200) {
      final inc = await _normalize(jsonDecode(rInc.body));
      setState(() {
        _incomeCategories = {
          for (var c in inc) c['id'] as int: c['name'].toString()
        };
      });
    }
    // Expense
    final rExp = await http.get(
      Uri.parse(
          '${ApiService().baseUrl}/api/Category?userId=${widget.userId}&type=Expense'
      ),
      headers: {'Authorization': 'Bearer $token'},
    );
    if (rExp.statusCode == 200) {
      final exp = await _normalize(jsonDecode(rExp.body));
      setState(() {
        _expenseCategories = {
          for (var c in exp) c['id'] as int: c['name'].toString()
        };
      });
    }
  }

  /// 3) Pobieramy transakcje już przefiltrowane po userId i budgetId
  Future<List<Map<String, dynamic>>> _fetchTransactions(
      String endpoint) async {
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    final uri = Uri.parse(
        '${ApiService().baseUrl}/api/$endpoint'
            '?userId=${widget.userId}&budgetId=${widget.budgetId}'
    );
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode != 200) {
      throw Exception('Błąd pobierania $endpoint: ${r.statusCode}');
    }
    return await _normalize(jsonDecode(r.body));
  }

  Widget _buildSection(
      String title,
      Future<List<Map<String, dynamic>>> futureTx,
      String type,
      ) {
    return FutureBuilder<List<Map<String, dynamic>>>(
      future: futureTx,
      builder: (ctx, snap) {
        if (snap.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        }
        if (snap.hasError) {
          return Center(child: Text('Błąd: ${snap.error}'));
        }
        final items = snap.data!;
        final catMap = type == 'income'
            ? _incomeCategories
            : _expenseCategories;

        return Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // nagłówek + przycisk
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(title,
                    style: const TextStyle(
                        fontSize: 20, fontWeight: FontWeight.bold)),
                TextButton.icon(
                  icon: const Icon(Icons.add),
                  label: Text(
                      type == 'income' ? 'Dodaj przychód' : 'Dodaj wydatek'
                  ),
                  onPressed: () => Navigator.of(context)
                      .push<bool>(MaterialPageRoute(
                    builder: (_) => AddTransactionScreen(
                      userId: widget.userId,
                      budgetId: widget.budgetId,
                      type: type,
                    ),
                  ))
                      .then((added) {
                    if (added == true) setState(() {});
                  }),
                ),
              ],
            ),

            const SizedBox(height: 8),
            if (items.isEmpty)
              const Text('Brak danych', style: TextStyle(color: Colors.grey))
            else
              ...items.map((t) {
                final rawCat = t['categoryId'] ?? t['categoryID'];
                final catId   = rawCat is int
                    ? rawCat
                    : int.tryParse('$rawCat') ?? -1;

                final rawAcc = t['accountId'] ?? t['accountID'];
                final accId   = rawAcc is int
                    ? rawAcc
                    : int.tryParse('$rawAcc') ?? -1;

                final amt     = t['amount'] ?? 0;
                final sign    = type == 'income' ? '+' : '-';
                final color   = type == 'income' ? Colors.green : Colors.red;
                final accName = _accountNames[accId] ?? 'Konto';
                final catName = catMap[catId]      ?? '—';

                return ListTile(
                  leading: Icon(
                    type == 'income'
                        ? Icons.add_circle_outline
                        : Icons.remove_circle_outline,
                    color: color,
                  ),
                  title: Text(
                    '$sign$amt PLN',
                    style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.w600,
                        color: color
                    ),
                  ),
                  subtitle: Text('$accName • $catName'),
                );
              }).toList(),

            const SizedBox(height: 24),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      padding: const EdgeInsets.all(16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildSection('Przychody', _fetchTransactions('Income'), 'income'),
          _buildSection('Wydatki', _fetchTransactions('Expense'), 'expense'),
        ],
      ),
    );
  }
}
