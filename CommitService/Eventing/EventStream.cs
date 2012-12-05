using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ServiceStack.Text;

namespace CommitService.Eventing
{
    public class EventStream : Hub
    {
        public static void NotifyAllClients(string topic, object message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventStream>();
            context.Clients.All.Notify(topic, message.ToJson());
        }
    }
}