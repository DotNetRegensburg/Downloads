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

namespace SilverlightBusinessApplication
{
    public partial class WebcamWindow : ChildWindow
    {
        private CaptureSource captureSource;

        public WebcamWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(WebcamWindow_Loaded);
        }

        void WebcamWindow_Loaded(object sender, RoutedEventArgs e)
        {
            captureSource = new CaptureSource();
            captureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
            captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();

            if (CaptureDeviceConfiguration.RequestDeviceAccess() || CaptureDeviceConfiguration.AllowedDeviceAccess)
            {
                VideoBrush videoBrush = new VideoBrush();
                videoBrush.SetSource(captureSource);
                captureSource.Start();
                Screen.Fill = videoBrush;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            captureSource.Stop();

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

