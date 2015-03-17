using DoubanFM.Desktop.API.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.Infrastructure.Events
{
    public class UserLoggedInEvent:PubSubEvent<LoginResult>
    {
    }

    public class UserLoggedOutEvent:PubSubEvent<LoginResult>
    {
    }
}
