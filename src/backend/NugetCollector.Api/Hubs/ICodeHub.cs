using System.Threading.Tasks;

namespace NugetCollector.Api.Hubs
{
    public interface ICodeHub
    {
        Task JoinGroup(string repositoryName);
        Task LeaveGroup(string repositoryName);
        Task SendStatus(Notification notification);
        Task SendMessage(string message);
    }
}