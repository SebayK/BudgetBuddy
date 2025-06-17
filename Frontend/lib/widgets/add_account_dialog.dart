import 'package:flutter/material.dart';

class AddAccountDialog extends StatefulWidget {
  final void Function(Map<String, dynamic>) onSubmit;

  const AddAccountDialog({super.key, required this.onSubmit});

  @override
  State<AddAccountDialog> createState() => _AddAccountDialogState();
}

class _AddAccountDialogState extends State<AddAccountDialog> {
  final _formKey = GlobalKey<FormState>();

  int accountNumber = 0;
  int accountTypesId = 1;
  String currencyId = 'PLN';

  @override
  Widget build(BuildContext context) {
    return Dialog(
      insetPadding: const EdgeInsets.all(16),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      child: Container(
        padding: const EdgeInsets.all(20),
        child: Form(
          key: _formKey,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text('Dodaj konto', style: Theme.of(context).textTheme.titleLarge),
              const SizedBox(height: 16),
              TextFormField(
                decoration: const InputDecoration(labelText: 'Numer konta'),
                keyboardType: TextInputType.number,
                onChanged: (val) => accountNumber = int.tryParse(val) ?? 0,
                validator: (val) =>
                val == null || val.isEmpty ? 'Podaj numer konta' : null,
              ),
              const SizedBox(height: 12),
              DropdownButtonFormField<int>(
                decoration: const InputDecoration(labelText: 'Typ konta'),
                value: accountTypesId,
                onChanged: (val) => setState(() => accountTypesId = val ?? 1),
                items: const [
                  DropdownMenuItem(value: 1, child: Text('Oszczędnościowe')),
                  DropdownMenuItem(value: 2, child: Text('Bieżące')),
                ],
              ),
              const SizedBox(height: 12),
              DropdownButtonFormField<String>(
                decoration: const InputDecoration(labelText: 'Waluta'),
                value: currencyId,
                onChanged: (val) => setState(() => currencyId = val ?? 'PLN'),
                items: const [
                  DropdownMenuItem(value: 'PLN', child: Text('PLN')),
                  DropdownMenuItem(value: 'EUR', child: Text('EUR')),
                  DropdownMenuItem(value: 'USD', child: Text('USD')),
                ],
              ),
              const SizedBox(height: 24),
              Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  TextButton(
                    onPressed: () => Navigator.of(context).pop(),
                    child: const Text('Anuluj'),
                  ),
                  ElevatedButton(
                    onPressed: () {
                      if (_formKey.currentState!.validate()) {
                        widget.onSubmit({
                          'accountNumber': accountNumber,
                          'accountTypesId': accountTypesId,
                          'currencyId': currencyId,
                        });
                        Navigator.of(context).pop();
                      }
                    },
                    child: const Text('Zapisz'),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
