namespace TwitterViewer
{
    using System;
    using System.Windows.Input;
    using TwitterViewer.ViewModels;

    class SubscribeCommand : ICommand
    {
        private TwitterMonitorViewModel _twitterMonitorViewModel;

        public SubscribeCommand(TwitterMonitorViewModel twitterMonitorViewModel)
        {
            _twitterMonitorViewModel = twitterMonitorViewModel;
            _twitterMonitorViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "SearchTerm")
                {
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, null);
                    }
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_twitterMonitorViewModel.SearchTerm);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _twitterMonitorViewModel.Model.Subscribe(_twitterMonitorViewModel.SearchTerm);
        }
    }
}
