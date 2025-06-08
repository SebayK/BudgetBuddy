import 'package:flutter/material.dart';

class AddTransactionScreen extends StatefulWidget {
  final Function()? onTransactionAdded;

  const AddTransactionScreen({Key? key, this.onTransactionAdded}) : super(key: key);

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
      // Tu podmień na swój endpoint!
      final uri = Uri.parse('https://TWOJE_API/api/transactions'); // lub expenses, jeśli masz osobno
      final response = await
      // Jeśli używasz http:
      // http.post(uri,
      //   headers: {'Content-Type': 'application/json'},
      //   body: jsonEncode({
      //     'name': _name,
      //     'amount': _amount,
      //     'type': _type,
      //   }),
      // );
      // W tym miejscu możesz dopasować do swojego API
      // --- przykład poniżej w komentarzu!

      if (response.statusCode == 201 || response.statusCode == 200) {
        widget.onTransactionAdded?.call();
        Navigator.of(context).pop();
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
                items: ['Wydatek', 'Dochód'].map((e) =>
                    DropdownMenuItem(value: e, child: Text(e))).toList(),
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
