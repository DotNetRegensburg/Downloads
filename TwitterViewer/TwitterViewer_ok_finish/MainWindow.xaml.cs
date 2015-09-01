namespace TwitterViewer
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using TwitterWrapper;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TwitterMonitor _monitor = new TwitterMonitor();

        public MainWindow()
        {
            this.Tweets = new ObservableCollection<Tweet>();
            this.Subscribe = new RoutedCommand();

            this.DataContext = this;

            InitializeComponent();
            _monitor.Start();
            _monitor.NewTweetFound += (s, e) => this.Dispatcher.BeginInvoke(new Action(() => Tweets.Add(e.Tweet)));
        }

        public ObservableCollection<Tweet> Tweets { get; private set; }
        public string SearchTerm { get; set; }
        public RoutedCommand Subscribe { get; private set; }
     
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _monitor.Subscribe(SearchTerm);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SearchTerm);
        }
    }
}
