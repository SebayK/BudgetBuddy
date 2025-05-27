import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class AddExpenseScreen extends StatefulWidget {
  final String token;
  final VoidCallback onBack;

  const AddExpenseScreen({Key? key, required this.token, required this.onBack}) : super(key: key);

  @override
  State<AddExpenseScreen> createState() => _AddExpenseScreenState();
}

class _AddExpenseScreenState extends State<AddExpenseScreen> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _amountController = TextEditingController();
  String? _error;
  bool _isLoading = false;
  bool _success = false;

  Future<void> _addExpense() async {
    setState(() {
      _isLoading = true;
      _error = null;
      _success = false;
    });

    try {
      final response = await http.post(
        Uri.parse('http://localhost:5000/api/Expense'),
        headers: {
          'Authorization': 'Bearer ${widget.token}',
          'Content-Type': 'application/json',
        },
       body: jsonEncode({
          'name': _nameController.text,
          'amount': double.tryParse(_amountController.text) ?? 0.0,
          'date': DateTime.now().toIso8601String(),
          'categoryId': 1,
          'UserId': '4d8b6501-7897-474d-9ebc-d5e4be7ef99e', // <- z bazy!
          'invoiceId': 0,
      }),

      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        setState(() {
          _success = true;
          _nameController.clear();
          _amountController.clear();
        });
      } else {
        setState(() => _error = 'Błąd dodawania wydatku: ${response.body}');
      }
    } catch (e) {
      setState(() => _error = 'Błąd połączenia: $e');
    } finally {
      setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Dodaj wydatek'),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: widget.onBack,
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              if (_success)
                const Padding(
                  padding: EdgeInsets.only(bottom: 16.0),
                  child: Text(
                    'Wydatek dodany!',
                    style: TextStyle(color: Colors.green, fontWeight: FontWeight.bold),
                  ),
                ),
              TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(labelText: 'Nazwa wydatku'),
                validator: (v) => v == null || v.isEmpty ? 'Wpisz nazwę' : null,
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: _amountController,
                decoration: const InputDecoration(labelText: 'Kwota'),
                keyboardType: TextInputType.numberWithOptions(decimal: true),
                validator: (v) {
                  if (v == null || v.isEmpty) return 'Wpisz kwotę';
                  final amount = double.tryParse(v.replaceAll(',', '.'));
                  if (amount == null || amount <= 0) return 'Podaj poprawną kwotę';
                  return null;
                },
              ),
              const SizedBox(height: 24),
              ElevatedButton(
                onPressed: _isLoading
                    ? null
                    : () {
                        if (_formKey.currentState?.validate() ?? false) {
                          _addExpense();
                        }
                      },
                child: _isLoading
                    ? const CircularProgressIndicator()
                    : const Text('Dodaj wydatek'),
              ),
              if (_error != null) ...[
                const SizedBox(height: 16),
                Text(_error!, style: const TextStyle(color: Colors.red)),
              ],
            ],
          ),
        ),
      ),
    );
  }
}
