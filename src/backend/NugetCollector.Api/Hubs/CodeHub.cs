using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NugetCollector.Api.Hubs
{
    public class CodeHub : Hub<ICodeHub>
    {
        public Task JoinGroup(string repositoryName)
        {
            return base.Groups.AddToGroupAsync(Context.ConnectionId, repositoryName)
                .ContinueWith(t => base.Clients.Group(repositoryName).SendStatus(new Notification
                {
                    Name = repositoryName,
                    Message = "Connected"
                }));
        }

        public Task LeaveGroup(string repositoryName)
        {
            return base.Groups.RemoveFromGroupAsync(Context.ConnectionId, repositoryName);
        }
    }
}