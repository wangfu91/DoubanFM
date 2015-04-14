using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DoubanFM.Desktop.Infrastructure.Events
{
    public class SwitchBackgroudColorEvent : PubSubEvent<Color>
    {
    }
}
