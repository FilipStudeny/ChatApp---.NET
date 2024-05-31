import 'package:flutter/material.dart' show BuildContext, Placeholder, State, StatefulWidget, Widget;

class FeedPage extends StatefulWidget {
  const FeedPage({super.key});

  static const String route = "feed";

  @override
  State<FeedPage> createState() => _FeedPageState();
}

class _FeedPageState extends State<FeedPage> {
  @override
  Widget build(BuildContext context) {
    return const Placeholder();
  }
}
