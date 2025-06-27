import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../services/api_service.dart';

class AuthProvider extends ChangeNotifier {
  String? _token;
  String? _userId;
  int?    _budgetId;
  String? _userFirstName;

  String? get token => _token;
  String? get userId => _userId;
  int?    get budgetId => _budgetId;
  String? get userFirstName => _userFirstName;

  final ApiService _api = ApiService();

  bool get isAuthenticated => _token != null && _token!.isNotEmpty;

  /// Ustawia token, wyciąga userId z tokena, zapisuje token i ładuje profil użytkownika
  Future<void> setToken(String token) async {
    _token  = token;
    _userId = _extractUserIdFromToken(token);

    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('token', token);

    if (_userId != null) {
      await loadUserProfile();
    }

    notifyListeners();
  }

  /// Ładuje token, budgetId i imię z SharedPreferences przy starcie
  Future<void> loadFromPrefs() async {
    final prefs       = await SharedPreferences.getInstance();
    final savedToken    = prefs.getString('token');
    final savedBudgetId = prefs.getInt('budgetId');
    final savedName     = prefs.getString('userFirstName');

    if (savedToken != null) {
      _token  = savedToken;
      _userId = _extractUserIdFromToken(savedToken);
      if (savedName != null) {
        _userFirstName = savedName;
      } else if (_userId != null) {
        await loadUserProfile();
      }
    }

    if (savedBudgetId != null) {
      _budgetId = savedBudgetId;
    }

    notifyListeners();
  }

  /// Pobiera profil z API i wyciąga imię (pole 'firstName')
  Future<void> loadUserProfile() async {
    if (_token == null || _userId == null) return;
    try {
      final profile = await _api.getUserProfile(_token!, _userId!);
      // Zakładam, że w profilu jest pole 'firstName'
      _userFirstName = profile['firstName'] as String?;
      final prefs = await SharedPreferences.getInstance();
      if (_userFirstName != null) {
        await prefs.setString('userFirstName', _userFirstName!);
      }
      notifyListeners();
    } catch (e) {
      debugPrint('Nie udało się pobrać profilu: $e');
    }
  }

  /// Ustawia budgetId w pamięci i SharedPreferences
  Future<void> setBudgetId(int id) async {
    _budgetId = id;
    final prefs = await SharedPreferences.getInstance();
    await prefs.setInt('budgetId', id);
    notifyListeners();
  }

  /// Czyści wszystko przy wylogowaniu
  Future<void> clearToken() async {
    _token          = null;
    _userId         = null;
    _budgetId       = null;
    _userFirstName  = null;

    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('token');
    await prefs.remove('budgetId');
    await prefs.remove('userFirstName');

    notifyListeners();
  }

  /// Wyciąga userId z JWT (pole 'nameid' lub 'sub')
  String? _extractUserIdFromToken(String token) {
    try {
      final parts = token.split('.');
      if (parts.length != 3) return null;
      final payload = json.decode(
        utf8.decode(
          base64Url.decode(base64Url.normalize(parts[1])),
        ),
      ) as Map<String, dynamic>;
      return payload['nameid']?.toString() ?? payload['sub']?.toString();
    } catch (_) {
      return null;
    }
  }
}
