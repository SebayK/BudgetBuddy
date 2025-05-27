import 'package:flutter/material.dart';

class CategoriesScreen extends StatelessWidget {
  final VoidCallback onBack;
  const CategoriesScreen({Key? key, required this.onBack}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final List<String> mockCategories = [
      "Jedzenie", "Transport", "Rozrywka", "Rachunki", "Zakupy"
    ];
    return Scaffold(
      appBar: AppBar(
        title: const Text("Kategorie"),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: onBack,
        ),
      ),
      body: ListView(
        children: [
          ...mockCategories.map((c) => ListTile(
                leading: const Icon(Icons.category),
                title: Text(c),
                trailing: IconButton(
                  icon: const Icon(Icons.edit),
                  onPressed: () {}, // tu możesz rozwinąć edycję w przyszłości
                ),
              )),
          const Divider(),
          ListTile(
            leading: const Icon(Icons.add),
            title: const Text("Dodaj kategorię"),
            onTap: () {
              // Tu możesz dodać logikę dodawania nowej kategorii
            },
          ),
        ],
      ),
    );
  }
}
