namespace TwitterViewer
{
    using System.Windows;
    using TwitterViewer.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Resources.Add("TwitterMonitorViewModel", new TwitterMonitorViewModel());
        }
    }
}
