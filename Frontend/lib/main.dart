import 'package:flutter/material.dart';
import 'screens/login_screen.dart';
import 'screens/register_screen.dart';
import 'screens/home_screen.dart';
import 'screens/add_expense_screen.dart'; 
import 'screens/categories_screen.dart'; // <-- DODAJ TEN IMPORT
import 'package:http/http.dart' as http;
import 'dart:convert';

// Prosty placeholder dla innych ekranów
class PlaceholderScreen extends StatelessWidget {
  final String title;
  final VoidCallback onBack;
  const PlaceholderScreen({required this.title, required this.onBack, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(
          title: Text(title),
          leading: IconButton(icon: Icon(Icons.arrow_back), onPressed: onBack),
        ),
        body: Center(child: Text('$title (tutaj pojawi się prawdziwa funkcjonalność)')),
      );
}

void main() {
  runApp(const MyApp());
}

class MyApp extends StatefulWidget {
  const MyApp({super.key});

  @override
  State<MyApp> createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  String? _token;
  bool _showRegister = false;
  String _currentScreen = 'home';

  @override
  Widget build(BuildContext context) {
    Widget screen;
    if (_token == null) {
      // Login/Register
      screen = _showRegister
          ? RegisterScreen(
              onRegisterSuccess: () => setState(() => _showRegister = false),
              onShowLogin: () => setState(() => _showRegister = false),
            )
          : LoginScreen(
              onLoginSuccess: (token) => setState(() {
                _token = token;
                _currentScreen = 'home';
              }),
              onShowRegister: () => setState(() => _showRegister = true),
            );
    } else {
      switch (_currentScreen) {
        case 'home':
          screen = HomeScreen(
            onShowExpenses: () => setState(() => _currentScreen = 'expenses'),
            onAddExpense: () => setState(() => _currentScreen = 'add_expense'),
            onShowIncomes: () => setState(() => _currentScreen = 'incomes'),
            onAddIncome: () => setState(() => _currentScreen = 'add_income'),
            onShowAccounts: () => setState(() => _currentScreen = 'accounts'),
            onAddAccount: () => setState(() => _currentScreen = 'add_account'),
            onShowGoals: () => setState(() => _currentScreen = 'goals'),
            onAddGoal: () => setState(() => _currentScreen = 'add_goal'),
            onAddInvoice: () => setState(() => _currentScreen = 'add_invoice'),
            onShowCategories: () => setState(() => _currentScreen = 'categories'), // <--- CALLBACK KATEGORII
            onLogout: () => setState(() {
              _token = null;
              _currentScreen = 'home';
            }),
          );
          break;
        case 'expenses':
          screen = ExpenseListScreen(
            token: _token!,
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'add_expense':
          screen = AddExpenseScreen(
            token: _token!,
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'categories': // <--- OBSŁUGA KATEGORII
          screen = CategoriesScreen(
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        // Przykładowe placeholdery (możesz rozwinąć w przyszłości):
        case 'incomes':
          screen = PlaceholderScreen(
            title: 'Przychody (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'add_income':
          screen = PlaceholderScreen(
            title: 'Dodaj przychód (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'accounts':
          screen = PlaceholderScreen(
            title: 'Moje konta (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'add_account':
          screen = PlaceholderScreen(
            title: 'Dodaj konto (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'goals':
          screen = PlaceholderScreen(
            title: 'Cele oszczędnościowe (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'add_goal':
          screen = PlaceholderScreen(
            title: 'Dodaj cel oszczędnościowy (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        case 'add_invoice':
          screen = PlaceholderScreen(
            title: 'Dodaj fakturę (mock)',
            onBack: () => setState(() => _currentScreen = 'home'),
          );
          break;
        default:
          screen = const Center(child: Text('Nieznany ekran'));
      }
    }

    return MaterialApp(
      title: 'BudgetBuddy',
      theme: ThemeData(primarySwatch: Colors.blue),
      home: screen,
    );
  }
}

// Ekran wydatków
class ExpenseListScreen extends StatefulWidget {
  final String token;
  final VoidCallback onBack;
  const ExpenseListScreen({super.key, required this.token, required this.onBack});

  @override
  _ExpenseListScreenState createState() => _ExpenseListScreenState();
}

class _ExpenseListScreenState extends State<ExpenseListScreen> {
  List<dynamic> _expenses = [];

  @override
  void initState() {
    super.initState();
    _fetchExpenses();
  }

  Future<void> _fetchExpenses() async {
    try {
      final response = await http.get(
        Uri.parse('http://localhost:5000/api/Expense'),
        headers: {'Authorization': 'Bearer ${widget.token}'},
      );

      if (response.statusCode == 200) {
        final data = json.decode(response.body);
        setState(() {
          _expenses = data;
        });
      } else {
        setState(() {
          _expenses = [];
        });
      }
    } catch (e) {
      setState(() {
        _expenses = [];
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Wydatki'),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: widget.onBack,
        ),
      ),
      body: _expenses.isEmpty
          ? const Center(child: CircularProgressIndicator())
          : ListView.builder(
              itemCount: _expenses.length,
              itemBuilder: (context, index) {
                final expense = _expenses[index];
                return ListTile(
                  title: Text(expense['name']),
                  subtitle: Text('Kwota: ${expense['amount']}'),
                );
              },
            ),
    );
  }
}
