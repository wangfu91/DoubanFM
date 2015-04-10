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

namespace DoubanFM.Desktop.Channels.ViewModels
{
    public class ChannelListViewModel : ViewModelBase
    {
        private IChannelService _channelSerivce;
        private Channel _currentChannel;
        private IEventAggregator _eventAggregator;
        private bool _isLoggedIn;
        private CollectionViewSource _channelListViewSource;

        public ChannelListViewModel(
            IChannelService channelService,
            IEventAggregator eventAggregator)
        {
            this._channelSerivce = channelService;
            this._eventAggregator = eventAggregator;
            this._channelListViewSource = new CollectionViewSource();

            _eventAggregator.GetEvent<UserStateChangedEvent>().Subscribe(async s => await HandleUserStateChange(s));
            GetChannels().ConfigureAwait(false);
        }

        public ICollectionView ChannelsView
        {
            get { return this._channelListViewSource.View; }
            private set
            {
                OnPropertyChanged(() => this.ChannelsView);
            }
        }

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
            var channelList = new ObservableCollection<Channel>();
            var results = await _channelSerivce.GetChannels();
            if (results != null && results.Channels.Count > 0)
            {
                results.Channels.ForEach(channelList.Add);
            }
            this._channelListViewSource.Source = channelList;
            this.ChannelsView = this._channelListViewSource.View;
        }

        private async Task HandleUserStateChange(LoginResult result)
        {
            if (result != null)
            {
                IsLoggedIn = true;
                _channelSerivce = new ChannelService(result);
            }
            else
            {
                IsLoggedIn = false;
                _channelSerivce = new ChannelService();
            }
            await GetChannels();
        }

    }
}
