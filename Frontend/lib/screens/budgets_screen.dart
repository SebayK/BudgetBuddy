// lib/screens/budgets_screen.dart

import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:emoji_picker_flutter/emoji_picker_flutter.dart';
import 'package:flutter_colorpicker/flutter_colorpicker.dart';

import '../services/api_service.dart';
import '../providers/auth_provider.dart';

class BudgetsScreen extends StatefulWidget {
  const BudgetsScreen({Key? key}) : super(key: key);

  @override
  State<BudgetsScreen> createState() => _BudgetsScreenState();
}

class _BudgetsScreenState extends State<BudgetsScreen> {
  final ApiService _api = ApiService();

  List<Map<String, dynamic>> _accounts      = [];
  List<Map<String, dynamic>> _sharedBudgets = [];
  List<Map<String, dynamic>> _accountTypes  = [];

  bool _isLoadingAccounts = true;
  bool _isLoadingShared   = true;
  bool _isLoadingTypes    = true;

  final Map<int, Map<String, dynamic>> _localMeta = {};

  // Dla dialogu „Dodaj konto”
  int?    _selectedAccountTypeId;
  String? _selectedCurrency;
  final  _accountNumberCtrl = TextEditingController();
  String? _errorAddAccount;

  @override
  void initState() {
    super.initState();
    _loadLocalMeta();
    _loadAccountTypes();
    _loadAccounts();
    _loadSharedBudgets();
  }

  Future<void> _loadLocalMeta() async {
    final prefs = await SharedPreferences.getInstance();
    final raw   = prefs.getString('accountTypeMeta');
    if (raw != null) {
      final Map<String, dynamic> decoded = jsonDecode(raw);
      setState(() {
        _localMeta.clear();
        decoded.forEach((key, value) {
          _localMeta[int.parse(key)] = {
            'emoji': value['emoji'],
            'color': value['color'],
          };
        });
      });
    }
  }

  Future<void> _saveLocalMeta() async {
    final prefs = await SharedPreferences.getInstance();
    final toSave = _localMeta.map<String, dynamic>(
          (k, v) => MapEntry(k.toString(), v),
    );
    await prefs.setString('accountTypeMeta', jsonEncode(toSave));
  }

  Future<void> _loadAccountTypes() async {
    setState(() => _isLoadingTypes = true);
    try {
      final token = Provider.of<AuthProvider>(context, listen: false).token!;
      final types = await _api.getAccountTypes(token);
      setState(() {
        _accountTypes = types;
        for (var t in types) {
          final id = t['id'] as int;
          if (t.containsKey('emoji') && t.containsKey('colorValue')) {
            _localMeta[id] = {
              'emoji': t['emoji'] as String,
              'color': t['colorValue'] as int,
            };
          }
        }
      });
      await _saveLocalMeta();
    } catch (e) {
      debugPrint('Błąd ładowania typów kont: $e');
    }
    setState(() => _isLoadingTypes = false);
  }

  Future<void> _loadAccounts() async {
    setState(() => _isLoadingAccounts = true);
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    try {
      final raw = await _api.getAccounts(token);
      final withBal = await Future.wait(raw.map((acc) async {
        final id = acc['id'] as int;
        double bal = 0;
        try {
          bal = await _api.getAccountBalance(token: token, accountId: id);
        } catch (_) {}
        return {
          ...acc,
          'balance': bal,
        };
      }));
      setState(() => _accounts = withBal);
    } catch (e) {
      debugPrint('Błąd ładowania kont: $e');
    }
    setState(() => _isLoadingAccounts = false);
  }

  Future<void> _loadSharedBudgets() async {
    setState(() => _isLoadingShared = true);
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    try {
      final sb = await _api.getSharedBudgets(token);
      setState(() => _sharedBudgets = sb);
    } catch (e) {
      debugPrint('Błąd ładowania budżetów współdzielonych: $e');
    }
    setState(() => _isLoadingShared = false);
  }

  Future<void> _refreshAll() async {
    await Future.wait([
      _loadAccounts(),
      _loadSharedBudgets(),
    ]);
  }

