using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NugetCollector.Api.Hubs
{
    internal class CodeHubSender : ICodeHubSender
    {
        private readonly IHubContext<CodeHub, ICodeHub> _hubContext;

        public CodeHubSender(IHubContext<CodeHub, ICodeHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendMessage(string message)
        {
            return _hubContext.Clients.All.SendMessage("Synchronizing all repositories");
        }

        public Task SendStatus(string repositoryName, string status)
        {
            return _hubContext.Clients.Group(repositoryName).SendStatus(new Notification
            {
                Name = repositoryName,
                Message = status
            });
        }
    }
}