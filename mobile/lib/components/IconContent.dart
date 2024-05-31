import 'package:flutter/material.dart';

class IconContent extends StatelessWidget {
  final IconData icon;
  final String label;

  const IconContent({super.key, required this.icon, required this.label});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(2.0),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Icon(
            icon,
            size: 40.0,
            color: Colors.white,
          ),
          Text(
            label,
            style: const TextStyle(
              fontSize: 18.0,
              color: Colors.white,
            ),
          ),
        ],
      ),
    );
  }
}