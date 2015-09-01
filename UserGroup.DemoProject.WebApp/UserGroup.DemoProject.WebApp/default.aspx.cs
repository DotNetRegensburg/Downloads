using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserGroup.DemoProject.WebApp
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket("testticket", true, 100000);

                string str = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                HttpCookie cookie = new HttpCookie("testcookie");
                cookie.Expires = ticket.Expiration;
                cookie.Value = str;

                Response.Cookies.Add(cookie);
            }
            else
            {
                string str = Request.Cookies["testcookie"].Value;
                System.Web.Security.FormsAuthenticationTicket ticket =System.Web.Security.FormsAuthentication.Decrypt(str);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
