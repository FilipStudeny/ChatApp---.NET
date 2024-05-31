import 'package:flutter/material.dart';
import 'package:mobile/pages/FeedPage.dart';
import 'package:mobile/pages/RegisterPage.dart';
import 'constants/ThemeSchema.dart';
import 'pages/LoginPage.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: themeSchema,
      initialRoute: HomePage.route,
      routes: {
        HomePage.route: (context) => const HomePage(title: "hOMEpAGE"),
        LoginPage.route: (context) => const LoginPage(),
        RegisterPage.route: (context) => const RegisterPage(),
        FeedPage.route: (context) => const FeedPage()
      },
    );
  }
}

class HomePage extends StatefulWidget {
  const HomePage({super.key, required this.title});

  final String title;
  static const String route = 'home';

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Chat app - home page"),
      ),
      body: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: ElevatedButton(
              onPressed: () {
                Navigator.pushNamed(context, LoginPage.route);
              },
              child: const Text("Sign in"),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: ElevatedButton(
              onPressed: () {
                Navigator.pushNamed(context, RegisterPage.route);
              },
              child: const Text("Create account"),
            ),
          ),
        ]

      ),
    );
  }
}
