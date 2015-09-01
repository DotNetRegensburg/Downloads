using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Contoso.MailManager;
using Contoso.MailManager.Common;
using System.Net.Mail;
using System.Diagnostics;

namespace ContosoMailManager.Activities
{
    [ActivityValidator(typeof(SendConfirmationEmailValidator))]
    public partial class SendConfirmationEmailActivity : System.Workflow.Activities.SequenceActivity
	{
        protected string bodyText;
        protected string subject;
        [NonSerialized]
        protected MailMessage mailMessage;

        #region Properties
        public static DependencyProperty ToProperty = System.Workflow.ComponentModel.DependencyProperty.Register("To", typeof(string), typeof(SendConfirmationEmailActivity));

        [Description("Email recipient")]
        [Category("Email Configuration")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string To
        {
            get
            {
                return ((string)(base.GetValue(SendConfirmationEmailActivity.ToProperty)));
            }
            set
            {
                base.SetValue(SendConfirmationEmailActivity.ToProperty, value);
            }
        }

        public static DependencyProperty FromProperty = System.Workflow.ComponentModel.DependencyProperty.Register("From", typeof(string), typeof(SendConfirmationEmailActivity));

        [Description("Email sender")]
        [Category("Email Configuration")]
        [Browsable(true)]        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string From
        {
            get
            {
                return ((string)(base.GetValue(SendConfirmationEmailActivity.FromProperty)));
            }
            set
            {
                base.SetValue(SendConfirmationEmailActivity.FromProperty, value);
            }
        }
        
        public static DependencyProperty SubscriptionActionProperty = System.Workflow.ComponentModel.DependencyProperty.Register("SubscriptionAction", typeof(SubscriptionAction), typeof(SendConfirmationEmailActivity));

        [Description("Subscribe or unsubscribe?")]
        [Category("Email Configuration")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public SubscriptionAction SubscriptionAction
        {
            get
            {
                return ((SubscriptionAction)(base.GetValue(SendConfirmationEmailActivity.SubscriptionActionProperty)));
            }
            set
            {
                base.SetValue(SendConfirmationEmailActivity.SubscriptionActionProperty, value);
            }
        }

        public static DependencyProperty SubscriberIdProperty = System.Workflow.ComponentModel.DependencyProperty.Register("SubscriberId", typeof(Guid), typeof(SendConfirmationEmailActivity));

        [Description("The id of the subscriber")]
        [Category("Email Configuration")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Guid SubscriberId
        {
            get
            {
                return ((Guid)(base.GetValue(SendConfirmationEmailActivity.SubscriberIdProperty)));
            }
            set
            {
                base.SetValue(SendConfirmationEmailActivity.SubscriberIdProperty, value);
            }
        }

        public static DependencyProperty ConfirmationLinkProperty = System.Workflow.ComponentModel.DependencyProperty.Register("ConfirmationLink", typeof(string), typeof(SendConfirmationEmailActivity));

        [Description("Link for the user to click to confirm the registration.")]
        [Category("Email Configuration")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ConfirmationLink
        {
            get
            {
                return ((string)(base.GetValue(SendConfirmationEmailActivity.ConfirmationLinkProperty)));
            }
            set
            {
                base.SetValue(SendConfirmationEmailActivity.ConfirmationLinkProperty, value);
            }
        }

        public static DependencyProperty SmtpServerProperty = System.Workflow.ComponentModel.DependencyProperty.Register("SmtpServer", typeof(string), typeof(SendConfirmationEmailActivity));

        //[Description("SMTP Server to use for sending the email.")]
        //[Category("Email Configuration")]
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public string SmtpServer
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(SendConfirmationEmailActivity.SmtpServerProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(SendConfirmationEmailActivity.SmtpServerProperty, value);
        //    }
        //}
        #endregion

		public SendConfirmationEmailActivity()
		{
			InitializeComponent();           
		}

        #region Code Acti        nt Handlers
        
        private void CreateSubscriptionEmail_ExecuteCode(object sender, EventArgs e)
        {
            string url = String.Format("{0}?id={1}", this.ConfirmationLink, this.SubscriberId.ToString());
            this.bodyText = string.Format("Please confirm subscription of the newsletter by clicking on the following link: {0}.", url);

            this.subject = "Confirm newsletter subscription.";
        }

        private void CreateUnsubscriptionEmail_ExecuteCode(object sender, EventArgs e)
        {
            this.bodyText = string.Format("You have been unsubscribed from our newsletter. Goodbye.");

            this.subject = "Newsletter subscription cancelled.";
        }

        private void FinalizeMessage_ExecuteCode(object sender, EventArgs e)
        {
            //Compose the mail message
            MailAddress toAddress = new MailAddress(To);
            MailAddress fromAddress = new MailAddress(From);

            this.mailMessage = new MailMessage(fromAddress, toAddress);

            this.mailMessage.Subject = this.subject;
            this.mailMessage.Body = this.bodyText;
            
            SmtpClient mailClient = new SmtpClient();
            mailClient.Send(this.mailMessage);
        }
        #endregion
	}
}
