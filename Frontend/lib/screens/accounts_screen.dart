import 'package:flutter/material.dart';

class AccountsScreen extends StatelessWidget {
  const AccountsScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    // Dane jako Map<String, String>
    final List<Map<String, String>> accounts = [
      {'name': 'Konto główne', 'balance': '5000'},
      {'name': 'Konto oszczędnościowe', 'balance': '12000'},
    ];

    return ListView(
      children: accounts
          .map(
            (acc) => ListTile(
          leading: const Icon(Icons.account_balance_wallet),
          title: Text(acc['name']!),
          trailing: Text('${acc['balance']} zł'),
        ),
      )
          .toList(),
    );
  }
}
