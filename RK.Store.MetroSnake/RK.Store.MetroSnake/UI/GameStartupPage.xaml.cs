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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace RK.Store.MetroSnake.UI
{
    public sealed partial class GameStartupPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStartupPage" /> class.
        /// </summary>
        public GameStartupPage()
        {
            this.InitializeComponent();

            this.FadeOutAnimation.Completed += (sender, eArgs) =>
            {
                this.Frame.Content = null;
            };
        }

        /// <summary>
        /// Called when user clicks the main button..
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnCmdMainClick(object sender, RoutedEventArgs e)
        {
            this.FadeOutAnimation.Begin();
        }
    }
}
