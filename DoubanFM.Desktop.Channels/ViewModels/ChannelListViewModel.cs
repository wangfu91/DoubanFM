using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Prism.Events;

namespace DoubanFM.Desktop.Channels.ViewModels
{
    public class ChannelListViewModel : ViewModelBase
    {
        private IChannelService _channelSerivce;
        private Channel _currentChannel;
        private readonly IEventAggregator _eventAggregator;
        private bool _isLoggedIn;

        public ChannelListViewModel(
            IChannelService channelService,
            IEventAggregator eventAggregator)
        {
            this._channelSerivce = channelService;
            this._eventAggregator = eventAggregator;
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
            var channelGroupList = await _channelSerivce.GetAppChannels();
            if (channelGroupList != null)
            {
                ChannelList.Clear();
                foreach (var group in channelGroupList.Groups)
                {
                    foreach (var channel in group.Channels)
                    {
                        ChannelList.Add(channel);
                    }
                }
            }
            this.CurrentChannel = ChannelList.FirstOrDefault();
        }

        private async void HandleUserStateChange(LoginResult result)
        {

            _channelSerivce = new ChannelService();

            if (result != null)
            {
                IsLoggedIn = true;
                _channelSerivce = new ChannelService {AccessToken = result.AccessToken};
            }
            else
            {
                IsLoggedIn = false;
            }
            await GetChannels();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.CurrentChannel = null;
            this.ChannelList = null;
        }

    }
}
