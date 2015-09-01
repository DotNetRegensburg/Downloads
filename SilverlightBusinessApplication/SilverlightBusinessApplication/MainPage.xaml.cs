using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Automation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Printing;
using System.Windows.Shapes;
using Expression.Blend.SampleData.SampleDataSource;
using SilverlightBusinessApplication.Templates;
using System.IO;

namespace SilverlightBusinessApplication
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument document = new PrintDocument();
            document.PrintPage += DocumentOnPrintPage;
            document.Print("Customers Example");
        }

        private void DocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageVisual = this;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CustomersItem customer = (CustomersItem) DgCustomers.SelectedItem;

            PrintDocument document = new PrintDocument();
            document.PrintPage += (s, eventArgs) =>
                                      {
                                          eventArgs.PageVisual = new WelcomeCustomer(customer.Name, customer.Company,
                                                                                     customer.EMail);
                                      };
            document.Print("WelcomeCustomer");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            CustomersItem customer = (CustomersItem)DgCustomers.SelectedItem;

            Clipboard.SetText(customer.Name);
        }

        private void ImgLogo_Drop(object sender, DragEventArgs e)
        {
            FileInfo[] fileInfos = (FileInfo[]) e.Data.GetData(DataFormats.FileDrop);

            foreach (FileInfo fileInfo in fileInfos)
            {
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    ImgLogo.Source = bitmapImage;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

                WebcamWindow webcamWindow = new WebcamWindow();
                webcamWindow.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            CustomersItem customer = (CustomersItem)DgCustomers.SelectedItem;

            dynamic excel = AutomationFactory.CreateObject("Excel.Application");
            excel.Visible = true;

            dynamic workbook = excel.workbooks;
            workbook.Add();

            dynamic sheet = excel.ActiveSheet;
            dynamic range;
            range = sheet.Range("A1");
            range.Value = "Company";

            range = sheet.Range("A2");
            range.Value = customer.Company;

            range = sheet.Range("B1");
            range.Value = "Name";

            range = sheet.Range("B2");
            range.Value = customer.Name;
        }
    }
}
