using DoubanFM.Desktop.API.Models;
using Prism.Events;

namespace DoubanFM.Desktop.Infrastructure.Events
{
    public class UserStateChangedEvent:PubSubEvent<LoginResult>
    {
    }
}
