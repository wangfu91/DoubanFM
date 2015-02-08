using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.Channels.ViewModels
{
    public class ChannelListViewModel : ViewModelBase
    {
        private IChannelService _channelSerivce;
        private Channel _currentChannel;

        public ChannelListViewModel(IChannelService channelService)
        {
            this._channelSerivce = channelService;
            this.ChannelList = new ObservableCollection<Channel>();

            GetChannels();
        }

        public ObservableCollection<Channel> ChannelList { get; set; }

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {
                if (value != _currentChannel)
                {
                    _currentChannel = value;
                    OnPropertyChanged(() => this.CurrentChannel);
                }
            }
        }

        private async void GetChannels()
        {
            var results = await _channelSerivce.GetChannels();
            if (results != null && results.Channels.Count > 0)
            {
                results.Channels.ForEach(c => this.ChannelList.Add(c));
            }
        }

    }
}
