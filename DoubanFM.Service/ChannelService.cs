
namespace DoubanFM.Service
{
    public class ChannelService : BaseService
    {

        public async void GetChannels(int channel, string type)
        {
            var param = new ServiceParameter { channel = channel.ToString(), type = type };
            var response = await Get("j/app/radio/channels", param);
            var content = response.Content;

        }
    }
}
