using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace UserGroup.DemoProject.CTL
{
	public static class TesttableController
	{

		#region standard
		public static BOL.TesttableInfo GetTesttableByPK(int pKINcr)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableGetByPK]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			BOL.TesttableInfo returnItem = new BOL.TesttableInfo();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetByPK]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetByPK]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetByPK]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = pKINcr;
				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					returnItem.PKINcr = (int)dr[0];
					returnItem.PK1 = (Guid)dr[1];
					returnItem.OwnerId = (int)dr[2];
					returnItem.TypeId = (int)dr[3];
					returnItem.Value = (string)dr[4];
					if (dr[5] == DBNull.Value) returnItem.Data = null;
					else returnItem.Data = (DateTime?)dr[5];
					returnItem.incr = (int)dr[6];
					if (dr[7] == DBNull.Value) returnItem.test = null;
					else returnItem.test = (string)dr[7];
				}



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

		public static List<BOL.TesttableInfo> GetAllTesttable()
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

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

					returnItem.Add(rowItem);
				}



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

		public static List<BOL.TesttableInfo> GetAllTesttableCache()
		{

			object obj = System.Web.HttpContext.Current.Cache["KEY_GetAllTesttableCache"];
			if (obj != null)
			{
				return (System.Collections.Generic.List<BOL.TesttableInfo>)obj;
			}


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

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

					returnItem.Add(rowItem);
				}



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

			System.Web.HttpContext.Current.Cache["KEY_GetAllTesttableCache"]=returnItem;
			return returnItem;

		}

		public static List<BOL.TesttableInfo> SearchTesttable(int? pKINcr, Guid? pK1, int? ownerId, int? typeId, string value, DateTime? data, int? incr, string test)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableSearch]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableSearch]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableSearch]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableSearch]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

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

					returnItem.Add(rowItem);
				}



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

		public static int AddTesttable(BOL.TesttableInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableInsert]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			int returnItem = 0;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableInsert]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableInsert]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableInsert]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.PK1;
				command.Parameters[2].Value = item.OwnerId;
				command.Parameters[3].Value = item.TypeId;
				command.Parameters[4].Value = item.Value;
				command.Parameters[5].Value = item.Data;
				command.Parameters[6].Value = item.incr;
				command.Parameters[7].Value = item.test;

				returnItem = Convert.ToInt32(command.ExecuteScalar());



			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				GlobalHelper.LogException(ex);
				return 0;

			}
			catch (Exception ex)
			{
				GlobalHelper.LogException(ex);
				return 0;

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

		public static bool UpdateTesttable(BOL.TesttableInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableUpdate]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableUpdate]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableUpdate]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableUpdate]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.PKINcr;
				command.Parameters[2].Value = item.PK1;
				command.Parameters[3].Value = item.OwnerId;
				command.Parameters[4].Value = item.TypeId;
				command.Parameters[5].Value = item.Value;
				command.Parameters[6].Value = item.Data;
				command.Parameters[7].Value = item.incr;
				command.Parameters[8].Value = item.test;

				command.ExecuteNonQuery();



			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				GlobalHelper.LogException(ex);
				return false;

			}
			catch (Exception ex)
			{
				GlobalHelper.LogException(ex);
				return false;

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

		public static bool DeleteTesttable(int pKINcr)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_TesttableDelete]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableDelete]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableDelete]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_TesttableDelete]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = pKINcr;

				command.ExecuteNonQuery();



			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				GlobalHelper.LogException(ex);
				return false;

			}
			catch (Exception ex)
			{
				GlobalHelper.LogException(ex);
				return false;

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

		#region userdef
		public static List<BOL.TesttableInfo> GetTopAsBO()
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_GetTopAsBOStandardSelect]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsBOStandardSelect]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsBOStandardSelect]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsBOStandardSelect]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
                
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

                IDataReader dr = command.ExecuteReader(CommandBehavior.SequentialAccess);

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

					returnItem.Add(rowItem);
				}



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

		public static DataTable GetTopAsTable()
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_GetTopAsTableStandardSelect]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			DataTable returnItem = new DataTable();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsTableStandardSelect]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsTableStandardSelect]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_GetTopAsTableStandardSelect]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

               
				returnItem.Load(command.ExecuteReader());



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

		public static List<BOL.TesttableInfo> GetByOwnerId(int ownerId)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_GetByOwnerIdStandardSelect]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByOwnerIdStandardSelect]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByOwnerIdStandardSelect]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByOwnerIdStandardSelect]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = ownerId;
				IDataReader dr = command.ExecuteReader();

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

					returnItem.Add(rowItem);
				}



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

		public static List<BOL.TesttableInfo> GetByTypeId(int typeId)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_GetByTypeIdStandardSelect]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.TesttableInfo> returnItem = new List<BOL.TesttableInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByTypeIdStandardSelect]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByTypeIdStandardSelect]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_GetByTypeIdStandardSelect]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = typeId;
				IDataReader dr = command.ExecuteReader();

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

					returnItem.Add(rowItem);
				}



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

