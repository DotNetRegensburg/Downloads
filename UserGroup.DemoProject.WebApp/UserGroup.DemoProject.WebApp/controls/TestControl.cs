using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserGroup.DemoProject.WebApp.controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TestControl runat=server></{0}:TestControl>")]
    public class TestControl : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
        
           

            output.Write("<input name=\"ctl00$ContentPlaceHolder1$TextBox1\" type=\"text\" id=\"ctl00_ContentPlaceHolder1_TextBox1\" /> ");
        }
    }
}
