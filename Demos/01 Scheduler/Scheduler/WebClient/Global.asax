<%@ Application Language="C#" %>

<%@ Import Namespace="System.Workflow.Runtime" %>
<%@ Import Namespace="System.Workflow.Runtime.Hosting" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        WorkflowRuntime workflowRuntime = new WorkflowRuntime();

        //Configure Scheduler
        ManualWorkflowSchedulerService manualService = new ManualWorkflowSchedulerService();
        workflowRuntime.AddService(manualService);
        
        workflowRuntime.StartRuntime();

        Application["WorkflowRuntime"] = workflowRuntime; 

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        WorkflowRuntime workflowRuntime = (WorkflowRuntime) Application["WorkflowRuntime"];
        workflowRuntime.StopRuntime();
        workflowRuntime.Dispose();
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
