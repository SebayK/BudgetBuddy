// lib/screens/overview_screen.dart

import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../services/api_service.dart';
import '../providers/auth_provider.dart';

class OverviewScreen extends StatefulWidget {
  const OverviewScreen({Key? key}) : super(key: key);

  @override
  State<OverviewScreen> createState() => _OverviewScreenState();
}

class _OverviewScreenState extends State<OverviewScreen> {
  final ApiService _api = ApiService();

  // meta typów kont: id → { emoji, color }
  final Map<int, Map<String, dynamic>> _localMeta = {};

  // nazwy typów kont: id → name
  final Map<int, String> _typeNames = {};

  // lista kont z saldami i powiązanym typem
  List<Map<String, dynamic>> _accounts = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _initAll();
  }

  Future<void> _initAll() async {
    await _loadLocalMeta();
    await _loadAccountTypes();
    await _loadAccountsAndComputedBalances();
  }

  Future<void> _loadLocalMeta() async {
    final prefs = await SharedPreferences.getInstance();
    final raw = prefs.getString('accountTypeMeta');
    if (raw != null) {
      final decoded = jsonDecode(raw) as Map<String, dynamic>;
      _localMeta.clear();
      decoded.forEach((key, value) {
        _localMeta[int.parse(key)] = {
          'emoji': value['emoji'],
          'color': value['color'],
        };
      });
    }
  }

  Future<void> _loadAccountTypes() async {
    final token = Provider.of<AuthProvider>(context, listen: false).token;
    if (token == null) return;
    final types = await _api.getAccountTypes(token);
    _typeNames.clear();
    for (var t in types) {
      final id = t['id'] as int;
      _typeNames[id] = t['name'] as String;
    }
  }

  Future<void> _loadAccountsAndComputedBalances() async {
    setState(() => _isLoading = true);
    try {
      final token = Provider.of<AuthProvider>(context, listen: false).token;
      if (token == null) throw Exception('Brak tokena');

      final rawAccounts = await _api.getAccounts(token);
      final incomes  = await _api.getTransactions(token, 'Income');
      final expenses = await _api.getTransactions(token, 'Expense');

      final Map<int, double> sumIncome  = {};
      final Map<int, double> sumExpense = {};

      for (var tx in incomes) {
        final accId = int.tryParse('${tx['accountId']}') ?? -1;
        final amt   = (tx['amount'] as num).toDouble();
        sumIncome[accId] = (sumIncome[accId] ?? 0) + amt;
      }
      for (var tx in expenses) {
        final accId = int.tryParse('${tx['accountId']}') ?? -1;
        final amt   = (tx['amount'] as num).toDouble();
        sumExpense[accId] = (sumExpense[accId] ?? 0) + amt;
      }

      final List<Map<String, dynamic>> withBal = rawAccounts.map((acc) {
        final id        = acc['id'] as int;
        final typeId    = acc['accountTypesId'] as int;
        final currency  = acc['currencyId'] as String? ?? '';
        final incomeSum = sumIncome[id] ?? 0.0;
        final expSum    = sumExpense[id] ?? 0.0;
        final balance   = incomeSum - expSum;

        final meta  = _localMeta[typeId];
        final emoji = meta != null ? meta['emoji'] as String : '🏦';
        final color = meta != null
            ? Color(meta['color'] as int)
            : Colors.grey;

        // tylko nazwa typu, bez numeru
        final displayName = _typeNames[typeId] ?? 'Konto';

        return {
          'id': id,
          'emoji': emoji,
          'color': color,
          'displayName': displayName,
          'balance': balance,
          'currency': currency,
        };
      }).toList();

      setState(() => _accounts = withBal);
    } catch (e) {
      debugPrint('Błąd ładowania sald kont: $e');
    }
    setState(() => _isLoading = false);
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final name  = Provider.of<AuthProvider>(context).userFirstName ?? '';

    return Scaffold(
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                if (name.isNotEmpty) ...[
                  Text(
                    'Witaj $name!',
                    style: theme.textTheme.headlineSmall
                        ?.copyWith(fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 24),
                ],
                Text('Przegląd', style: theme.textTheme.headlineSmall),
                const SizedBox(height: 20),

                Card(
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(16)),
                  color: theme.colorScheme.surfaceVariant,
                  child: Padding(
                    padding: const EdgeInsets.all(24),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'Salda kont',
                          style: theme.textTheme.titleMedium
                              ?.copyWith(fontWeight: FontWeight.w600),
                        ),
                        const SizedBox(height: 16),
                        if (_isLoading)
                          const Center(child: CircularProgressIndicator())
                        else if (_accounts.isEmpty)
                          const Text('Brak kont do wyświetlenia.')
                        else
                          Column(
                            children: _accounts.map((acc) {
                              return Padding(
                                padding:
                                const EdgeInsets.symmetric(vertical: 6),
                                child: Row(
                                  children: [
                                    CircleAvatar(
                                      backgroundColor: acc['color'] as Color,
                                      child: Text(acc['emoji'] as String),
                                    ),
                                    const SizedBox(width: 12),
                                    Expanded(
                                      child: Text(
                                        acc['displayName'] as String,
                                        style: theme.textTheme.bodyLarge,
                                      ),
                                    ),
                                    Text(
                                      '${(acc['balance'] as double).toStringAsFixed(2)} ${acc['currency']}',
                                      style: theme.textTheme.bodyLarge,
                                    ),
                                  ],
                                ),
                              );
                            }).toList(),
                          ),
                      ],
                    ),
                  ),
                ),

                const SizedBox(height: 24),
                _buildTransactionSection(theme, 'Planowane'),
                const SizedBox(height: 16),
                _buildTransactionSection(theme, 'Oczekujące'),
                const SizedBox(height: 50),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildTransactionSection(ThemeData theme, String title) {
    return Card(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      color: theme.colorScheme.surfaceVariant,
      child: SizedBox(
        height: 140,
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(title,
                  style: theme.textTheme.titleMedium
                      ?.copyWith(fontWeight: FontWeight.w600)),
              const Spacer(),
              Center(
                child: Text(
                  'Brak ${title.toLowerCase()} transakcji',
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
    );
  }
}
