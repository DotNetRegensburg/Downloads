using System;
using System.Collections.Generic;
using System.Configuration;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using Contoso.MailManager.Common;
using Contoso.MailManager.Workflows;

public partial class Newsletter : System.Web.UI.Page
{    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SubscriptionAction action = (SubscriptionAction)Enum.Parse(typeof(SubscriptionAction), rblSubscribe.SelectedValue);
        
            Guid subscriptionID = Guid.NewGuid();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserEmailAddress", txtEmail.Text.Trim());
            parameters.Add("SenderEmailAddress", ConfigurationManager.AppSettings["SenderEmailAddress"]);
            parameters.Add("Action", action);
            parameters.Add("ConfirmationLink", ConfigurationManager.AppSettings["ConfirmationLink"]);
            parameters.Add("SubscriptionID", subscriptionID);
            parameters.Add("TimeoutDuration", TimeSpan.Parse(ConfigurationManager.AppSettings["SubscriptionTimeout"]));

            WorkflowRuntime workflowRuntime = Application["WorkflowRuntime"] as WorkflowRuntime;            
            
            WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                typeof(NewsletterSubscriptionWorkflow), 
                parameters, 
                subscriptionID);

            instance.Start();

            ManualWorkflowSchedulerService manualScheduler = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;

            manualScheduler.RunWorkflow(instance.InstanceId);

            //Update the UI
            mView.ActiveViewIndex = 1;

            if (rblSubscribe.SelectedValue.Equals("subscribe", StringComparison.InvariantCultureIgnoreCase))
                lblMessage.Text = "Subscription registered. Please click on the link in the mail we have sent you.";
            else
                lblMessage.Text = "Subscription has been cancelled. Goodbye.";

        }
    }
}