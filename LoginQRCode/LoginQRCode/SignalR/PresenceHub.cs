using LoginQRCode.Dtos;
using Microsoft.AspNetCore.SignalR;
using System;

namespace LoginQRCode.SignalR
{
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.GetUsername();
            var connectionId = Context.ConnectionId;
            if (string.IsNullOrEmpty(username))
            {
                var uuid = Guid.NewGuid().ToString();                
                await _tracker.UserConnected(uuid, connectionId);
                var qrcodeData = $"{uuid}#{connectionId}";
                await Clients.Caller.SendAsync("QrCodeConnect", qrcodeData);
            }
            else 
            {
                await _tracker.UserConnected(username, connectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //var isOffline = await _tracker.UserDisconnected(username, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
