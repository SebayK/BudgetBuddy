import 'package:flutter/material.dart';

class OtherScreen extends StatelessWidget {
  const OtherScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text(
        'Inne funkcje pojawią się tutaj',
        style: Theme.of(context).textTheme.titleLarge,
      ),
    );
  }
}
