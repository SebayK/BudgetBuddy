import 'dart:convert';
import 'dart:io' show Platform;
import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:http/http.dart' as http;

class ApiService {
  final String baseUrl = _getBaseUrl();

  static String _getBaseUrl() {
    if (kIsWeb) {
      return 'http://localhost:5000';
    } else if (Platform.isAndroid) {
      return 'http://10.0.2.2:5000';
    } else {
      return 'http://localhost:5000';
    }
  }

  /// Pobiera saldo budżetu o danym ID
  Future<double> getBudgetBalance(int budgetId, String token) async {
    final response = await http.get(
      Uri.parse('$baseUrl/api/BudgetBalance/$budgetId'),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body);
      return (json['balance'] as num).toDouble();
    } else {
      throw Exception('Błąd pobierania salda budżetu');
    }
  }

  /// Pobiera listę budżetów/kont użytkownika (przykładowo)
  Future<List<Map<String, dynamic>>> getUserBudgets(String token) async {
    final response = await http.get(
      Uri.parse('$baseUrl/api/Budgets'),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 200) {
      final List data = jsonDecode(response.body);
      return data.map((item) => {
        'budgetId': item['id'] ?? 0,
        'name': 'Konto ${item['id']}',
      }).toList();
    } else {
      throw Exception('Błąd pobierania budżetów');
    }
  }
}
