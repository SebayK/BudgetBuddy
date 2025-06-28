import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';

class AddCategoryScreen extends StatefulWidget {
  final String initialType; // 'income' lub 'expense'
  const AddCategoryScreen({Key? key, required this.initialType}) : super(key: key);

  @override
  State<AddCategoryScreen> createState() => _AddCategoryScreenState();
}

class _AddCategoryScreenState extends State<AddCategoryScreen> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _nameCtrl = TextEditingController();
  late String _type;
  bool _isLoading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    _type = widget.initialType; // ustawiamy domyślny typ z przekazanego argumentu
  }

  Future<void> _saveCategory() async {
    if (!_formKey.currentState!.validate()) return;
    setState(() {
      _isLoading = true;
      _error = null;
    });

    final token = Provider.of<AuthProvider>(context, listen: false).token;
    final userId = Provider.of<AuthProvider>(context, listen: false).userId;

    try {
      final response = await http.post(
        Uri.parse('http://localhost:5000/api/Category'),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode({
          'name': _nameCtrl.text,
          'type': _type,
          'userId': userId,
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        Navigator.of(context).pop();
      } else {
        setState(() => _error = 'Błąd: ${response.body}');
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
      appBar: AppBar(title: const Text('Nowa kategoria')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                controller: _nameCtrl,
                decoration: const InputDecoration(labelText: 'Nazwa'),
                validator: (v) => v!.isEmpty ? 'Podaj nazwę' : null,
              ),
              const SizedBox(height: 12),
              DropdownButtonFormField<String>(
                value: _type,
                onChanged: (v) => setState(() => _type = v!),
                items: const [
                  DropdownMenuItem(value: 'income', child: Text('Przychód')),
                  DropdownMenuItem(value: 'expense', child: Text('Wydatek')),
                ],
                decoration: const InputDecoration(labelText: 'Typ'),
              ),
              const SizedBox(height: 24),
              ElevatedButton(
                onPressed: _isLoading ? null : _saveCategory,
                child: _isLoading
                    ? const SizedBox(
                  width: 16,
                  height: 16,
                  child: CircularProgressIndicator(strokeWidth: 2),
                )
                    : const Text('Zapisz'),
              ),
              if (_error != null) ...[
                const SizedBox(height: 12),
                Text(_error!, style: const TextStyle(color: Colors.red)),
              ],
            ],
          ),
        ),
      ),
    );
  }
}
