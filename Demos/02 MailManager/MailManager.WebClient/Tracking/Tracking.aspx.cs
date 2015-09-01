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
using System.Workflow.Runtime.Tracking;
using System.Collections.Generic;


public partial class Tracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();   
        }
    }
    public void BindData()
    {
        SqlTrackingQuery tQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings["TrackingStoreConnectionstring"].ConnectionString);
        
        //Set filter, so we only get data about the NewsletterSubscriptionWorkflow
        SqlTrackingQueryOptions queryOptions = new SqlTrackingQueryOptions();
        queryOptions.WorkflowType = typeof(Contoso.MailManager.Workflows.NewsletterSubscriptionWorkflow);

        IList<SqlTrackingWorkflowInstance> result = tQuery.GetWorkflows(queryOptions);
        this.trackingGrid.DataSource = result;
        this.trackingGrid.DataBind();
    }
}
