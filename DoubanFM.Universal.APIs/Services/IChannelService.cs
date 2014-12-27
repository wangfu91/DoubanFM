using DoubanFM.Universal.APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public interface IChannelService
    {
        Task<ChannelList> GetChannels();
    }
}
