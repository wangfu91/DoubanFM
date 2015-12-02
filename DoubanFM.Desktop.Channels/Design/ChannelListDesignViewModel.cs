using DoubanFM.Desktop.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            if (Infrastructure.Extension.d.IsInDesignMode)
                LoadDesignTimeData();
            else
                return;
        }

        private void LoadDesignTimeData()
        {
            //var currentDirectory=Environment.CurrentDirectory
            var sampleJsonPath = @"C:\Users\Frank\Source\Repos\Douban\SampleData\app_channels.json";
            using (var reader = new StreamReader(sampleJsonPath))
            {
                var json = reader.ReadToEnd();
                var groupList = JsonConvert.DeserializeObject<ChannelGroupList>(json);
                ChannelList = groupList.Groups.First().Channels;
            }             

            CurrentChannel = ChannelList.First();

        }

        public List<Channel> ChannelList { get; set; }
        public Channel CurrentChannel { get; set; }

    }

}
