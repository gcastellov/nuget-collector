using System.Threading.Tasks;

namespace NugetCollector.Api.Hubs
{
    public interface ICodeHubSender
    {
        Task SendMessage(string message);
        Task SendStatus(string repositoryName, string status);
    }
}