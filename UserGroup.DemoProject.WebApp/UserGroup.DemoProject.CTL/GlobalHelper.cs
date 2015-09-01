using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;

namespace UserGroup.DemoProject.CTL
{
	public static class GlobalHelper
	{
		internal static string CONNECTIONSTRING = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString;

		public static SqlTransaction InitTransaction()
		{
			SqlConnection connection = new SqlConnection(CONNECTIONSTRING);
			return connection.BeginTransaction();
		}
		public static void LogException(Exception ex)
		{
			log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
			log4net.ILog log = log4net.LogManager.GetLogger("IntranetAnwendungLOG");
			log.Error(string.Empty, ex);

			#if DEBUG
				System.Web.HttpContext.Current.Response.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture,"<div style='top:200px;width:600px; position: absolute; z-index: 2; border: 1pt solid red; background-color: #FFCC66'><b>{0}</b><BR/><BR/>{1}</div>", ex.Message, ex.ToString()));
			#endif

		}
	}
}

