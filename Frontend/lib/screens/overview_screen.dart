import 'package:flutter/material.dart';

class OverviewScreen extends StatelessWidget {
  const OverviewScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final accounts = [
      {'name': 'Konto domyślne', 'balance': 2500},
      {'name': 'Oszczędności', 'balance': 10000},
    ];

    final planned = <Map<String, dynamic>>[];
    final pending = <Map<String, dynamic>>[];

    final theme = Theme.of(context);
    final textStyleHeader =
    theme.textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold);
    final textStyleSectionTitle =
    theme.textTheme.titleMedium?.copyWith(fontWeight: FontWeight.w600);

    return Scaffold(
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('Przegląd', style: textStyleHeader),
                const SizedBox(height: 20),
                Card(
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16),
                  ),
                  color: theme.colorScheme.surfaceVariant,
                  child: Padding(
                    padding: const EdgeInsets.all(24),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Salda kont (2025)',
                            style: textStyleSectionTitle),
                        const SizedBox(height: 16),
                        ...accounts.map((acc) => Padding(
                          padding:
                          const EdgeInsets.symmetric(vertical: 6),
                          child: Row(
                            mainAxisAlignment:
                            MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                acc['name'].toString(),
                                style: theme.textTheme.bodyLarge,
                              ),
                              Text(
                                '${acc['balance'].toString()} zł',
                                style: theme.textTheme.bodyLarge,
                              ),
                            ],
                          ),
                        )),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 24),
                Column(
                  children: [
                    Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 140,
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text('Planowane',
                                  style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  planned.isEmpty
                                      ? 'Brak planowanych transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium?.copyWith(
                                      fontStyle: FontStyle.italic),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              const Spacer(),
                            ],
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 16),
                    Card(
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16)),
                      color: theme.colorScheme.surfaceVariant,
                      child: SizedBox(
                        height: 140,
                        child: Padding(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text('Oczekujące',
                                  style: textStyleSectionTitle),
                              const Spacer(),
                              Center(
                                child: Text(
                                  pending.isEmpty
                                      ? 'Brak oczekujących transakcji'
                                      : '',
                                  style: theme.textTheme.bodyMedium?.copyWith(
                                      fontStyle: FontStyle.italic),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              const Spacer(),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 50),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
