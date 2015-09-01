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

public partial class TrackingDetails : System.Web.UI.Page
{
    private Guid _InstanceId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(Request.QueryString["id"]))
            Response.Redirect("Tracking.aspx");

        _InstanceId = new Guid(Request.QueryString["id"]);
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        Guid id = _InstanceId;

        SqlTrackingQuery tQuery = new SqlTrackingQuery(ConfigurationManager.ConnectionStrings["TrackingStoreConnectionstring"].ConnectionString);
        SqlTrackingWorkflowInstance trackingWorkflowInstance;
        tQuery.TryGetWorkflow(id, out trackingWorkflowInstance);
                
        this.trackingDetailView.DataSource = trackingWorkflowInstance.WorkflowEvents;
        this.trackingDetailView.DataBind();
    }
        
    public Guid InstanceId
    {
        get
        {
            return _InstanceId;
        }
        set
        {
            _InstanceId = value;
        }
    }
}
