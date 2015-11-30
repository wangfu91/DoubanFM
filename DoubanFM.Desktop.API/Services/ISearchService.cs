using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface ISearchService
    {
        Task<SearchChannels> SearchChannel(string query, int start, int size);
    }
}
