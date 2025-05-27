# LoginQRCode
## Bước 1, web load lên sẽ gọi event signalR (CreateHubConnection() tại react) xướng backend .net core lấy data là một chuỗi có dạng UUID#ConenctionId.
## Bước 2, mobile quét mã qrcode lấy ra ConenctionId, call api truyền username trên mobile đã login và ConenctionId.
## Bước 3, lúc này dưới Backend netcore sẽ bắn event tới web dựa vào ConenctionId để thực hiện chức năng đăng nhập.
## Bước 4, tại web thực hiện call api dựa vào username backend để lấy token.
