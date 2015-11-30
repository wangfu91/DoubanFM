using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class SearchService : ServiceBase, ISearchService
    {
        private const string searchChannel = "search/channel";

        public async Task<SearchChannels> SearchChannel(string query, int start, int size)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"q",query },
                {"start",start.ToString() },
                {"limit", size.ToString() }
            };

            var requestUri = BuildRequestUri(BaseUrl, searchChannel, paramSet);
            return await SendRequestAsync<SearchChannels>(requestUri, "", HttpMethod.Get);
        }
    }
}
