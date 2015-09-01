<%@ Application Language="C#" %>
<%@ Import Namespace="System.Workflow.Runtime" %>
<%@ Import Namespace="System.Workflow.Runtime.Hosting" %>
<%@ Import Namespace="System.Workflow.Runtime.Tracking" %>
<%@ Import Namespace="System.Workflow.Activities" %>

<script runat="server">    
    
    void Application_Start(object sender, EventArgs e) 
    {
        WorkflowRuntime workflowRuntime = new WorkflowRuntime();

        //************** Configure Services *******************
        
        //Scheduling
        ManualWorkflowSchedulerService manualService =
            new ManualWorkflowSchedulerService(true); //true, because we are using a delay activity
        workflowRuntime.AddService(manualService);

        //ExternalDataExchange
        ExternalDataExchangeService dataExchangeService = new ExternalDataExchangeService();
        workflowRuntime.AddService(dataExchangeService);

        //Custom
        Contoso.MailManager.Services.EmailDataService emailService = new Contoso.MailManager.Services.EmailDataService();
        dataExchangeService.AddService(emailService);

        ////Persistence
        //SqlWorkflowPersistenceService persistenceService = new SqlWorkflowPersistenceService(
        //    ConfigurationManager.ConnectionStrings["TrackingStoreConnectionstring"].ConnectionString,
        //    true, //UnloadOnIdle
        //    new TimeSpan(0, 1, 0),
        //    new TimeSpan(0, 0, 20)
        //    );
        //workflowRuntime.AddService(persistenceService);

        //Tracking
        //SqlTrackingService trackingService = new SqlTrackingService(
        //    ConfigurationManager.ConnectionStrings["TrackingStoreConnectionstring"].ConnectionString
        //    );
        //trackingService.IsTransactional = false;
        //workflowRuntime.AddService(trackingService);

        workflowRuntime.StartRuntime();

        Application["WorkflowRuntime"] = workflowRuntime; 

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        System.Workflow.Runtime.WorkflowRuntime workflowRuntime =
         Application["WorkflowRuntime"] as System.Workflow.Runtime.WorkflowRuntime;
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
