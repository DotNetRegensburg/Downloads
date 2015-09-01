using System;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace Basta08
{
    public partial class ThisAddIn
    {
        private CommandBarButton openContactButton;
        private MailItem selection;

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            Globals.ThisAddIn.Application.ItemContextMenuDisplay += Application_ItemContextMenuDisplay;
            Globals.ThisAddIn.Application.ContextMenuClose += Application_ContextMenuClose;
        }

        private void Application_ContextMenuClose(OlContextMenu ContextMenu)
        {
            if (openContactButton != null)
                openContactButton.Click -= openContactButton_Click;
            openContactButton = null;
        }

        private void Application_ItemContextMenuDisplay(CommandBar CommandBar, Selection Selection)
        {
            if (Selection.Count == 1)
            {
                selection = Selection[1] as MailItem;

                if (selection.Class == OlObjectClass.olMail)
                {
                    openContactButton = (CommandBarButton) CommandBar.Controls.Add
                                                               (MsoControlType.msoControlButton, Type.Missing,
                                                                "OpenContact", CommandBar.Controls.Count + 1,
                                                                Type.Missing);

                    openContactButton.BeginGroup = true;
                    openContactButton.Tag = "OpenContact";
                    openContactButton.Caption = "Gehe zu Kontakt";
                    openContactButton.Visible = true;
                    openContactButton.Click += openContactButton_Click;
                }
            }
        }

        private void openContactButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            var mail = Application.ActiveExplorer().Selection[1] as MailItem;

            if (mail != null)
            {
                string filter = "[Email1Address] = '" + mail.SenderEmailAddress + "'";

                var contact =
                    Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderContacts).Items.Find(
                        filter) as ContactItem;
                Inspector inspector = Application.Inspectors.Add(contact);
                inspector.Display(false);
            }
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

        #endregion
    }
}