  Future<int?> _showAddAccountTypeDialog() async {
    final nameCtrl = TextEditingController();
    String pickedEmoji = '🏦';
    Color pickedColor = Colors.blue;
    String? error;

    return showDialog<int>(
      context: context,
      builder: (ctx) => StatefulBuilder(
        builder: (ctx2, setSt) => AlertDialog(
          title: const Text('Nowy typ konta'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: nameCtrl,
                decoration: const InputDecoration(labelText: 'Nazwa'),
              ),
              const SizedBox(height: 12),
              Row(children: [
                const Text('Emoji:'),
                const SizedBox(width: 8),
                GestureDetector(
                  onTap: () {
                    showModalBottomSheet(
                      context: context,
                      builder: (_) => EmojiPicker(
                        onEmojiSelected: (_, emoji) {
                          setSt(() => pickedEmoji = emoji.emoji);
                          Navigator.pop(context);
                        },
                        config: const Config(columns: 7),
                      ),
                    );
                  },
                  child: Text(pickedEmoji,
                      style: const TextStyle(fontSize: 24)),
                ),
              ]),
              const SizedBox(height: 12),
              Row(children: [
                const Text('Kolor:'),
                const SizedBox(width: 8),
                GestureDetector(
                  onTap: () {
                    showDialog(
                      context: context,
                      builder: (_) => AlertDialog(
                        title: const Text('Wybierz kolor'),
                        content: BlockPicker(
                          pickerColor: pickedColor,
                          onColorChanged: (col) =>
                              setSt(() => pickedColor = col),
                        ),
                        actions: [
                          TextButton(
                              onPressed: () => Navigator.pop(context),
                              child: const Text('OK')),
                        ],
                      ),
                    );
                  },
                  child: CircleAvatar(
                      backgroundColor: pickedColor, radius: 14),
                ),
              ]),
              if (error != null) ...[
                const SizedBox(height: 8),
                Text(error!, style: const TextStyle(color: Colors.red)),
              ],
            ],
          ),
          actions: [
            TextButton(
                onPressed: () => Navigator.of(ctx).pop(),
                child: const Text('Anuluj')),
            ElevatedButton(
              onPressed: () async {
                final name = nameCtrl.text.trim();
                if (name.isEmpty) {
                  setSt(() => error = 'Podaj nazwę');
                  return;
                }
                final token =
                Provider.of<AuthProvider>(context, listen: false).token!;
                final newId = await _api.createAccountType(
                  token: token,
                  name: name,
                  emoji: pickedEmoji,
                  colorValue: pickedColor.value,
                );
                if (newId != null && newId > 0) {
                  setState(() {
                    _accountTypes.add({'id': newId, 'name': name});
                    _localMeta[newId] = {
                      'emoji': pickedEmoji,
                      'color': pickedColor.value,
                    };
                  });
                  await _saveLocalMeta();
                  Navigator.of(ctx).pop(newId);
                } else {
                  setSt(() => error = 'Nie udało się dodać typu');
                }
              },
              child: const Text('Dodaj'),
            ),
          ],
        ),
      ),
    );
  }

  void _showAddAccountDialog() {
    _selectedAccountTypeId = null;
    _selectedCurrency = null;
    _accountNumberCtrl.clear();
    _errorAddAccount = null;

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => StatefulBuilder(
        builder: (ctx2, setSt) => AlertDialog(
          title: const Text('Dodaj konto'),
          content: SingleChildScrollView(
            child: Column(mainAxisSize: MainAxisSize.min, children: [
              if (_isLoadingTypes)
                const Center(child: CircularProgressIndicator()),
              DropdownButtonFormField<int>(
                decoration: const InputDecoration(labelText: 'Typ konta'),
                items: _accountTypes.map((t) {
                  final id = t['id'] as int;
                  final name = t['name'] as String;
                  final meta = _localMeta[id];
                  final emoji = meta != null
                      ? meta['emoji'] as String
                      : '🏦';
                  return DropdownMenuItem<int>(
                    value: id,
                    child: Row(children: [
                      Text(emoji),
                      const SizedBox(width: 8),
                      Text(name),
                    ]),
                  );
                }).toList(),
                value: _selectedAccountTypeId,
                onChanged: (v) => setSt(() => _selectedAccountTypeId = v),
              ),
              Align(
                alignment: Alignment.centerRight,
                child: TextButton.icon(
                  onPressed: () async {
                    final newId = await _showAddAccountTypeDialog();
                    if (newId != null) {
                      setSt(() => _selectedAccountTypeId = newId);
                    }
                  },
                  icon: const Icon(Icons.add_circle_outline),
                  label: const Text('Nowy typ'),
                ),
              ),
              const SizedBox(height: 8),
              TextField(
                controller: _accountNumberCtrl,
                decoration:
                const InputDecoration(labelText: 'Numer konta'),
                keyboardType: TextInputType.number,
              ),
              TextField(
                decoration: const InputDecoration(labelText: 'Waluta'),
                onChanged: (v) => setSt(() => _selectedCurrency = v),
              ),
              if (_errorAddAccount != null) ...[
                const SizedBox(height: 8),
                Text(_errorAddAccount!, style: const TextStyle(color: Colors.red)),
              ],
            ]),
          ),
          actions: [
            TextButton(
                onPressed: () => Navigator.of(ctx).pop(),
                child: const Text('Anuluj')),
            ElevatedButton(
              onPressed: () async {
                final auth =
                Provider.of<AuthProvider>(context, listen: false);
                final token = auth.token!;
                final userId = auth.userId!;
                final typeId = _selectedAccountTypeId;
                final num =
                int.tryParse(_accountNumberCtrl.text.trim());
                final currency = _selectedCurrency?.trim();
                if (typeId == null ||
                    num == null ||
                    currency == null ||
                    currency.isEmpty) {
                  return setSt(
                          () => _errorAddAccount = 'Uzupełnij wszystkie pola');
                }
                final ok = await _api.createAccount(
                  token: token,
                  userId: userId,
                  accountTypeId: typeId,
                  currencyId: currency,
                  accountNumber: num,
                );
                if (ok) {
                  Navigator.of(ctx).pop();
                  await _loadAccounts();
                } else {
                  setSt(() =>
                  _errorAddAccount = 'Nie udało się dodać konta');
                }
              },
              child: const Text('Dodaj'),
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      backgroundColor: const Color(0xFFFDF0CB),
      body: RefreshIndicator(
        onRefresh: _refreshAll,
        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            Text('Twoje konta',
                style: theme.textTheme.headlineSmall
                    ?.copyWith(fontWeight: FontWeight.bold)),
            const SizedBox(height: 12),
            if (_isLoadingAccounts)
              const Center(child: CircularProgressIndicator())
            else if (_accounts.isEmpty)
              const Text('Brak kont.')
            else
              ..._accounts.map((acc) {
                final typeId = acc['accountTypesId'] as int;
                final meta = _localMeta[typeId];
                final color = meta != null
                    ? Color(meta['color'] as int)
                    : Colors.grey;
                final emoji = meta != null
                    ? meta['emoji'] as String
                    : '🏦';
                final type = _accountTypes.firstWhere(
                        (t) => t['id'] == typeId,
                    orElse: () => {'name': '—'});
                final name = type['name'] as String;
                final curr = acc['currencyId'] as String? ?? '';
                final bal = acc['balance'] as double? ?? 0.0;

                return Card(
                  color: color,
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12)),
                  child: ListTile(
                    leading: CircleAvatar(
                        backgroundColor: color, child: Text(emoji)),
                    title: Text(name,
                        style: const TextStyle(color: Colors.white)),
                    subtitle: Text(
                      'Saldo: ${bal.toStringAsFixed(2)} $curr',
                      style: const TextStyle(color: Colors.white70),
                    ),
                    trailing: IconButton(
                      icon: const Icon(Icons.delete_outline,
                          color: Colors.white),
                      onPressed: () async {
                        final token =
                        Provider.of<AuthProvider>(context, listen: false)
                            .token!;
                        final ok = await _api.deleteAccount(
                          token: token,
                          accountId: acc['id'] as int,
                        );
                        if (ok) await _loadAccounts();
                      },
                    ),
                  ),
                );
              }).toList(),
            const SizedBox(height: 24),
            Text('Konta współdzielone',
                style: theme.textTheme.headlineSmall
                    ?.copyWith(fontWeight: FontWeight.bold)),
            const SizedBox(height: 12),
            if (_isLoadingShared)
              const Center(child: CircularProgressIndicator())
            else if (_sharedBudgets.isEmpty)
              const Text('Brak współdzielonych budżetów.')
            else
              ..._sharedBudgets.map((sb) => Card(
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12)),
                child: ListTile(
                  title: Text(sb['name'] ?? '—'),
                  subtitle:
                  Text('Właściciel: ${sb['ownerUserId']}'),
                ),
              )),
          ],
        ),
      ),
      floatingActionButtonLocation:
      FloatingActionButtonLocation.centerFloat,
      floatingActionButton: FloatingActionButton.extended(
        onPressed: _showAddAccountDialog,
        icon: const Icon(Icons.add),
        label: const Text('Dodaj konto'),
        backgroundColor: Colors.amber,
        foregroundColor: Colors.black,
      ),
    );
  }
}
