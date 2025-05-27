import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

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

    // Jeśli jesteś na emulatorze Android - ZAMIEN localhost na 10.0.2.2!
    final url = Uri.parse('http://localhost:5000/api/authentication/registration');
    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'username': _usernameController.text,
          'firstName': _firstNameController.text,
          'lastName': _lastNameController.text,
          'role': 0, // lub inna wartość zależnie od wymagań backendu
          'email': _emailController.text,
          'password': _passwordController.text,
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        widget.onRegisterSuccess();
      } else {
        setState(() => _error = 'Błąd rejestracji: ${response.body}');
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
