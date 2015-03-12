using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;

namespace DoubanFM.Desktop.Channels.ViewModels
{
    public class ChannelListViewModel : ViewModelBase
    {
        private IChannelService _channelSerivce;
        private Channel _currentChannel;
        private IEventAggregator _eventAggregator;

        public ChannelListViewModel(
            IChannelService channelService,
            IEventAggregator eventAggregator)
        {
            this._channelSerivce = channelService;
            this._eventAggregator = eventAggregator;
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
                    HandleCurrentChannelChange();
                }
            }
        }

        private void HandleCurrentChannelChange()
        {
            if (CurrentChannel != null)
            {
                _eventAggregator.GetEvent<SwitchChannelEvent>()
                    .Publish(CurrentChannel);
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
