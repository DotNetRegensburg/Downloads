using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightBusinessApplication.Templates
{
    public partial class WelcomeCustomer : UserControl
    {
        public WelcomeCustomer(string name, string company, string email)
        {
            InitializeComponent();

            TbName.Text = name;
            TbCompany.Text = company;
            TbEMail.Text = email;
        }
    }
}
