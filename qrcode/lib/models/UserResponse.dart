class UserResponse{
  String displayName;
  String userName;
  String token;

  UserResponse({
    required this.displayName,
    required this.userName,
    required this.token
  });

  factory UserResponse.fromJson(Map<String, dynamic> json) {
    return UserResponse(
      displayName: json['displayName'],
      userName: json['userName'],
      token: json['token']
    );
  }
}