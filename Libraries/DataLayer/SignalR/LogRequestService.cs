using DataLayer.Mongo.Entities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DataLayer.SignalR
{
    public class LogRequestService
    {
        private readonly IHubContext<RequestHub> _hub;

        public LogRequestService(IHubContext<RequestHub> hub)
        {
            this._hub = hub;
        }
        public async Task SendLogRequestToMetricsClient(LogRequest testing)
        {
            await this._hub.Clients.All.SendAsync("SendRequestStart", testing);
        }
    }
}
