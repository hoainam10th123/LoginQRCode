import 'package:flutter/material.dart';
import 'package:qrcode/Global.dart';
import 'package:qrcode/SignalRHub.dart';
import 'QrCode_screen.dart';


class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key});

  @override
  State<HomeScreen> createState() {
    return HomeScreenState();
  }
}

class HomeScreenState extends State<HomeScreen>{
  SignalRHub signalr = SignalRHub();


  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    signalr.createHubConnection(Global.token!);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Home"),
        actions: [
          IconButton(
              onPressed: (){
                Navigator.of(context).push(MaterialPageRoute(
                  builder: (context) => const QRViewExample(),
                ));
              },
              icon: const Icon(Icons.qr_code)
          )
        ],
      ),
      body: Center(
        child: Column(
          children: [
            Text(Global.username.toString())
          ],
        ),
      ),
    );
  }
}

