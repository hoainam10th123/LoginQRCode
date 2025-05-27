import 'package:signalr_netcore/signalr_client.dart';


class SignalRHub{
  HubConnection? _hubConnection;

  createHubConnection(String token){
    if (_hubConnection == null) {
      String hubUrl = "http://192.168.1.7:5084/hubs/";
      _hubConnection = HubConnectionBuilder()
          .withUrl("${hubUrl}presence",
          options: HttpConnectionOptions(
              accessTokenFactory: () async => token))
          .build();

      _hubConnection!.onclose(({Exception? error}) => _myFunction(error));

      if (_hubConnection!.state != HubConnectionState.Connected) {
        _hubConnection!.start()?.catchError(
                (e) => {print("PresenceService at Start: $e")});
      }

      //_hubConnection!.on("Login", loginSignalr);
    }
  }

  _myFunction(Exception? error) => print(error.toString());
}