// lib/services/api_service.dart

import 'dart:convert';
import 'dart:io' show Platform;
import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:http/http.dart' as http;

class ApiService {
  final String baseUrl = _getBaseUrl();

  static String _getBaseUrl() {
    if (kIsWeb) return 'http://localhost:5000';
    if (Platform.isAndroid) return 'http://10.0.2.2:5000';
    return 'http://localhost:5000';
  }

  // ONBOARDING

  Future<bool> checkUserHasData(String token) async {
    final uri = Uri.parse('$baseUrl/api/Onboarding/Check');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final j = jsonDecode(r.body);
      return (j['hasAccountTypes'] || j['hasAccounts'] || j['hasBudget']) == true;
    }
    throw Exception('Error checking onboarding: ${r.statusCode}');
  }

  Future<int?> createBudgetAndReturnId(String token, String name) async {
    final uri = Uri.parse('$baseUrl/api/Budgets');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode({'name': name}),
    );
    if (r.statusCode == 200 || r.statusCode == 201) {
      final j = jsonDecode(r.body);
      return j['id'] as int?;
    }
    throw Exception('Error creating budget: ${r.statusCode}');
  }

  Future<void> enableNotifications(String token) async {
    final uri = Uri.parse('$baseUrl/api/Notifications/Enable');
    final r = await http.post(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode != 200) {
      throw Exception('Error enabling notifications: ${r.statusCode}');
    }
  }

  Future<Map<String, dynamic>> getUserProfile(String token, String userId) async {
    final uri = Uri.parse('$baseUrl/api/User/$userId');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      return jsonDecode(r.body) as Map<String, dynamic>;
    }
    throw Exception('Error fetching user profile: ${r.statusCode}');
  }

  // BUDGETS

  Future<List<Map<String, dynamic>>> getUserBudgets(String token) async {
    final uri = Uri.parse('$baseUrl/api/Budgets');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final d = jsonDecode(r.body);
      final list = d is List ? d : [d];
      return list
          .cast<Map<String, dynamic>>()
          .map((b) => {'id': b['id'], 'name': b['name']})
          .toList();
    }
    throw Exception('Error fetching budgets: ${r.statusCode}');
  }

  Future<List<Map<String, dynamic>>> getSharedBudgets(String token) async {
    final uri = Uri.parse('$baseUrl/api/ShareBudgets');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      return (jsonDecode(r.body) as List).cast<Map<String, dynamic>>();
    }
    throw Exception('Error fetching shared budgets: ${r.statusCode}');
  }

  Future<bool> createSharedBudget(String token, String name, List<String> emails) async {
    final uri = Uri.parse('$baseUrl/api/ShareBudgets');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode({'name': name, 'emails': emails}),
    );
    return r.statusCode == 200 || r.statusCode == 201;
  }

  Future<double> getBudgetBalance(int budgetId, String token) async {
    final uri = Uri.parse('$baseUrl/api/BudgetBalance/$budgetId');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final j = jsonDecode(r.body);
      return (j['balance'] as num).toDouble();
    }
    throw Exception('Error fetching balance: ${r.statusCode}');
  }

  // ACCOUNT TYPES

  Future<List<Map<String, dynamic>>> getAccountTypes(String token) async {
    final uri = Uri.parse('$baseUrl/api/AccountType');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final data = jsonDecode(r.body);
      if (data is List) return data.cast<Map<String, dynamic>>();
      if (data is Map && data.containsKey(r'$values')) {
        return (data[r'$values'] as List).cast<Map<String, dynamic>>();
      }
    }
    throw Exception('Error fetching account types: ${r.statusCode}');
  }

  Future<int> createAccountType({
    required String token,
    required String name,
    required String emoji,
    required int colorValue,
  }) async {
    final uri = Uri.parse('$baseUrl/api/AccountType');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode({'name': name, 'emoji': emoji, 'color': colorValue}),
    );
    if (r.statusCode == 200 || r.statusCode == 201) {
      final j = jsonDecode(r.body);
      return j['id'] as int;
    }
    throw Exception('Error creating account type: ${r.statusCode}');
  }

  // ACCOUNTS

  Future<List<Map<String, dynamic>>> getAccounts(String token) async {
    final uri = Uri.parse('$baseUrl/api/Account');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final body = jsonDecode(r.body);
      if (body is List) return body.cast<Map<String, dynamic>>();
      if (body is Map && body.containsKey(r'$values')) {
        return (body[r'$values'] as List).cast<Map<String, dynamic>>();
      }
    }
    throw Exception('Error fetching accounts: ${r.statusCode}');
  }

  Future<bool> createAccount({
    required String token,
    required String userId,
    required int accountTypeId,
    required String currencyId,
    required int accountNumber,
  }) async {
    final uri = Uri.parse('$baseUrl/api/Account');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode({
        'userId': userId,
        'accountTypesId': accountTypeId,
        'currencyId': currencyId,
        'accountNumber': accountNumber,
      }),
    );
    return r.statusCode == 200 || r.statusCode == 201;
  }

  Future<bool> deleteAccount({
    required String token,
    required int accountId,
  }) async {
    final uri = Uri.parse('$baseUrl/api/Account/$accountId');
    final r = await http.delete(uri, headers: {'Authorization': 'Bearer $token'});
    return r.statusCode == 200 || r.statusCode == 204;
  }

  // NEW: getAccountBalance

  /// Pobiera bieżące saldo dla konta o podanym ID.
  Future<double> getAccountBalance({
    required String token,
    required int accountId,
  }) async {
    final uri = Uri.parse('$baseUrl/api/Account/$accountId/Balance');
    final r = await http.get(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json',
      },
    );
    if (r.statusCode == 200) {
      final body = r.body.trim();
      // Jeśli odpowiedź to czysta liczba:
      if (body.startsWith(RegExp(r'\d'))) {
        return double.parse(body);
      }
      // Albo JSON { "balance": x }
      final j = jsonDecode(body);
      return (j['balance'] as num).toDouble();
    }
    throw Exception('Error fetching account balance: ${r.statusCode}');
  }

  // CATEGORIES

  Future<List<Map<String, dynamic>>> getCategories(
      String token, String userId, String type) async {
    final uri = Uri.parse('$baseUrl/api/Category?userId=$userId&type=$type');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final b = jsonDecode(r.body);
      if (b is List) return b.cast<Map<String, dynamic>>();
      if (b is Map && b.containsKey(r'$values')) {
        return (b[r'$values'] as List).cast<Map<String, dynamic>>();
      }
    }
    throw Exception('Error fetching categories: ${r.statusCode}');
  }

  Future<bool> createCategory(
      String token, String userId, String type, String name) async {
    final uri = Uri.parse('$baseUrl/api/Category');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode({'userId': userId, 'type': type, 'name': name}),
    );
    return r.statusCode == 200 || r.statusCode == 201;
  }

  // TRANSACTIONS

  Future<List<Map<String, dynamic>>> getTransactions(
      String token, String endpoint) async {
    final uri = Uri.parse('$baseUrl/api/$endpoint');
    final r = await http.get(uri, headers: {'Authorization': 'Bearer $token'});
    if (r.statusCode == 200) {
      final b = jsonDecode(r.body);
      if (b is List) return b.cast<Map<String, dynamic>>();
      if (b is Map && b.containsKey(r'$values')) {
        return (b[r'$values'] as List).cast<Map<String, dynamic>>();
      }
    }
    throw Exception('Error fetching $endpoint: ${r.statusCode}');
  }

  Future<bool> createTransaction(
      String token, String endpoint, Map<String, dynamic> dto) async {
    final uri = Uri.parse('$baseUrl/api/$endpoint');
    final r = await http.post(
      uri,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json'
      },
      body: jsonEncode(dto),
    );
    return r.statusCode == 200 || r.statusCode == 201;
  }
}
