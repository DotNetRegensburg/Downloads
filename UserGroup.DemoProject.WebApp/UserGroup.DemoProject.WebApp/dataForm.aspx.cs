using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserGroup.DemoProject.WebApp
{
    public partial class dataForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();


            //for (int i = 0; i < 10; i++)
            //{
            //    List<BOL.TesttableInfo> lst = CTL.TesttableController.GetTopAsBO();
            //}
            //sw.Stop();
            //Response.Write(sw.Elapsed.TotalSeconds+"<BR/>");


            //sw = System.Diagnostics.Stopwatch.StartNew();

            //for (int i = 0; i < 10; i++)
            //{
            //    System.Data.DataTable lst = CTL.TesttableController.GetTopAsTable();
            //}
            //sw.Stop();
            //Response.Write(sw.Elapsed.TotalSeconds+"<br/>");



            sw = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                List<BOL.TesttableInfo> lst1 = CTL.TesttableController.GetByOwnerId(2);
                List<BOL.TesttableInfo> lst2 = CTL.TesttableController.GetByTypeId(3);

            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds + "<br/>");


            sw = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                List<List<BOL.IReturnObject>> lst1 = CTL.MultiSP.GetMulti(2, 3);
                List<BOL.TesttableInfo> lst = lst1[0].Cast<BOL.TesttableInfo>().ToList();

            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds + "<br/>");
        }
    }
}
