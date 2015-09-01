using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook.Extensions;
using Microsoft.Office.Interop.Outlook.Extensions.Linq;

namespace Basta08
{
    public partial class WpfOlAddIn
    {
        public WpfOlAddIn()
        {
            InitializeComponent();
            searchText.TextChanged += searchText_TextChanged;
            // Fügen Sie Code, der bei der Objekterstellung erforderlich ist, unter diesem Punkt ein.
        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchInbox();
        }

        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchInbox();
        }

        private void SearchInbox()
        {
            //Posteingang
            var folder =
                Globals.ThisAddIn.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as
                Folder;

            //Hat der Benutzer uns Attachments geschickt?
            var results = from mItem in folder.Items.AsQueryable<Mail>()
                          where mItem.Body.Contains(searchText.Text)
                          select new {Date = mItem.Date.ToShortDateString(), mItem.Subject};

            SearchResult.DataContext = results;
        }
    }
}