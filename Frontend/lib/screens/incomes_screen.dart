import 'package:flutter/material.dart';

class IncomesScreen extends StatefulWidget {
  final String token;
  const IncomesScreen({Key? key, required this.token}) : super(key: key);

  @override
  _IncomesScreenState createState() => _IncomesScreenState();
}

class _IncomesScreenState extends State<IncomesScreen> {
  List<dynamic> _incomes = [];

  @override
  void initState() {
    super.initState();
    _fetchIncomes();
  }

  Future<void> _fetchIncomes() async {
    // TODO: zamień na prawdziwe wywołanie API
    setState(() {
      _incomes = [
        {'name': 'Wynagrodzenie', 'amount': 3500},
        {'name': 'Sprzedaż rzeczy', 'amount': 400},
      ];
    });
  }

  @override
  Widget build(BuildContext context) {
    return _incomes.isEmpty
        ? const Center(child: CircularProgressIndicator())
        : ListView.builder(
      itemCount: _incomes.length,
      itemBuilder: (context, index) {
        final income = _incomes[index];
        return ListTile(
          title: Text(income['name']),
          trailing: Text('+ ${income['amount']} zł'),
        );
      },
    );
  }
}
