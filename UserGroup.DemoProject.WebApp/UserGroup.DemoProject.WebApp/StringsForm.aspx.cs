using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace UserGroup.DemoProject.WebApp
{
    public partial class StringsForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int iCount = 100000;
            //5 Elemente
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            //Einfach

            for (int i = 0; i < iCount; i++)
            {
                string sEnd = string.Empty;
                sEnd = sEnd + "test1";
                sEnd = sEnd + "test2";
                sEnd = sEnd + "test3";
                sEnd = sEnd + "test4";
                sEnd = sEnd + "test5";
                sEnd = sEnd + "test6";
                sEnd = sEnd + "test7";
                sEnd = sEnd + "test8";
            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds+"-Simple<BR/>");

            //Format
            sw = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < iCount; i++)
            {
                string sForm = string.Empty;
                sForm = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", "test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8");
            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds + "-Format<BR/>");

            //stringbuilder
            sw = System.Diagnostics.Stopwatch.StartNew();


            for (int i = 0; i < iCount; i++)
            {
                StringBuilder sb1 = new StringBuilder();
                sb1.Append("test1");
                sb1.Append("test2");
                sb1.Append("test3");
                sb1.Append("test4");
                sb1.Append("test5");
                sb1.Append("test6");
                sb1.Append("test7");
                sb1.Append("test8");
            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds+"-SB<BR/>");

     

            //concat
            sw = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < iCount; i++)
            {
                string sCon = string.Empty;
                sCon = string.Concat("test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8");
            }
            sw.Stop();
            Response.Write(sw.Elapsed.TotalSeconds + "-Concat<BR/>");

           
        }
    }
}
