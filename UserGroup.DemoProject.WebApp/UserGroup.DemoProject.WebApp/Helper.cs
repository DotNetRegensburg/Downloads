using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace UserGroup.DemoProject.WebApp
{
    public class Helper
    {
       Random rnd = new Random();

        public Helper()
        {

        }

        public string RandomString(int minSize, int maxSize)
        {


                StringBuilder builder = new StringBuilder();

                char ch;
                int size = rnd.Next(minSize, maxSize);

                for (int i = 0; i < size; i++)
                {
                    bool bo = Convert.ToBoolean(rnd.Next(0, 2));
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rnd.NextDouble() + 65)));
                    if (bo)
                        builder.Append(ch.ToString().ToLower());
                    else
                        builder.Append(ch.ToString().ToUpper());
                }


                return string.Concat("'", builder.ToString(), "'");
            

        }
    }
}
