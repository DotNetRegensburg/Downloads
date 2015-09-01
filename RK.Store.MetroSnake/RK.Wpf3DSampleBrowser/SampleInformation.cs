using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RK.Common.Mvvm;

namespace RK.Wpf3DSampleBrowser
{
    public class SampleInformation : IComparable<SampleInformation>
    {
        public string DisplayName
        {
            get;
            set;
        }

        public int OrderValue
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public UserControl TargetControl
        {
            get;
            set;
        }

        /// <summary>
        /// This command shows a sample on the screen.
        /// </summary>
        public DelegateCommand<SampleInformation> ApplySample
        {
            get;
            set;
        }

        public int CompareTo(SampleInformation other)
        {
            return this.OrderValue.CompareTo(other.OrderValue);
        }
    }
}
