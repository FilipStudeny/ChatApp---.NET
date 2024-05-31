import 'package:flutter/material.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:mobile/components/IconContent.dart';

enum Gender { Male, Female }

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key});

  static const String route = "register";

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  String _errorMessage = '';

  final TextEditingController _usernameController = TextEditingController();
  final TextEditingController _firstnameController = TextEditingController();
  final TextEditingController _lastnameController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _repeatPasswordController = TextEditingController();

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  Gender? _selectedGender;

  void _submitForm() {
    if (_formKey.currentState!.validate()) {
      if (_passwordController.text != _repeatPasswordController.text) {
        setState(() {
          _errorMessage = 'Passwords do not match';
        });
      } else if (_selectedGender == null) {
        setState(() {
          _errorMessage = 'Please select a gender';
        });
      } else {
        setState(() {
          _errorMessage = '';
          // Handle successful form submission
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        title: const Text("Chat app - register page"),
      ),
      body: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 24.0),
        child: Form(
          key: _formKey,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              const Center(
                child: Text(
                  'Create new account',
                  style: TextStyle(fontSize: 20),
                ),
              ),
              const SizedBox(height: 2),
              TextFormField(
                controller: _usernameController,
                decoration: const InputDecoration(hintText: 'Username'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter a username';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 1),
              TextFormField(
                controller: _firstnameController,
                decoration: const InputDecoration(hintText: 'Firstname'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter your firstname';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 1),
              TextFormField(
                controller: _lastnameController,
                decoration: const InputDecoration(hintText: 'Lastname'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter your lastname';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 5),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  ElevatedButton(
                    onPressed: () {
                      setState(() {
                        _selectedGender = Gender.Male;
                      });
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: _selectedGender == Gender.Male ? Colors.blueAccent : Colors.grey,
                    ),
                    child: const IconContent(
                      icon: FontAwesomeIcons.mars,
                      label: 'Male',
                    ),
                  ),
                  ElevatedButton(
                    onPressed: () {
                      setState(() {
                        _selectedGender = Gender.Female;
                      });
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: _selectedGender == Gender.Female ? Colors.pinkAccent : Colors.grey,
                    ),
                    child: const IconContent(
                      icon: FontAwesomeIcons.venus,
                      label: 'Female',
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 1),
              TextFormField(
                controller: _emailController,
                decoration: const InputDecoration(hintText: 'Email'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter your email';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 1),
              TextFormField(
                controller: _passwordController,
                decoration: const InputDecoration(hintText: 'Password'),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter your password';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 1),
              TextFormField(
                controller: _repeatPasswordController,
                decoration: const InputDecoration(hintText: 'Repeat password'),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please repeat your password';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 1),
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8.0),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    ElevatedButton(
                      onPressed: _submitForm,
                      child: const Text("Create account"),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8.0),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    const Text("Already have an account?"),
                    ElevatedButton(
                      onPressed: () {
                        // Navigate to the sign-in page
                      },
                      child: const Text("Sign in"),
                    ),
                  ],
                ),
              ),
              if (_errorMessage.isNotEmpty)
                Container(
                  width: double.infinity,
                  padding: const EdgeInsets.all(12.0),
                  margin: const EdgeInsets.only(top: 30),
                  color: Colors.redAccent,
                  child: Text(
                    _errorMessage,
                    textAlign: TextAlign.center,
                    style: const TextStyle(
                        color: Colors.white,
                        fontSize: 18,
                        fontWeight: FontWeight.bold),
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}

