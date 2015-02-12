using DoubanFM.Desktop.API.Models;
using Microsoft.Practices.Prism.PubSubEvents;

namespace DoubanFM.Desktop.Infrastructure.Events
{
    public class SwitchChannelEvent : PubSubEvent<Channel>
    {
    }
}
