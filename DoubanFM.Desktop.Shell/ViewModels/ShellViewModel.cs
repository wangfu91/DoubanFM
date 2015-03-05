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
		private Window _mainWindow = App.Current.MainWindow;

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
			_mainWindow.WindowState = WindowState.Minimized;
		}

		private void SwitchToMiniMode()
		{
			_mainWindow.WindowState = WindowState.Minimized;
			_mainWindow.Hide();
		}

		private void Exit()
		{
			_mainWindow.Close();
			App.Current.Shutdown();
		}

	}
}
