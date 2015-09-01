using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RK.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RK.Store.MetroSnake.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverPage : Page
    {
        public GameOverPage()
        {
            this.InitializeComponent();

            //Start fade in animation when this control gets displayed.
            this.Loaded += (sender, eArgs) =>
            {
                FadeInAnimation.Begin();
            };

            this.FadeOutAnimation.Completed += (sender, eArgs) =>
            {
                this.Frame.Content = null;
            };

            this.InvokeDelayed(
                () => this.Frame.Focus(Windows.UI.Xaml.FocusState.Programmatic),
                TimeSpan.FromMilliseconds(50.0));
        }

        /// <summary>
        /// Called when user clicks the main button..
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnCmdRestartClick(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainPage();
        }
    }
}
