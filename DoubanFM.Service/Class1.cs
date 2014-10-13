using RestSharp;

namespace DoubanFM.Service
{
    public class Class1
    {
        public Class1()
        {

        }

        RestClient client = new RestClient("http://www.douban.com/");

        public string Get(string resource)
        {

            var request = new RestRequest(resource, Method.GET);

            //request.AddUrlSegment("subjectID", "2272292");

            //request.AddParameter("", "");

            var response = client.Execute(request);

            //Debug.WriteLine(response.ResponseUri);

            var content = response.Content;

            return content;
        }

        public string Post(string resource)
        {
            var request = new RestRequest(resource, Method.POST);

            request.AddParameter("app_name", "radio_desktop_win");
            request.AddParameter("version", "100");
            request.AddParameter("sid", "");
            request.AddParameter("channel", "1");
            request.AddParameter("type", "n");

            var response = client.Execute(request);

            var content = response.Content;

            return content;
        }
    }
}
