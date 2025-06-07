import 'package:flutter/material.dart';

class SavingsScreen extends StatelessWidget {
  const SavingsScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final List<Map<String, String>> savings = [
      {'goal': 'Wakacje', 'amount': '3000', 'saved': '1500'},
      {'goal': 'Nowy komputer', 'amount': '5000', 'saved': '2000'},
    ];

    return ListView(
      children: savings
          .map(
            (s) => ListTile(
          title: Text(s['goal']!),
          subtitle: Text('Zebrano: ${s['saved']} / ${s['amount']} zł'),
          trailing: const Icon(Icons.flag),
        ),
      )
          .toList(),
    );
  }
}
