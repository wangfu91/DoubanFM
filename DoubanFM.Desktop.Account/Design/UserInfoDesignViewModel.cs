using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoubanFM.Desktop.Account.Design
{
	public class UserInfoDesignViewModel
	{
		public UserInfoDesignViewModel()
		{
			if (IsInDesignMode)
				LoadDesignTimeData();
			else
				return;
		}

		private void LoadDesignTimeData()
		{
			User = new User
			{
				Name = "Coding4u",
				PlayedNum = 3848,
				LikedNum = 144,
				BannedNum = 113,
				Icon = "http://img3.douban.com/icon/u67242159-1.jpg"
			};
		}

		public bool IsInDesignMode
		{
			get
			{
				return (bool)DesignerProperties.IsInDesignModeProperty
					.GetMetadata(typeof(DependencyObject)).DefaultValue;
			}
		}

		public User User { get; set; }
	}
}
