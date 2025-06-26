import 'package:flutter/material.dart';

class WelcomeScreen extends StatelessWidget {
  final VoidCallback onStartWizard;

  const WelcomeScreen({Key? key, required this.onStartWizard}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF9F0D6),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
            const Text(
              'Witaj w BudgetBuddy!',
              style: TextStyle(
                fontSize: 28,
                fontWeight: FontWeight.bold,
                color: Color(0xFF081C2C),
              ),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 20),
            const Text(
              'Aby rozpocząć korzystanie z aplikacji, skonfiguruj podstawowe dane.',
              style: TextStyle(fontSize: 16),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 40),
            ElevatedButton(
              onPressed: onStartWizard,
              style: ElevatedButton.styleFrom(
                backgroundColor: const Color(0xFFF7B733),
                padding: const EdgeInsets.symmetric(horizontal: 32, vertical: 16),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              ),
              child: const Text('Rozpocznij konfigurację', style: TextStyle(fontSize: 16, color: Colors.white)),
            ),
          ]),
        ),
      ),
    );
  }
}
