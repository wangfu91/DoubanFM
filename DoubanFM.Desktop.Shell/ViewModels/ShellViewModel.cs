using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DoubanFM.Desktop.Shell.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Brush _backgroundColor;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this._backgroundColor = new SolidColorBrush(Colors.DeepSkyBlue);
            _eventAggregator.GetEvent<SwitchBackgroudColorEvent>().Subscribe(ChangeBackgroundColor);

            this.MinimizeCommand = new DelegateCommand(Minimize);
            this.SwitchToMiniModeCommand = new DelegateCommand(SwitchToMiniMode);
            this.ExitCommand = new DelegateCommand(Exit);
        }


        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    OnPropertyChanged(() => this.BackgroundColor);
                }
            }
        }

        private void ChangeBackgroundColor(Color color)
        {
            this.BackgroundColor = new SolidColorBrush(color);
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
