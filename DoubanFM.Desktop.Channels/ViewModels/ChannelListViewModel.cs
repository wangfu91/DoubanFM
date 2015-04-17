using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;

namespace DoubanFM.Desktop.Channels.ViewModels
{
    public class ChannelListViewModel : ViewModelBase
    {
        private IChannelService _channelSerivce;
        private Channel _currentChannel;
        private IEventAggregator _eventAggregator;
        private bool _isLoggedIn;
        private Channel _redHeartChannel;

        public ChannelListViewModel(
            IChannelService channelService,
            IEventAggregator eventAggregator)
        {
            this._channelSerivce = channelService;
            this._eventAggregator = eventAggregator;
            this._redHeartChannel = new Channel { Name = "红心兆赫", SeqId = 0, AbbrEN = "My", ChannelId = -3 };
            this.ChannelList = new ObservableCollection<Channel>();

            _eventAggregator.GetEvent<UserStateChangedEvent>().Subscribe(HandleUserStateChange);
            GetChannels().ConfigureAwait(false);
        }

        public ObservableCollection<Channel> ChannelList { get; private set; }

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

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                if (value != _isLoggedIn)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged(() => this.IsLoggedIn);
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

        private async Task GetChannels()
        {
            var results = await _channelSerivce.GetChannels();
            if (results != null && results.Channels.Count > 0)
            {
                results.Channels.ForEach(ChannelList.Add);
            }
            this.CurrentChannel = ChannelList.FirstOrDefault();
        }

        private void HandleUserStateChange(LoginResult result)
        {


            if (result != null)
            {
                IsLoggedIn = true;
                _channelSerivce = new ChannelService(result);
                if (!ChannelList.Contains(_redHeartChannel))
                {
                    ChannelList.Insert(0, _redHeartChannel);
                }
            }
            else
            {
                IsLoggedIn = false;
                _channelSerivce = new ChannelService();
                var redHeartSelected = CurrentChannel.ChannelId == -3;
                ChannelList.Remove(_redHeartChannel);
                if (redHeartSelected)
                {
                    CurrentChannel = ChannelList.FirstOrDefault();
                }

            }
            //await GetChannels();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.CurrentChannel = null;
            this.ChannelList = null;
        }

    }
}
