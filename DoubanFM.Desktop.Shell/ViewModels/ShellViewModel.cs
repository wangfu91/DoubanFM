using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DoubanFM.Desktop.Shell.ViewModels
{
	public class ShellViewModel : ViewModelBase
	{
		public ShellViewModel()
		{
			this.MinimizeCommand = new DelegateCommand(Minimize);
			this.SwitchToMiniModeCommand = new DelegateCommand(SwitchToMiniMode);
			this.ExitCommand = new DelegateCommand(Exit);
		}


		public ICommand MinimizeCommand { get; set; }

		public ICommand SwitchToMiniModeCommand { get; set; }

		public ICommand ExitCommand { get; set; }

		private void Minimize()
		{
			App.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private void SwitchToMiniMode()
		{
			App.Current.MainWindow.WindowState = WindowState.Minimized;
			//TODO: After Hide MainWindow should exist in SystemTray, but it dispear, can only shutdown it down use TaskManager.
            //App.Current.MainWindow.Hide();
		}

		private void Exit()
		{
			App.Current.MainWindow.Close();
			//App.Current.Shutdown();
		}

	}
}
