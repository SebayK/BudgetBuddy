import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';

class RegisterScreen extends StatefulWidget {
  final VoidCallback onRegisterSuccess;
  final VoidCallback onShowLogin;

  const RegisterScreen({
    super.key,
    required this.onRegisterSuccess,
    required this.onShowLogin,
  });

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _usernameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  String? _error;
  bool _isLoading = false;

  Future<void> _register() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final registerUrl = Uri.parse('http://localhost:5000/api/authentication/registration');
    final loginUrl = Uri.parse('http://localhost:5000/api/authentication/login');
    final budgetUrl = Uri.parse('http://localhost:5000/api/Budgets');

    try {
      // 1. Rejestracja
      final registerResponse = await http.post(
        registerUrl,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'username': _usernameController.text,
          'firstName': _firstNameController.text,
          'lastName': _lastNameController.text,
          'role': 0,
          'email': _emailController.text,
          'password': _passwordController.text,
        }),
      );

      if (registerResponse.statusCode == 200 || registerResponse.statusCode == 201) {
        // 2. Automatyczne logowanie po rejestracji
        final loginResponse = await http.post(
          loginUrl,
          headers: {'Content-Type': 'application/json'},
          body: jsonEncode({
            'username': _usernameController.text,
            'password': _passwordController.text,
          }),
        );

        if (loginResponse.statusCode == 200) {
          final token = loginResponse.body;
          if (token.isNotEmpty) {
            authProvider.setToken(token);

            // 3. Pobranie budżetów
            final budgetResponse = await http.get(
              budgetUrl,
              headers: {'Authorization': 'Bearer $token'},
            );

            if (budgetResponse.statusCode == 200) {
              final List<dynamic> budgets = jsonDecode(budgetResponse.body);
              if (budgets.isNotEmpty) {
                final int budgetId = budgets[0]['id'];
                authProvider.setBudgetId(budgetId);
              }
              // Powiadamiamy, że rejestracja i pobranie danych zakończyło się sukcesem
              widget.onRegisterSuccess();
            } else {
              setState(() => _error = 'Nie udało się pobrać budżetów: ${budgetResponse.statusCode}');
            }
          } else {
            setState(() => _error = 'Brak tokena po rejestracji');
          }
        } else {
          setState(() => _error = 'Błąd logowania po rejestracji');
        }
      } else {
        setState(() => _error = 'Błąd rejestracji: ${registerResponse.body}');
      }
    } catch (e) {
      setState(() => _error = 'Błąd połączenia: $e');
    } finally {
      setState(() => _isLoading = false);
    }
  }

  @override
  void dispose() {
    _usernameController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _firstNameController.dispose();
    _lastNameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Rejestracja')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              TextField(
                controller: _usernameController,
                decoration: const InputDecoration(labelText: 'Nazwa użytkownika'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: _firstNameController,
                decoration: const InputDecoration(labelText: 'Imię'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: _lastNameController,
                decoration: const InputDecoration(labelText: 'Nazwisko'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: _emailController,
                decoration: const InputDecoration(labelText: 'Email'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: _passwordController,
                obscureText: true,
                decoration: const InputDecoration(labelText: 'Hasło'),
              ),
              const SizedBox(height: 16),
              ElevatedButton(
                onPressed: _isLoading ? null : _register,
                child: _isLoading
                    ? const CircularProgressIndicator()
                    : const Text('Zarejestruj się'),
              ),
              TextButton(
                onPressed: widget.onShowLogin,
                child: const Text('Masz już konto? Zaloguj się!'),
              ),
              if (_error != null) ...[
                const SizedBox(height: 16),
                Text(_error!, style: const TextStyle(color: Colors.red)),
              ],
            ],
          ),
        ),
      ),
    );
  }
}