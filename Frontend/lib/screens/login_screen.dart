import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class LoginScreen extends StatefulWidget {
  final void Function(String token) onLoginSuccess;
  final VoidCallback onShowRegister;

  const LoginScreen({
    super.key,
    required this.onLoginSuccess,
    required this.onShowRegister,
  });

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();
  String? _error;
  bool _isLoading = false;

  Future<void> _login() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    final url = Uri.parse('http://localhost:5000/api/authentication/login'); // Podmień na IP serwera, jeśli testujesz z telefonu

    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'username': _usernameController.text,
          'password': _passwordController.text,
        }),
      );

      if (response.statusCode == 200) {
        // Zakładam, że backend zwraca czysty token jako plain text
        final token = response.body;
        if (token.isNotEmpty) {
          widget.onLoginSuccess(token);
        } else {
          setState(() => _error = 'Brak tokena w odpowiedzi');
        }
      } else {
        // Jeśli odpowiedź to treść błędu (możesz ją też sparsować z response.body, jeśli masz JSON z message)
        setState(() => _error = response.body.isNotEmpty ? response.body : 'Nieprawidłowy login lub hasło');
      }
    } catch (e) {
      setState(() => _error = 'Błąd połączenia: $e');
    } finally {
      setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Logowanie')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            TextField(
              controller: _usernameController,
              decoration: const InputDecoration(labelText: 'Nazwa użytkownika'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _passwordController,
              obscureText: true,
              decoration: const InputDecoration(labelText: 'Hasło'),
            ),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: _isLoading ? null : _login,
              child: _isLoading
                  ? const SizedBox(height: 16, width: 16, child: CircularProgressIndicator(strokeWidth: 2))
                  : const Text('Zaloguj się'),
            ),
            TextButton(
              onPressed: widget.onShowRegister,
              child: const Text('Nie masz konta? Zarejestruj się!'),
            ),
            if (_error != null) ...[
              const SizedBox(height: 16),
              Text(_error!, style: const TextStyle(color: Colors.red)),
            ],
          ],
        ),
      ),
    );
  }
}
