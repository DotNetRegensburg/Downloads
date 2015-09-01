namespace TwitterViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using TwitterWrapper;
    using System.ComponentModel;
    using System.Windows.Threading;
    using System;
    using System.Windows.Data;

    class TwitterMonitorViewModel : INotifyPropertyChanged
    {

        Dispatcher _uiDispatcher;
        public TwitterMonitor Model { get; private set; }

        public ICollectionView Tweets { get { return CollectionViewSource.GetDefaultView(_tweets); } }
        public ObservableCollection<Tweet> _tweets;

        public ICommand Subscribe { get; private set; }

        private Tweet _selectedTweet;

        public Tweet SelectedTweet
        {
            get { return _selectedTweet; }
            set
            {
                if (value == _selectedTweet)
                {
                    return;
                }

                _selectedTweet = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedTweet"));
                }
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (value == _searchTerm)
                {
                    return;
                }

                _searchTerm = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchTerm"));
                }
            }
        }


        public TwitterMonitorViewModel()
        {
            _uiDispatcher = Dispatcher.CurrentDispatcher;
            this.Model = new TwitterMonitor();
            this.Model.Start();
            this.Subscribe = new SubscribeCommand(this);
            this._tweets = new ObservableCollection<Tweet>();
            Tweets.SortDescriptions.Add(new SortDescription("created_at", ListSortDirection.Descending));
            Model.NewTweetFound += (s, e) => _uiDispatcher.BeginInvoke(new Action(() => _tweets.Add(e.Tweet)));


        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
