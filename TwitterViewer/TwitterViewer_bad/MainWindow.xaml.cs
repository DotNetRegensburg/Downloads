// todo: 2 create vm from TwitterMonitor
// todo: 2 move views out of window

namespace TwitterViewer
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using TwitterWrapper;
    using System.Collections.ObjectModel;
    using TwitterViewer.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
