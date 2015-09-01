using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using Application=Microsoft.Office.Interop.Outlook.Application;
using Exception=System.Exception;
using Row=Microsoft.Office.Interop.Outlook.Row;
using Table=Microsoft.Office.Interop.Outlook.Table;

namespace WordTemplate1
{
    public partial class Ribbon1 : OfficeRibbon
    {
        private Folder _contactFolder;
        private Table _contactTable;

        public Ribbon1()
        {
            InitializeComponent();
        }

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void SetContenControls(Row row)
        {
            Globals.ThisDocument.ptFirma.LockContents = false;
            Globals.ThisDocument.ptFirma.Text = (row["CompanyName"] ?? "Error").ToString();
            Globals.ThisDocument.ptFirma.Range.Font.Color = WdColor.wdColorBlack;
            Globals.ThisDocument.ptFirma.LockContents = true;

            Globals.ThisDocument.ptAdresse.LockContents = false;
            Globals.ThisDocument.ptAdresse.Text = (row["BusinessAddress"] ?? "Error").ToString();
            Globals.ThisDocument.ptAdresse.Range.Font.Color = WdColor.wdColorBlack;
            Globals.ThisDocument.ptAdresse.LockContents = true;

            if (row["Title"].ToString() == "Herr")
                Globals.ThisDocument.ptAnrede.Text = "Sehr geehrter Herr " + row["LastName"];
            else
            {
                Globals.ThisDocument.ptAnrede.Text = "Sehr geehrte Frau " + row["LastName"];
            }
        }

        private void btNext_Click(object sender, RibbonControlEventArgs e)
        {
            if (!_contactTable.EndOfTable)
            {
                Row nextRow = _contactTable.GetNextRow();
                SetContenControls(nextRow);
            }
        }

        private void btSearch_Click(object sender, RibbonControlEventArgs e)
        {
            //Outlook Applikation
            Application oApp = new Application();
            NameSpace oNS = oApp.GetNamespace("mapi");

            //Login
            oNS.Logon(Missing.Value, Missing.Value, true, true);

            try
            {
                //Kalender Ordner 
                _contactFolder = oApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderContacts) as Folder;

                if (_contactFolder != null)
                {
                    string filter = "@SQL=" + "\"" + "urn:schemas:contacts:sn" + "\"" + " like '%" + editBox1.Text +
                                    "%'";

                    _contactTable = _contactFolder.GetTable(filter, OlTableContents.olUserItems);

                    _contactTable.Columns.RemoveAll();
                    _contactTable.Columns.Add("FirstName"); //Vorname
                    _contactTable.Columns.Add("LastName"); //Nachname
                    _contactTable.Columns.Add("BusinessAddress"); //Adresse
                    _contactTable.Columns.Add("CompanyName"); //Firmenname
                    _contactTable.Columns.Add("Title"); //Anrede

                    if (!_contactTable.EndOfTable)
                    {
                        Row nextRow = _contactTable.GetNextRow();
                        SetContenControls(nextRow);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }

            oNS.Logoff();

            // Aufräumen
            oApp = null;
            oNS = null;
        }
    }
}