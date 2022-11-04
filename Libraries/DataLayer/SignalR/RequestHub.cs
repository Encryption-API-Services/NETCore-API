using DataLayer.Mongo.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DataLayer.SignalR
{
    public class RequestHub : Hub
    {
        public async Task SendMessageRequestStart(LogRequest request)
        {
            await Clients.All.SendAsync("SendMessageRequestStart", request);
        }
    }
}
