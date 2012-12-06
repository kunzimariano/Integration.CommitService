using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ServiceStack.Text;

namespace CommitService.Eventing
{
    public class EventStream : Hub
    {
        public static void Publish<T>(T message) {
            var topic = typeof (T).Name;
            Publish(topic, message);
        }

        public static void Publish(string topic, object message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventStream>();
            context.Clients.Group(topic).Receive(topic, message.ToJson());
        }

        public void Subscribe(string topic)
        {
            Groups.Add(Context.ConnectionId, topic);
        }
    }
}