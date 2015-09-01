using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace UserGroup.DemoProject.CTL
{
	public static class MultiSP
	{


		#region userdef
		public static List<List<BOL.IReturnObject>> GetMulti(int? ownerId_SelectTesttable, int? typeId_SelectTesttable)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_GetMultiMulti]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<List<BOL.IReturnObject>> returnItem=new List<List<BOL.IReturnObject>>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_GetMultiMulti]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_GetMultiMulti]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_GetMultiMulti]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = ownerId_SelectTesttable;

				command.Parameters[2].Value = typeId_SelectTesttable;


				IDataReader dr = command.ExecuteReader();

				List<BOL.IReturnObject> rowItemListTesttableInfo = new List<BOL.IReturnObject>();
				while (dr.Read())
				{
					BOL.TesttableInfo rowItem = new BOL.TesttableInfo();

					rowItem.PKINcr = (int)dr[0];
					rowItem.PK1 = (Guid)dr[1];
					rowItem.OwnerId = (int)dr[2];
					rowItem.TypeId = (int)dr[3];
					rowItem.Value = (string)dr[4];
					if (dr[5] == DBNull.Value) rowItem.Data = null;
					else rowItem.Data = (DateTime?)dr[5];
					rowItem.incr = (int)dr[6];
					if (dr[7] == DBNull.Value) rowItem.test = null;
					else rowItem.test = (string)dr[7];

					rowItemListTesttableInfo.Add(rowItem);
				}
				returnItem.Add(rowItemListTesttableInfo);
				dr.NextResult();

				rowItemListTesttableInfo = new List<BOL.IReturnObject>();
				while (dr.Read())
				{
					BOL.TesttableInfo rowItem = new BOL.TesttableInfo();

					rowItem.PKINcr = (int)dr[0];
					rowItem.PK1 = (Guid)dr[1];
					rowItem.OwnerId = (int)dr[2];
					rowItem.TypeId = (int)dr[3];
					rowItem.Value = (string)dr[4];
					if (dr[5] == DBNull.Value) rowItem.Data = null;
					else rowItem.Data = (DateTime?)dr[5];
					rowItem.incr = (int)dr[6];
					if (dr[7] == DBNull.Value) rowItem.test = null;
					else rowItem.test = (string)dr[7];

					rowItemListTesttableInfo.Add(rowItem);
				}
				returnItem.Add(rowItemListTesttableInfo);
				dr.NextResult();



			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				GlobalHelper.LogException(ex);
				return null;

			}
			catch (Exception ex)
			{
				GlobalHelper.LogException(ex);
				return null;

			}
			finally
			{
				if (connection.State == ConnectionState.Open)
					connection.Close();

				connection.Dispose();
				command.Dispose();
			}

			return returnItem;

		}


		#endregion
	}
}

