namespace TwitterViewer
{
    using System.Windows;
    using TwitterViewer.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TwitterMonitorViewModel _vm;

        public MainWindow()
        {
            _vm = new TwitterMonitorViewModel();
            this.DataContext = _vm;
            InitializeComponent();
        }
    }
}
