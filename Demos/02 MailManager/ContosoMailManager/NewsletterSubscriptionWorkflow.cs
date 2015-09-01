using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Contoso.MailManager.Common;

namespace Contoso.MailManager.Workflows
{
	public sealed partial class NewsletterSubscriptionWorkflow: SequentialWorkflowActivity
    {
        #region Fields
        
        private ExternalDataEventArgs confirmationReceivedArgs;
        private string userEmailAddress;
        private string senderEmailAddress;
        private SubscriptionAction action;
        private string confirmationLink;
        private TimeSpan timeOut;
        private Guid subscriptionID;

        #endregion
        
        #region Properties
        
        public ExternalDataEventArgs ConfirmationReceivedArgs
        {
            get { return confirmationReceivedArgs; }
            set { confirmationReceivedArgs = value; }
        }
        public string UserEmailAddress
        {
            get {return userEmailAddress;}
            set { userEmailAddress = value;}
        }
        public string SenderEmailAddress
        {
            get {return senderEmailAddress;}
            set { senderEmailAddress = value; }
        }
        public SubscriptionAction Action
        {
            get { return action; }
            set { action = value; }
        }
        public string ConfirmationLink
        {
            get { return confirmationLink; }
            set { confirmationLink = value; }
        }
        public TimeSpan TimeoutDuration
        {
            get { return timeOut; }
            set { timeOut = value; }
        }       
        public Guid SubscriptionID
        {
            get { return subscriptionID; }
            set { subscriptionID = value; }
        }

        #endregion

		public NewsletterSubscriptionWorkflow()
		{
			InitializeComponent();            
        }

        private void ConfirmationTimeOut_InitializeTimeout(object sender, EventArgs e)
        {
            this.ConfirmationTimeout.TimeoutDuration = this.TimeoutDuration;
        }
	}

}
