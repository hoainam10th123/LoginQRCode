using LoginQRCode.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LoginQRCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly IHubContext<PresenceHub> _presenceHub;

        public QrCodeController(IHubContext<PresenceHub> presenceHub)
        {
            _presenceHub = presenceHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetQrCodeAsync(string username, string connectionId)
        {
            await _presenceHub.Clients.Clients(connectionId).SendAsync("Login", username);
            return Ok("Dummy QR Code Data");
        }
    }
}
