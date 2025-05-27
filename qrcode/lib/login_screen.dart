import 'package:flutter/material.dart';
import 'package:qrcode/Api/Auth.dart';
import 'package:qrcode/Home_screen.dart';
import 'Global.dart';
import 'models/ResponseData.dart';
import 'models/UserLogin.dart';
import 'models/UserResponse.dart';


enum FormType { login, register }

class LoginScreen extends StatefulWidget {
  final String title;

  LoginScreen({Key? key, required this.title})
      : super(key: key);

  @override
  State<LoginScreen> createState() {
    return LoginScreenState();
  }
}

class LoginScreenState extends State<LoginScreen> {
  static final formKey = GlobalKey<FormState>();
  FormType _formType = FormType.login;
  String _authHint = '';
  final UserLogin _user = UserLogin(userName: '', password: '');
  bool loading = false;

  @override
  void initState() {
    super.initState();
  }

  bool validateAndSave() {
    final form = formKey.currentState;
    if (form!.validate()) {
      form.save();
      return true;
    }
    return false;
  }

  void validateAndSubmit() async {
    if (validateAndSave()) {
      if (_formType == FormType.login) {
        setState(() {
          loading = true;
        });

        ResponseData<UserResponse> resp = await Auth.login(_user.userName, _user.password);
        if (resp.message == '200') {
          Global.username = resp.data?.userName;
          Global.token = resp.data?.token;
          Navigator.of(context).push(
            MaterialPageRoute(builder: (context) => HomeScreen()),
          );
          //save local storages
        } else {
          setState(() {
            _authHint = resp.message!;
            loading = false;
          });
        }
      }
    }
  }

  void moveToRegister() {
    formKey.currentState!.reset();
    setState(() {
      _formType = FormType.register;
      _authHint = '';
    });
  }

  void moveToLogin() {
    formKey.currentState!.reset();
    setState(() {
      _formType = FormType.login;
      _authHint = '';
    });
  }

  List<Widget> usernameAndPassword() {
    return [
      Padding(
          padding: const EdgeInsets.all(5),
          child: TextFormField(
            key: const Key('userName'),
            keyboardType: TextInputType.text,
            decoration: const InputDecoration(labelText: 'Username'),
            autocorrect: false,
            validator: (val) =>
            val!.isEmpty ? 'Username can\'t be empty.' : null,
            onSaved: (val) => _user.userName = val!,
          )),
      Padding(
          padding: const EdgeInsets.all(5),
          child: TextFormField(
            key: const Key('password'),
            decoration: const InputDecoration(labelText: 'Password'),
            obscureText: true,
            autocorrect: false,
            validator: (val) =>
            val!.isEmpty ? 'Password can\'t be empty.' : null,
            onSaved: (val) => _user.password = val!,
          )),
    ];
  }

  List<Widget> submitWidgets() {
    switch (_formType) {
      case FormType.login:
        return [
          ElevatedButton(
            onPressed: loading ? null : validateAndSubmit,
            child: loading ? const Center(
              child: CircularProgressIndicator(color: Colors.green,),
            ): const Text('Login'),
          ),
          ElevatedButton(
              onPressed: moveToRegister,
              child: const Text("Need an account? Register")),
        ];
      case FormType.register:
        return [
          ElevatedButton(
            onPressed: validateAndSubmit,
            child: const Text('register'),
          ),
          ElevatedButton(
              onPressed: moveToLogin,
              child: const Text("Have an account? Login")),
        ];
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: SingleChildScrollView(
          child: SafeArea(
            child: Column(
              children: [
                Form(
                  key: formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: usernameAndPassword() + submitWidgets(),
                  ),
                ),
                Text(_authHint, style: TextStyle(fontSize: 20, color: Colors.grey.withOpacity(0.6)),)
              ],
            ),
          ),
        )
    );
  }
}