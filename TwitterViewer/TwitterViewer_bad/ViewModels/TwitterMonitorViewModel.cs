using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TwitterWrapper;
using System.Windows.Input;
using System.Windows.Threading;

namespace TwitterViewer.ViewModels
{
    class TwitterMonitorViewModel : INotifyPropertyChanged
    {
        private Dispatcher _dispatcher;

        public TwitterMonitorViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            Tweets = new ObservableCollection<Tweet>();
            Model = new TwitterMonitor();
            Model.Start();

            Subscribe = new BaseCommand(
                (o) => Model.Subscribe(SearchPattern),
                (o) => !string.IsNullOrWhiteSpace(SearchPattern)
                );

            Model.NewTweetFound += (s, e) => _dispatcher.BeginInvoke(new Action(() => Tweets.Add(e.Tweet)));
        }

        public string SearchPattern { get; set; }

        public ICommand Subscribe { get; private set; }

        public ObservableCollection<Tweet> Tweets { get; set; }

        public TwitterMonitor Model { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
