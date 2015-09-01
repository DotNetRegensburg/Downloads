using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Contoso.MailManager.Workflows
{
	partial class NewsletterSubscriptionWorkflow
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding2 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding3 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding4 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding5 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding6 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            this.SendSubscriptionCancellation1 = new ContosoMailManager.Activities.SendConfirmationEmailActivity();
            this.DeleteFromDb2 = new System.Workflow.Activities.CallExternalMethodActivity();
            this.ConfirmationTimeout = new System.Workflow.Activities.DelayActivity();
            this.faultHandlersActivity2 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity2 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.UpdateDb = new System.Workflow.Activities.CallExternalMethodActivity();
            this.ConfirmationReceived = new System.Workflow.Activities.HandleExternalEventActivity();
            this.TimeOut = new System.Workflow.Activities.EventDrivenActivity();
            this.SubscriptionConfirmed = new System.Workflow.Activities.EventDrivenActivity();
            this.SendSubscriptionCancellation2 = new ContosoMailManager.Activities.SendConfirmationEmailActivity();
            this.DeleteFromDb1 = new System.Workflow.Activities.CallExternalMethodActivity();
            this.WaitForConfirmation = new System.Workflow.Activities.ListenActivity();
            this.SendSubscriptionConfirmationMail = new ContosoMailManager.Activities.SendConfirmationEmailActivity();
            this.InsertInDb = new System.Workflow.Activities.CallExternalMethodActivity();
            this.Else = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfSubscribe = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.SubscribeOrUnsubscribe = new System.Workflow.Activities.IfElseActivity();
            // 
            // SendSubscriptionCancellation1
            // 
            activitybind1.Name = "NewsletterSubscriptionWorkflow";
            activitybind1.Path = "ConfirmationLink";
            this.SendSubscriptionCancellation1.Description = "Sends an email confirming the subscription of a newsletter or its unsubscription." +
                "";
            activitybind2.Name = "NewsletterSubscriptionWorkflow";
            activitybind2.Path = "SenderEmailAddress";
            this.SendSubscriptionCancellation1.Name = "SendSubscriptionCancellation1";
            activitybind3.Name = "NewsletterSubscriptionWorkflow";
            activitybind3.Path = "SubscriptionID";
            this.SendSubscriptionCancellation1.SubscriptionAction = Contoso.MailManager.Common.SubscriptionAction.Unsubscribe;
            activitybind4.Name = "NewsletterSubscriptionWorkflow";
            activitybind4.Path = "UserEmailAddress";
            this.SendSubscriptionCancellation1.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.SendSubscriptionCancellation1.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ConfirmationLinkProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SendSubscriptionCancellation1.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.SubscriberIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.SendSubscriptionCancellation1.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // DeleteFromDb2
            // 
            this.DeleteFromDb2.InterfaceType = typeof(Contoso.MailManager.Services.IEmailDataService);
            this.DeleteFromDb2.MethodName = "RemoveSubscription";
            this.DeleteFromDb2.Name = "DeleteFromDb2";
            activitybind5.Name = "NewsletterSubscriptionWorkflow";
            activitybind5.Path = "UserEmailAddress";
            workflowparameterbinding1.ParameterName = "email";
            workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.DeleteFromDb2.ParameterBindings.Add(workflowparameterbinding1);
            // 
            // ConfirmationTimeout
            // 
            this.ConfirmationTimeout.Name = "ConfirmationTimeout";
            this.ConfirmationTimeout.TimeoutDuration = System.TimeSpan.Parse("00:00:00");
            this.ConfirmationTimeout.InitializeTimeoutDuration += new System.EventHandler(this.ConfirmationTimeOut_InitializeTimeout);
            // 
            // faultHandlersActivity2
            // 
            this.faultHandlersActivity2.Name = "faultHandlersActivity2";
            // 
            // cancellationHandlerActivity2
            // 
            this.cancellationHandlerActivity2.Name = "cancellationHandlerActivity2";
            // 
            // UpdateDb
            // 
            this.UpdateDb.InterfaceType = typeof(Contoso.MailManager.Services.IEmailDataService);
            this.UpdateDb.MethodName = "ConfirmSubscription";
            this.UpdateDb.Name = "UpdateDb";
            activitybind6.Name = "NewsletterSubscriptionWorkflow";
            activitybind6.Path = "ConfirmationReceivedArgs.InstanceId";
            workflowparameterbinding2.ParameterName = "id";
            workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.UpdateDb.ParameterBindings.Add(workflowparameterbinding2);
            // 
            // ConfirmationReceived
            // 
            this.ConfirmationReceived.EventName = "ConfirmationReceived";
            this.ConfirmationReceived.InterfaceType = typeof(Contoso.MailManager.Services.IEmailDataService);
            this.ConfirmationReceived.Name = "ConfirmationReceived";
            activitybind7.Name = "NewsletterSubscriptionWorkflow";
            activitybind7.Path = "ConfirmationReceivedArgs";
            workflowparameterbinding3.ParameterName = "e";
            workflowparameterbinding3.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.ConfirmationReceived.ParameterBindings.Add(workflowparameterbinding3);
            // 
            // TimeOut
            // 
            this.TimeOut.Activities.Add(this.ConfirmationTimeout);
            this.TimeOut.Activities.Add(this.DeleteFromDb2);
            this.TimeOut.Activities.Add(this.SendSubscriptionCancellation1);
            this.TimeOut.Name = "TimeOut";
            // 
            // SubscriptionConfirmed
            // 
            this.SubscriptionConfirmed.Activities.Add(this.ConfirmationReceived);
            this.SubscriptionConfirmed.Activities.Add(this.UpdateDb);
            this.SubscriptionConfirmed.Activities.Add(this.cancellationHandlerActivity2);
            this.SubscriptionConfirmed.Activities.Add(this.faultHandlersActivity2);
            this.SubscriptionConfirmed.Name = "SubscriptionConfirmed";
            // 
            // SendSubscriptionCancellation2
            // 
            activitybind8.Name = "NewsletterSubscriptionWorkflow";
            activitybind8.Path = "ConfirmationLink";
            this.SendSubscriptionCancellation2.Description = "Sends an email confirming the subscription of a newsletter or its unsubscription." +
                "";
            activitybind9.Name = "NewsletterSubscriptionWorkflow";
            activitybind9.Path = "SenderEmailAddress";
            this.SendSubscriptionCancellation2.Name = "SendSubscriptionCancellation2";
            activitybind10.Name = "NewsletterSubscriptionWorkflow";
            activitybind10.Path = "SubscriptionID";
            this.SendSubscriptionCancellation2.SubscriptionAction = Contoso.MailManager.Common.SubscriptionAction.Unsubscribe;
            activitybind11.Name = "NewsletterSubscriptionWorkflow";
            activitybind11.Path = "UserEmailAddress";
            this.SendSubscriptionCancellation2.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ConfirmationLinkProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.SendSubscriptionCancellation2.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.SendSubscriptionCancellation2.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.SendSubscriptionCancellation2.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.SubscriberIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // DeleteFromDb1
            // 
            this.DeleteFromDb1.InterfaceType = typeof(Contoso.MailManager.Services.IEmailDataService);
            this.DeleteFromDb1.MethodName = "RemoveSubscription";
            this.DeleteFromDb1.Name = "DeleteFromDb1";
            activitybind12.Name = "NewsletterSubscriptionWorkflow";
            activitybind12.Path = "UserEmailAddress";
            workflowparameterbinding4.ParameterName = "email";
            workflowparameterbinding4.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.DeleteFromDb1.ParameterBindings.Add(workflowparameterbinding4);
            // 
            // WaitForConfirmation
            // 
            this.WaitForConfirmation.Activities.Add(this.SubscriptionConfirmed);
            this.WaitForConfirmation.Activities.Add(this.TimeOut);
            this.WaitForConfirmation.Name = "WaitForConfirmation";
            // 
            // SendSubscriptionConfirmationMail
            // 
            activitybind13.Name = "NewsletterSubscriptionWorkflow";
            activitybind13.Path = "ConfirmationLink";
            this.SendSubscriptionConfirmationMail.Description = "Sends an email confirming the subscription of a newsletter or its unsubscription." +
                "";
            activitybind14.Name = "NewsletterSubscriptionWorkflow";
            activitybind14.Path = "SenderEmailAddress";
            this.SendSubscriptionConfirmationMail.Name = "SendSubscriptionConfirmationMail";
            activitybind15.Name = "NewsletterSubscriptionWorkflow";
            activitybind15.Path = "SubscriptionID";
            this.SendSubscriptionConfirmationMail.SubscriptionAction = Contoso.MailManager.Common.SubscriptionAction.Subscribe;
            activitybind16.Name = "NewsletterSubscriptionWorkflow";
            activitybind16.Path = "UserEmailAddress";
            this.SendSubscriptionConfirmationMail.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ConfirmationLinkProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.SendSubscriptionConfirmationMail.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.SendSubscriptionConfirmationMail.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.SendSubscriptionConfirmationMail.SetBinding(ContosoMailManager.Activities.SendConfirmationEmailActivity.SubscriberIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // InsertInDb
            // 
            this.InsertInDb.InterfaceType = typeof(Contoso.MailManager.Services.IEmailDataService);
            this.InsertInDb.MethodName = "InsertSubscription";
            this.InsertInDb.Name = "InsertInDb";
            activitybind17.Name = "NewsletterSubscriptionWorkflow";
            activitybind17.Path = "UserEmailAddress";
            workflowparameterbinding5.ParameterName = "email";
            workflowparameterbinding5.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            activitybind18.Name = "NewsletterSubscriptionWorkflow";
            activitybind18.Path = "SubscriptionID";
            workflowparameterbinding6.ParameterName = "id";
            workflowparameterbinding6.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            this.InsertInDb.ParameterBindings.Add(workflowparameterbinding5);
            this.InsertInDb.ParameterBindings.Add(workflowparameterbinding6);
            // 
            // Else
            // 
            this.Else.Activities.Add(this.DeleteFromDb1);
            this.Else.Activities.Add(this.SendSubscriptionCancellation2);
            this.Else.Name = "Else";
            // 
            // IfSubscribe
            // 
            this.IfSubscribe.Activities.Add(this.InsertInDb);
            this.IfSubscribe.Activities.Add(this.SendSubscriptionConfirmationMail);
            this.IfSubscribe.Activities.Add(this.WaitForConfirmation);
            ruleconditionreference1.ConditionName = "ActivateSubscription";
            this.IfSubscribe.Condition = ruleconditionreference1;
            this.IfSubscribe.Name = "IfSubscribe";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // SubscribeOrUnsubscribe
            // 
            this.SubscribeOrUnsubscribe.Activities.Add(this.IfSubscribe);
            this.SubscribeOrUnsubscribe.Activities.Add(this.Else);
            this.SubscribeOrUnsubscribe.Name = "SubscribeOrUnsubscribe";
            // 
            // NewsletterSubscriptionWorkflow
            // 
            this.Activities.Add(this.SubscribeOrUnsubscribe);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "NewsletterSubscriptionWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private CancellationHandlerActivity cancellationHandlerActivity2;
        private FaultHandlersActivity faultHandlersActivity1;
        private CancellationHandlerActivity cancellationHandlerActivity1;
        private FaultHandlersActivity faultHandlersActivity2;
        private ContosoMailManager.Activities.SendConfirmationEmailActivity SendSubscriptionCancellation1;
        private CallExternalMethodActivity DeleteFromDb1;
        private CallExternalMethodActivity DeleteFromDb2;
        private CallExternalMethodActivity UpdateDb;
        private CallExternalMethodActivity InsertInDb;
        private ContosoMailManager.Activities.SendConfirmationEmailActivity SendSubscriptionCancellation2;
        private ContosoMailManager.Activities.SendConfirmationEmailActivity SendSubscriptionConfirmationMail;
        private IfElseBranchActivity Else;
        private IfElseBranchActivity IfSubscribe;
        private IfElseActivity SubscribeOrUnsubscribe;
        private EventDrivenActivity TimeOut;
        private EventDrivenActivity SubscriptionConfirmed;
        private ListenActivity WaitForConfirmation;
        private HandleExternalEventActivity ConfirmationReceived;
        private DelayActivity ConfirmationTimeout;




































































    }
}
