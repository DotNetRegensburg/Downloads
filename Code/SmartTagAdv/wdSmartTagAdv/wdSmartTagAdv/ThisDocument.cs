using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Word;
using Action=Microsoft.Office.Tools.Word.Action;
using Application=Microsoft.Office.Interop.Outlook.Application;
using Exception=System.Exception;

namespace wdSmartTagAdv
{
    public partial class ThisDocument
    {
        private void ThisDocument_Startup(object sender, EventArgs e)
        {
            SmartTag st =
                new SmartTag("http://SmartTag/ST#SmartTagDate", "Datum in Outlook nachschlagen");

            Action menue1 = new Action("Datum in Outlook nachschlagen");

            st.Actions = new[] {menue1};

            menue1.Click += menue1_Click;

            st.Expressions.Add(new Regex("\\d{1,2}.\\d{1,2}.\\d{2,4}"));

            VstoSmartTags.Add(st);
        }

        private void menue1_Click(object sender, ActionEventArgs e)
        {

            //Datum parsen
            DateTime dt = DateTime.ParseExact(e.Text, "dd.MM.yy", null);

            //Outlook Applikation
            Application oApp = new Application();
            NameSpace oNS = oApp.GetNamespace("mapi");

            //Login
            oNS.Logon(Missing.Value, Missing.Value, true, true);

            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            try
            {
                //Kalender Ordner 
                Folder folder = oApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderCalendar) as Folder;

                if (folder != null)
                {
                    //Nach dem Datum filtern mit JET
                    string filter = "[Start] >= '" + dt.ToString("g", culture)
                                    + "' And [End] <= '" + dt.AddDays(1).ToString("g", culture) + "'";

                    Items calenderItems = folder.Items.Restrict(filter);

                    calenderItems.Sort("[Start]", System.Type.Missing);
                    calenderItems.IncludeRecurrences = true;

                    //Ausgabe vorbereiten
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Folgende Termine sind an dem Tag vermerkt:\n\n");

                    //Nicht Outlook.Items.Count verwenden, Wegen IncludeRecurrences
                    foreach (AppointmentItem appItem in calenderItems)
                    {
                        sb.AppendFormat("Titel:\t {0}\n", appItem.Subject);
                        sb.AppendFormat("Uhrzeit:\t {0} - {1}\n", appItem.Start.ToShortTimeString(),
                                        appItem.End.ToShortTimeString());
                        sb.AppendLine();
                    }

                    //Ausgabe
                    MessageBox.Show(sb.ToString(), "Terminübersicht für den " + dt.ToShortDateString(),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }

            oNS.Logoff();

            // Aufräumen
            oApp = null;
            oNS = null;
        }

        private void ThisDocument_Shutdown(object sender, EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisDocument_Startup;
            Shutdown += ThisDocument_Shutdown;
        }

        #endregion
    }
}