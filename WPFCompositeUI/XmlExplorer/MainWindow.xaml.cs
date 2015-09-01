using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;

namespace XmlExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Erzeuge neuen Dialog (View)
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "XML Files|*.xml|All Files|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Lade und parse XML-Dokument
                    XmlDocument document = new XmlDocument();
                    document.Load(dialog.FileName);
                    
                    // Setze Knotenliste als Quelle für Datenbindung
                    DataContext = document.SelectNodes("/");

                    // Aktiviere "Close"
                    this.closeMenuItem.IsEnabled = true;
                }
                // Fehlerbehandlung...
                catch (IOException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "I/O Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "XML Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            this.closeMenuItem.IsEnabled = false;
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
