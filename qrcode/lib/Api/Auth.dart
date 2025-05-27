import 'package:http/http.dart' as http;
import 'dart:convert';
import 'dart:io';
import '../models/ResponseData.dart';
import '../models/UserResponse.dart';


class Auth{

  static Future<ResponseData<UserResponse>> login(String username, String password) async{
    final dataRes = ResponseData<UserResponse>();
    try {
      final uri = Uri.parse('http://192.168.1.7:5084/api/account/login');

      var response = await http.post(uri, headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      }, body: jsonEncode({
        'username': username,
        'password': password,
      }),);

      if (response.statusCode == 200) {
        dataRes.message = '200';
        final user = UserResponse.fromJson(jsonDecode(response.body));
        dataRes.data = user;
      } else {
        dataRes.message = '${response.statusCode} ${response.body}';
      }
    } catch (e) {
      dataRes.message = e.toString();
    }
    return dataRes;
  }
}