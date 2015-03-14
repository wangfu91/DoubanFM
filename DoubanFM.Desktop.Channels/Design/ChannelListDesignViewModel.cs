using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoubanFM.Desktop.Channels.Design
{
    internal class ChannelListDesignViewModel
    {
        public ChannelListDesignViewModel()
        {
            if (IsInDesignMode)
                LoadDesignTimeData();
            else
                return;
        }

        private void LoadDesignTimeData()
        {
            ChannelList = new List<Channel>
            {
                new Channel{Name="华语"},
                new Channel{Name="欧美"},
                new Channel{Name="七零"},
                new Channel{Name="八零"},
                new Channel{Name="九零"},
                new Channel{Name="粤语"},
                new Channel{Name="摇滚"},
                new Channel{Name="民谣"},
                new Channel{Name="轻音乐"},
                new Channel{Name="原声"},
                new Channel{Name="爵士"},
                new Channel{Name="电子"},
                new Channel{Name="说唱"},
                new Channel{Name="R&B "},
                new Channel{Name="日语"},
                new Channel{Name="韩语"},
                new Channel{Name="女声"},
                new Channel{Name="法语"},
                new Channel{Name="古典"},
                new Channel{Name="动漫"},
                new Channel{Name="咖啡馆"}
            };

            CurrentChannel = ChannelList.First();

        }

        public bool IsInDesignMode
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty
                            .GetMetadata(typeof(DependencyObject)).DefaultValue;
            }
        }

        public List<Channel> ChannelList { get; set; }
        public Channel CurrentChannel { get; set; }

    }

}
