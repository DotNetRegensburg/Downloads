#region

using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook.Extensions;
using Microsoft.Office.Interop.Outlook.Extensions.Linq;
using Microsoft.Office.Tools.Outlook;
using Office = Microsoft.Office.Core;

#endregion

namespace Basta08
{
    internal partial class FormRegion1
    {
        #region Form Region Factory

        [FormRegionMessageClass(FormRegionMessageClassAttribute.Contact)]
        [FormRegionName("nrw08.FormRegion1")]
        public partial class FormRegion1Factory
        {
            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void FormRegion1Factory_FormRegionInitializing(object sender, FormRegionInitializingEventArgs e)
            {
                var item = (ContactItem) e.OutlookItem;

                if (String.IsNullOrEmpty(item.Email1Address))
                    e.Cancel = true;
                else
                    return;
            }
        }

        #endregion

        private readonly WpfOlAddIn wpf = new WpfOlAddIn();
        private ElementHost elemHost;


        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void FormRegion1_FormRegionShowing(object sender, EventArgs e)
        {
            //WPF integration
            elemHost = new ElementHost();
            elemHost.Child = wpf;
            elemHost.Dock = DockStyle.Fill;
            Controls.Add(elemHost);

            ContactItem contact = OutlookItem as ContactItem;

            if (contact != null)
            {
                GetStats(contact.Email1Address);
                GetAttachments(contact.Email1Address);
            }
        }

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void FormRegion1_FormRegionClosed(object sender, EventArgs e)
        {
        }

        private void GetStats(string email)
        {
            //Statistik über den Posteingang
            Folder folderInbox =
                Globals.ThisAddIn.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as
                Folder;

            IQueryable<Mail> results = from mItem in folderInbox.Items.AsQueryable<Mail>()
                                       where mItem.FromEmail.Contains(email)
                                       select mItem;

            //Auswertung
            int counter = results.ToList().Count;
            string lastDate = results.ToList().Last().Date.ToShortDateString();

            //Werte im WPF-Control setzen
            wpf.lastContact.Text = lastDate;
            wpf.incomingCount.Text = counter.ToString();

            //Statistik über die Gesendete Objekte
            Folder folderSent =
                Globals.ThisAddIn.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderSentMail) as
                Folder;

            IQueryable<Mail> results2 = from mItem in folderSent.Items.AsQueryable<Mail>()
                                        where mItem.DisplayTo.Contains(email)
                                        select mItem;

            //Auswertung
            int counterOut = results2.ToList().Count;

            //Wert im WPF-Control setzen
            wpf.outgoingCount.Text = counterOut.ToString();

            //Gesamtanzahl der Kontakte in WPF-Control setzen
            wpf.contactCount.Text = (counterOut + counter).ToString();
        }

        private void GetAttachments(string email)
        {
            //Posteingang
            Folder folder =
                Globals.ThisAddIn.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as
                Folder;

            //Hat der Benutzer uns Attachments geschickt?
            var results = from mItem in folder.Items.AsQueryable<Mail>()
                          where (mItem.HasAttachment) && mItem.FromEmail.Contains(email)
                          select
                              new
                                  {
                                      Date = mItem.Date.ToShortDateString(),
                                      AttachmentName = mItem.Item.Attachments[1].DisplayName,
                                      mItem.Subject
                                  };

            //Liste im WPF-Control binden
            wpf.AttachmentsListView.DataContext = results;
        }


        private void StatsOld()
        {
            Folder folder =
                Globals.ThisAddIn.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox) as
                Folder;

            //string filter = "";
            //string filter = "@SQL=" + "%lastmonth(" + "\"" + "DAV:getlastmodified" + "\"" + ")%";
            string filter = "@SQL=" + "%thisweek(" + "\"" + "DAV:getlastmodified" + "\"" + ")%";

            Table table = folder.GetTable(filter, Type.Missing);


            table.Columns.Add("urn:schemas:httpmail:sender");
            table.Sort("LastModificationTime", OlSortOrder.olDescending);

            int count = 0;
            while (!table.EndOfTable)
            {
                Row nextRow = table.GetNextRow();
                if (nextRow["urn:schemas:httpmail:sender"].Equals("Lars Keller"))
                {
                    count++;
                }
            }
            MessageBox.Show("Emails: " + count);
        }
    }
}