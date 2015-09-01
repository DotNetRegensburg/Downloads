using System;
using System.Workflow.Activities;
using System.Workflow.Runtime;
using Contoso.MailManager.Services;
using System.Workflow.Runtime.Hosting;

public partial class Newsletter_ConfirmSubscription : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(Request.QueryString["id"]))
            return;

        Guid subscriptionId = new Guid(Request.QueryString["id"]);

        WorkflowRuntime workflowRuntime = Application["WorkflowRuntime"] as WorkflowRuntime;

        ExternalDataExchangeService exService = workflowRuntime.GetService(typeof(ExternalDataExchangeService)) as ExternalDataExchangeService;

        EmailDataService emailService = exService.GetService(typeof(Contoso.MailManager.Services.EmailDataService)) as EmailDataService;

        emailService.RaiseConfirmationReceived(subscriptionId);

        ManualWorkflowSchedulerService manualScheduler = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
        manualScheduler.RunWorkflow(subscriptionId);

        lblOutput.Text = "Subscription succeeded.";
    }
}
