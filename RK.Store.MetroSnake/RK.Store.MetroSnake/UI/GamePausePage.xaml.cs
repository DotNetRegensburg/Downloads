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
using RK.Store.MetroSnake.Util;
using RK.Common;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RK.Store.MetroSnake.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePausePage : Page
    {
        public GamePausePage()
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
        private void OnCmdMainClick(object sender, RoutedEventArgs e)
        {
            if (this.FadeOutAnimation.GetCurrentState() == ClockState.Stopped)
            {
                this.FadeOutAnimation.Begin();
            }
        }

        private void OnCmdMainKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.P) { return; }

            if (this.FadeOutAnimation.GetCurrentState() == ClockState.Stopped)
            {
                this.FadeOutAnimation.Begin();
            }
        }
    }
}
