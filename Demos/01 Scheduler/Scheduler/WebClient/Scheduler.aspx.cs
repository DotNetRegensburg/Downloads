using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Workflow.Runtime;
using Workflows;
using System.Workflow.Runtime.Hosting;

public partial class Scheduler : System.Web.UI.Page
{
    private string _outputMessage;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PreRenderComplete += new EventHandler(OnPreRenderComplete);
    }

    void OnPreRenderComplete(object sender, EventArgs e)
    {
        this.lblOutput.Text = _outputMessage;
    }

    void runtime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
    {
        string returnValue = e.OutputParameters["Message"].ToString();
        
        _outputMessage += "<br/>Workflow instance " + e.WorkflowInstance.InstanceId + " completed." +
            " Return value: <i>" + returnValue + "</i>";        
    }

    protected void btnStartWorkflow_Click(object sender, EventArgs e)
    {
        WorkflowRuntime runtime = (WorkflowRuntime)Application["WorkflowRuntime"];
        runtime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(runtime_WorkflowCompleted);

        WorkflowInstance instance = runtime.CreateWorkflow(typeof(UselessWorkflow));
        instance.Start();

        _outputMessage = "Workflow instance " + instance.InstanceId + " started.";

        ManualWorkflowSchedulerService scheduler = runtime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
        scheduler.RunWorkflow(instance.InstanceId);
    }
}
