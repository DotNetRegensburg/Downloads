using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace UserGroup.DemoProject.CTL
{
	public static class CriMessageController
	{

		#region standard
		public static BOL.CriMessageInfo GetCriMessageByPK(int messageID, int userID)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageGetByPK]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			BOL.CriMessageInfo returnItem = new BOL.CriMessageInfo();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetByPK]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetByPK]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetByPK]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = messageID;
				command.Parameters[2].Value = userID;
				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					returnItem.MessageID = (int)dr[0];
					returnItem.UserID = (int)dr[1];
					returnItem.UserIDDescription = (string)dr[2];
					returnItem.UserIDReceiver = (int)dr[3];
					returnItem.UserIDReceiverDescription = (string)dr[4];
					returnItem.StatusID = (int)dr[5];
					if (dr[6] == DBNull.Value) returnItem.AnsweredAt = null;
					else returnItem.AnsweredAt = (DateTime?)dr[6];
					returnItem.SendAt = (DateTime)dr[7];
					returnItem.Subject = (string)dr[8];
					returnItem.SubjectShort = (string)dr[9];
					returnItem.PostText = (string)dr[10];
					if (dr[11] == DBNull.Value) returnItem.ReadAt = null;
					else returnItem.ReadAt = (DateTime?)dr[11];
					returnItem.IsAktivSender = (bool)dr[12];
					returnItem.IsAktivReceiver = (bool)dr[13];
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

		public static List<BOL.CriMessageInfo> GetAllCriMessage()
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriMessageInfo> returnItem = new List<BOL.CriMessageInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriMessageInfo rowItem = new BOL.CriMessageInfo();

					rowItem.MessageID = (int)dr[0];
					rowItem.UserID = (int)dr[1];
					rowItem.UserIDDescription = (string)dr[2];
					rowItem.UserIDReceiver = (int)dr[3];
					rowItem.UserIDReceiverDescription = (string)dr[4];
					rowItem.StatusID = (int)dr[5];
					if (dr[6] == DBNull.Value) rowItem.AnsweredAt = null;
					else rowItem.AnsweredAt = (DateTime?)dr[6];
					rowItem.SendAt = (DateTime)dr[7];
					rowItem.Subject = (string)dr[8];
					rowItem.SubjectShort = (string)dr[9];
					rowItem.PostText = (string)dr[10];
					if (dr[11] == DBNull.Value) rowItem.ReadAt = null;
					else rowItem.ReadAt = (DateTime?)dr[11];
					rowItem.IsAktivSender = (bool)dr[12];
					rowItem.IsAktivReceiver = (bool)dr[13];

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

		public static List<BOL.CriMessageInfo> GetAllCriMessageCache()
		{

			object obj = System.Web.HttpContext.Current.Cache["KEY_GetAllCriMessageCache"];
			if (obj != null)
			{
				return (System.Collections.Generic.List<BOL.CriMessageInfo>)obj;
			}


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriMessageInfo> returnItem = new List<BOL.CriMessageInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriMessageInfo rowItem = new BOL.CriMessageInfo();

					rowItem.MessageID = (int)dr[0];
					rowItem.UserID = (int)dr[1];
					rowItem.UserIDDescription = (string)dr[2];
					rowItem.UserIDReceiver = (int)dr[3];
					rowItem.UserIDReceiverDescription = (string)dr[4];
					rowItem.StatusID = (int)dr[5];
					if (dr[6] == DBNull.Value) rowItem.AnsweredAt = null;
					else rowItem.AnsweredAt = (DateTime?)dr[6];
					rowItem.SendAt = (DateTime)dr[7];
					rowItem.Subject = (string)dr[8];
					rowItem.SubjectShort = (string)dr[9];
					rowItem.PostText = (string)dr[10];
					if (dr[11] == DBNull.Value) rowItem.ReadAt = null;
					else rowItem.ReadAt = (DateTime?)dr[11];
					rowItem.IsAktivSender = (bool)dr[12];
					rowItem.IsAktivReceiver = (bool)dr[13];

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

			System.Web.HttpContext.Current.Cache["KEY_GetAllCriMessageCache"]=returnItem;
			return returnItem;

		}

		public static List<BOL.CriMessageInfo> SearchCriMessage(int? messageID, int? userID, string userIDDescription, int? userIDReceiver, string userIDReceiverDescription, int? statusID, DateTime? answeredAt, DateTime? sendAt, string subject, string subjectShort, string postText, DateTime? readAt, bool? isAktivSender, bool? isAktivReceiver)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageSearch]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriMessageInfo> returnItem = new List<BOL.CriMessageInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageSearch]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageSearch]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageSearch]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriMessageInfo rowItem = new BOL.CriMessageInfo();

					rowItem.MessageID = (int)dr[0];
					rowItem.UserID = (int)dr[1];
					rowItem.UserIDDescription = (string)dr[2];
					rowItem.UserIDReceiver = (int)dr[3];
					rowItem.UserIDReceiverDescription = (string)dr[4];
					rowItem.StatusID = (int)dr[5];
					if (dr[6] == DBNull.Value) rowItem.AnsweredAt = null;
					else rowItem.AnsweredAt = (DateTime?)dr[6];
					rowItem.SendAt = (DateTime)dr[7];
					rowItem.Subject = (string)dr[8];
					rowItem.SubjectShort = (string)dr[9];
					rowItem.PostText = (string)dr[10];
					if (dr[11] == DBNull.Value) rowItem.ReadAt = null;
					else rowItem.ReadAt = (DateTime?)dr[11];
					rowItem.IsAktivSender = (bool)dr[12];
					rowItem.IsAktivReceiver = (bool)dr[13];

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

		public static int AddCriMessage(BOL.CriMessageInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageInsert]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			int returnItem = 0;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageInsert]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageInsert]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageInsert]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.UserID;
				command.Parameters[2].Value = item.UserIDDescription;
				command.Parameters[3].Value = item.UserIDReceiver;
				command.Parameters[4].Value = item.UserIDReceiverDescription;
				command.Parameters[5].Value = item.StatusID;
				command.Parameters[6].Value = item.AnsweredAt;
				command.Parameters[7].Value = item.SendAt;
				command.Parameters[8].Value = item.Subject;
				command.Parameters[9].Value = item.SubjectShort;
				command.Parameters[10].Value = item.PostText;
				command.Parameters[11].Value = item.ReadAt;
				command.Parameters[12].Value = item.IsAktivSender;
				command.Parameters[13].Value = item.IsAktivReceiver;

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

		public static bool UpdateCriMessage(BOL.CriMessageInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageUpdate]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageUpdate]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageUpdate]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageUpdate]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.MessageID;
				command.Parameters[2].Value = item.UserID;
				command.Parameters[3].Value = item.UserIDDescription;
				command.Parameters[4].Value = item.UserIDReceiver;
				command.Parameters[5].Value = item.UserIDReceiverDescription;
				command.Parameters[6].Value = item.StatusID;
				command.Parameters[7].Value = item.AnsweredAt;
				command.Parameters[8].Value = item.SendAt;
				command.Parameters[9].Value = item.Subject;
				command.Parameters[10].Value = item.SubjectShort;
				command.Parameters[11].Value = item.PostText;
				command.Parameters[12].Value = item.ReadAt;
				command.Parameters[13].Value = item.IsAktivSender;
				command.Parameters[14].Value = item.IsAktivReceiver;

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

		public static bool DeleteCriMessage(int messageID, int userID)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriMessageDelete]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageDelete]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageDelete]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriMessageDelete]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = messageID;
				command.Parameters[2].Value = userID;

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

		#endregion
	}
}

