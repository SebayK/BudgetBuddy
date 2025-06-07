import 'package:flutter/material.dart';

class ExpensesScreen extends StatefulWidget {
  final String token;
  const ExpensesScreen({Key? key, required this.token}) : super(key: key);

  @override
  _ExpensesScreenState createState() => _ExpensesScreenState();
}

class _ExpensesScreenState extends State<ExpensesScreen> {
  List<dynamic> _expenses = [];

  @override
  void initState() {
    super.initState();
    _fetchExpenses();
  }

  Future<void> _fetchExpenses() async {
    // TODO: zamień na prawdziwe wywołanie API
    setState(() {
      _expenses = [
        {'name': 'Zakupy spożywcze', 'amount': 120},
        {'name': 'Kawa', 'amount': 15},
        {'name': 'Benzyna', 'amount': 180},
      ];
    });
  }

  @override
  Widget build(BuildContext context) {
    return _expenses.isEmpty
        ? const Center(child: CircularProgressIndicator())
        : ListView.builder(
      itemCount: _expenses.length,
      itemBuilder: (context, index) {
        final expense = _expenses[index];
        return ListTile(
          title: Text(expense['name']),
          trailing: Text('- ${expense['amount']} zł'),
        );
      },
    );
  }
}
