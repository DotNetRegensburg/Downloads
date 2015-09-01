namespace TwitterViewer
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using TwitterWrapper;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private TwitterMonitor _monitor = new TwitterMonitor();

        public MainWindow()
        {
            InitializeComponent();
            _monitor.Start();
            _monitor.NewTweetFound += (s, e) => this.Dispatcher.BeginInvoke(new Action(() => lstTweets.Items.Add(e.Tweet)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstTweets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridPreview.DataContext = lstTweets.SelectedItem;
        }

        private void txtQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSubscribe.IsEnabled = !string.IsNullOrEmpty(txtQuery.Text);
        }

        private void btnSubscribe_Click(object sender, RoutedEventArgs e)
        {
            _monitor.Subscribe(txtQuery.Text);
        }
    }
}
