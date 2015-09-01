using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace ContosoMailManager.Activities
{
	public partial class SendConfirmationEmailActivity
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            this.CreateUnsubscriptionEmail = new System.Workflow.Activities.CodeActivity();
            this.CreateSubscriptionEmail = new System.Workflow.Activities.CodeActivity();
            this.ElseUnsubscribe = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfSubscribe = new System.Workflow.Activities.IfElseBranchActivity();
            this.FinalizeMessage = new System.Workflow.Activities.CodeActivity();
            this.CreateEmailBody = new System.Workflow.Activities.IfElseActivity();
            // 
            // CreateUnsubscriptionEmail
            // 
            this.CreateUnsubscriptionEmail.Name = "CreateUnsubscriptionEmail";
            this.CreateUnsubscriptionEmail.ExecuteCode += new System.EventHandler(this.CreateUnsubscriptionEmail_ExecuteCode);
            // 
            // CreateSubscriptionEmail
            // 
            this.CreateSubscriptionEmail.Name = "CreateSubscriptionEmail";
            this.CreateSubscriptionEmail.ExecuteCode += new System.EventHandler(this.CreateSubscriptionEmail_ExecuteCode);
            // 
            // ElseUnsubscribe
            // 
            this.ElseUnsubscribe.Activities.Add(this.CreateUnsubscriptionEmail);
            this.ElseUnsubscribe.Name = "ElseUnsubscribe";
            // 
            // IfSubscribe
            // 
            this.IfSubscribe.Activities.Add(this.CreateSubscriptionEmail);
            ruleconditionreference1.ConditionName = "IfSubscribeIsTrue";
            this.IfSubscribe.Condition = ruleconditionreference1;
            this.IfSubscribe.Name = "IfSubscribe";
            // 
            // FinalizeMessage
            // 
            this.FinalizeMessage.Name = "FinalizeMessage";
            this.FinalizeMessage.ExecuteCode += new System.EventHandler(this.FinalizeMessage_ExecuteCode);
            // 
            // CreateEmailBody
            // 
            this.CreateEmailBody.Activities.Add(this.IfSubscribe);
            this.CreateEmailBody.Activities.Add(this.ElseUnsubscribe);
            this.CreateEmailBody.Name = "CreateEmailBody";
            // 
            // SendConfirmationEmailActivity
            // 
            this.Activities.Add(this.CreateEmailBody);
            this.Activities.Add(this.FinalizeMessage);
            this.Description = "Sends an email confirming the subscription of a newsletter or its unsubscription." +
                "";
            this.Name = "SendConfirmationEmailActivity";
            this.CanModifyActivities = false;

		}

		#endregion

        private CodeActivity FinalizeMessage;
        private CodeActivity CreateSubscriptionEmail;
        private CodeActivity CreateUnsubscriptionEmail;
        private IfElseBranchActivity ElseUnsubscribe;
        private IfElseBranchActivity IfSubscribe;
        private IfElseActivity CreateEmailBody;








    }
}
