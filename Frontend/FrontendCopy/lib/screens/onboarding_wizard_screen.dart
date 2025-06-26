// lib/screens/onboarding_wizard_screen.dart

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/auth_provider.dart';
import '../services/api_service.dart';

class OnboardingWizardScreen extends StatefulWidget {
  final VoidCallback onFinish;

  const OnboardingWizardScreen({
    Key? key,
    required this.onFinish,
  }) : super(key: key);

  @override
  State<OnboardingWizardScreen> createState() => _OnboardingWizardScreenState();
}

class _OnboardingWizardScreenState extends State<OnboardingWizardScreen> {
  final _budgetNameCtrl = TextEditingController();
  bool _enableNotifications = false;
  bool _isLoading = false;
  String? _error;
  int _currentStep = 0;

  Future<void> _createBudget() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    try {
      final newBudgetId = await ApiService()
          .createBudgetAndReturnId(token, _budgetNameCtrl.text.trim());
      if (newBudgetId != null) {
        // zapisz w providerze
        await Provider.of<AuthProvider>(context, listen: false)
            .setBudgetId(newBudgetId);
        setState(() => _currentStep = 1);
      } else {
        setState(() => _error = 'Nie otrzymano ID nowego budżetu');
      }
    } catch (e) {
      setState(() => _error = 'Błąd tworzenia budżetu: $e');
    }
    setState(() => _isLoading = false);
  }

  Future<void> _finish() async {
    setState(() => _isLoading = true);
    final token = Provider.of<AuthProvider>(context, listen: false).token!;
    if (_enableNotifications) {
      try {
        await ApiService().enableNotifications(token);
      } catch (_) {
        // ignoruj
      }
    }
    setState(() => _isLoading = false);
    widget.onFinish();
  }

  List<Step> get _steps => [
    Step(
      title: const Text('Nowy budżet'),
      content: Column(
        children: [
          TextField(
            controller: _budgetNameCtrl,
            decoration: const InputDecoration(labelText: 'Nazwa budżetu'),
          ),
          if (_error != null) ...[
            const SizedBox(height: 8),
            Text(_error!, style: const TextStyle(color: Colors.red)),
          ],
        ],
      ),
      isActive: _currentStep >= 0,
      state: _currentStep > 0 ? StepState.complete : StepState.indexed,
    ),
    Step(
      title: const Text('Powiadomienia'),
      content: CheckboxListTile(
        title: const Text('Włącz powiadomienia'),
        value: _enableNotifications,
        onChanged: (v) =>
            setState(() => _enableNotifications = v ?? false),
      ),
      isActive: _currentStep >= 1,
      state: _currentStep > 1 ? StepState.complete : StepState.indexed,
    ),
    Step(
      title: const Text('Zakończ'),
      content: const Text('Gotowe! Kliknij "Zakończ", aby przejść do aplikacji.'),
      isActive: _currentStep >= 2,
      state: StepState.indexed,
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Pierwsza konfiguracja')),
      body: Stepper(
        currentStep: _currentStep,
        onStepContinue: () async {
          if (_currentStep == 0) {
            if (_budgetNameCtrl.text.trim().isEmpty) {
              setState(() => _error = 'Podaj nazwę budżetu');
              return;
            }
            await _createBudget();
          } else if (_currentStep == 1) {
            setState(() => _currentStep = 2);
          } else {
            await _finish();
          }
        },
        onStepCancel: () {
          if (_currentStep > 0) setState(() => _currentStep--);
        },
        controlsBuilder: (context, details) {
          final isLast = _currentStep == _steps.length - 1;
          return Padding(
            padding: const EdgeInsets.only(top: 16),
            child: Row(
              children: [
                ElevatedButton(
                  onPressed: _isLoading ? null : details.onStepContinue,
                  child: _isLoading
                      ? const SizedBox(
                    width: 20,
                    height: 20,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  )
                      : Text(isLast ? 'Zakończ' : 'Dalej'),
                ),
                const SizedBox(width: 12),
                if (_currentStep > 0)
                  TextButton(
                    onPressed: _isLoading ? null : details.onStepCancel,
                    child: const Text('Wstecz'),
                  ),
              ],
            ),
          );
        },
        steps: _steps,
      ),
    );
  }
}
