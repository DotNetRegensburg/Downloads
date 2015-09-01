using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RK.Common;
using RK.Common.Mvvm;

namespace RK.Wpf3DSampleBrowser
{
    public class MainWindowSource : ViewModelBase
    {
        private UserControl m_selectedControl;
        private List<SampleInformation> m_wpfSamples;
        private List<SampleInformation> m_sharpDXSamples;

        [ImportMany(Infrastructure.SAMPLE_CONTRACT, typeof(UserControl))]
        private UserControl[] m_sampleUserControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowSource" /> class.
        /// </summary>
        public MainWindowSource()
        {
            //Load all sample controls
            Infrastructure.Compose(this);

            //Define local commands
            this.ApplySample = new DelegateCommand<SampleInformation>(
                (givenSampleInfo) =>
                {
                    CommonUtil.InvokeDelayed(
                        () => this.SelectedControl = givenSampleInfo.TargetControl,
                        TimeSpan.FromMilliseconds(100.0));
                });

            //Query for sample information
            m_wpfSamples = new List<SampleInformation>();
            m_sharpDXSamples = new List<SampleInformation>();
            foreach (UserControl actSampleControl in m_sampleUserControls)
            {
                //Get attribute information
                SampleAttribute sampleAttribute = CommonUtil.GetCustomAttribute<SampleAttribute>(actSampleControl.GetType());
                DisplayNameAttribute displayName = CommonUtil.GetCustomAttribute<DisplayNameAttribute>(actSampleControl.GetType());

                //Build sample 
                SampleInformation sampleInfo = new SampleInformation();
                sampleInfo.DisplayName = displayName.DisplayName;
                sampleInfo.TargetControl = actSampleControl;
                sampleInfo.ApplySample = this.ApplySample;
                sampleInfo.OrderValue = sampleAttribute.OrderValue;
                sampleInfo.ImageUrl = sampleAttribute.PreviewImageUrl;
                switch(sampleAttribute.SampleType)
                {
                    case SampleType.SharpDXSample:
                        m_sharpDXSamples.Add(sampleInfo);
                        break;

                    case SampleType.WpfSample:
                        m_wpfSamples.Add(sampleInfo);
                        break;
                }
            }
            m_wpfSamples.Sort();
            m_sharpDXSamples.Sort();
        }

        /// <summary>
        /// This command shows a sample on the screen.
        /// </summary>
        public DelegateCommand<SampleInformation> ApplySample
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the currently selected wpf user control. This control gets displayed directly in the main region.
        /// </summary>
        public UserControl SelectedControl
        {
            get { return m_selectedControl; }
            set
            {
                if (m_selectedControl != value)
                {
                    m_selectedControl = value;
                    RaisePropertyChanged(() => SelectedControl);
                }
            }
        }

        /// <summary>
        /// Gets a collection containing all possible wpf samples.
        /// </summary>
        public IEnumerable<SampleInformation> WpfSamples
        {
            get { return m_wpfSamples; }
        }

        /// <summary>
        /// Gets a collection containing all possible sharpdx samples.
        /// </summary>
        public IEnumerable<SampleInformation> SharpDXSamples
        {
            get { return m_sharpDXSamples; }
        }
    }
}
