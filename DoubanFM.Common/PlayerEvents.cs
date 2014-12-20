using DoubanFM.Data;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Common
{

    public class NormalEndedEvent : PubSubEvent<Tuple<string, SongResult>> { }

    public class SkipedEvent : PubSubEvent<Tuple<string, SongResult>> { }

    public class LikedEnvent : PubSubEvent<Tuple<string, SongResult>> { }

    public class UnlikedEvent : PubSubEvent<Tuple<string, SongResult>> { }

    public class BannedEvent : PubSubEvent<Tuple<string, SongResult>> { }


}
