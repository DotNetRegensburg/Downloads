using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace UserGroup.DemoProject.CTL
{
	public static class CriProfileController
	{

		#region standard
		public static BOL.CriProfileInfo GetCriProfileByPK(int userID)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileGetByPK]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			BOL.CriProfileInfo returnItem = new BOL.CriProfileInfo();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetByPK]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetByPK]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetByPK]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = userID;
				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					returnItem.UserID = (int)dr[0];
					returnItem.Username = (string)dr[1];
					returnItem.Firstname = (string)dr[2];
					returnItem.Lastname = (string)dr[3];
					returnItem.Gender = (bool)dr[4];
					returnItem.Birthday = (DateTime)dr[5];
					returnItem.Statement = (string)dr[6];
					returnItem.ZIP = (int)dr[7];
					returnItem.Location = (string)dr[8];
					returnItem.CountryID = (int)dr[9];
					returnItem.OriginLocation = (string)dr[10];
					returnItem.OriginCountryID = (int)dr[11];
					returnItem.Ocupation = (string)dr[12];
					returnItem.ZodiacID = (int)dr[13];
					returnItem.SearchID = (string)dr[14];
					returnItem.SearchIDDescription = (string)dr[15];
					returnItem.LanguagesID = (string)dr[16];
					returnItem.LanguagesIDDescription = (string)dr[17];
					returnItem.Height = (int)dr[18];
					returnItem.Weight = (int)dr[19];
					returnItem.FigureID = (int)dr[20];
					returnItem.SmookerID = (int)dr[21];
					returnItem.HairID = (int)dr[22];
					returnItem.EyesID = (int)dr[23];
					returnItem.PiercingsID = (string)dr[24];
					returnItem.PiercingsIDDescription = (string)dr[25];
					returnItem.TatoosID = (string)dr[26];
					returnItem.TatoosIDDescription = (string)dr[27];
					returnItem.InterestsID = (string)dr[28];
					returnItem.InterestsIDDescription = (string)dr[29];
					returnItem.SportID = (string)dr[30];
					returnItem.SportIDDescription = (string)dr[31];
					returnItem.MusikID = (string)dr[32];
					returnItem.MusikIDDescription = (string)dr[33];
					returnItem.KitchenID = (string)dr[34];
					returnItem.KitchenIDDescription = (string)dr[35];
					returnItem.VotingResult = (decimal)dr[36];
					returnItem.VotingCount = (int)dr[37];
					returnItem.ViewsCount = (int)dr[38];
					returnItem.HasEmailNotification = (bool)dr[39];
					returnItem.HasNewsletterNotification = (bool)dr[40];
					returnItem.LastActivityDate = (DateTime)dr[41];
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

		public static List<BOL.CriProfileInfo> GetAllCriProfile()
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriProfileInfo> returnItem = new List<BOL.CriProfileInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriProfileInfo rowItem = new BOL.CriProfileInfo();

					rowItem.UserID = (int)dr[0];
					rowItem.Username = (string)dr[1];
					rowItem.Firstname = (string)dr[2];
					rowItem.Lastname = (string)dr[3];
					rowItem.Gender = (bool)dr[4];
					rowItem.Birthday = (DateTime)dr[5];
					rowItem.Statement = (string)dr[6];
					rowItem.ZIP = (int)dr[7];
					rowItem.Location = (string)dr[8];
					rowItem.CountryID = (int)dr[9];
					rowItem.OriginLocation = (string)dr[10];
					rowItem.OriginCountryID = (int)dr[11];
					rowItem.Ocupation = (string)dr[12];
					rowItem.ZodiacID = (int)dr[13];
					rowItem.SearchID = (string)dr[14];
					rowItem.SearchIDDescription = (string)dr[15];
					rowItem.LanguagesID = (string)dr[16];
					rowItem.LanguagesIDDescription = (string)dr[17];
					rowItem.Height = (int)dr[18];
					rowItem.Weight = (int)dr[19];
					rowItem.FigureID = (int)dr[20];
					rowItem.SmookerID = (int)dr[21];
					rowItem.HairID = (int)dr[22];
					rowItem.EyesID = (int)dr[23];
					rowItem.PiercingsID = (string)dr[24];
					rowItem.PiercingsIDDescription = (string)dr[25];
					rowItem.TatoosID = (string)dr[26];
					rowItem.TatoosIDDescription = (string)dr[27];
					rowItem.InterestsID = (string)dr[28];
					rowItem.InterestsIDDescription = (string)dr[29];
					rowItem.SportID = (string)dr[30];
					rowItem.SportIDDescription = (string)dr[31];
					rowItem.MusikID = (string)dr[32];
					rowItem.MusikIDDescription = (string)dr[33];
					rowItem.KitchenID = (string)dr[34];
					rowItem.KitchenIDDescription = (string)dr[35];
					rowItem.VotingResult = (decimal)dr[36];
					rowItem.VotingCount = (int)dr[37];
					rowItem.ViewsCount = (int)dr[38];
					rowItem.HasEmailNotification = (bool)dr[39];
					rowItem.HasNewsletterNotification = (bool)dr[40];
					rowItem.LastActivityDate = (DateTime)dr[41];

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

		public static List<BOL.CriProfileInfo> GetAllCriProfileCache()
		{

			object obj = System.Web.HttpContext.Current.Cache["KEY_GetAllCriProfileCache"];
			if (obj != null)
			{
				return (System.Collections.Generic.List<BOL.CriProfileInfo>)obj;
			}


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileGetAll]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriProfileInfo> returnItem = new List<BOL.CriProfileInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileGetAll]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriProfileInfo rowItem = new BOL.CriProfileInfo();

					rowItem.UserID = (int)dr[0];
					rowItem.Username = (string)dr[1];
					rowItem.Firstname = (string)dr[2];
					rowItem.Lastname = (string)dr[3];
					rowItem.Gender = (bool)dr[4];
					rowItem.Birthday = (DateTime)dr[5];
					rowItem.Statement = (string)dr[6];
					rowItem.ZIP = (int)dr[7];
					rowItem.Location = (string)dr[8];
					rowItem.CountryID = (int)dr[9];
					rowItem.OriginLocation = (string)dr[10];
					rowItem.OriginCountryID = (int)dr[11];
					rowItem.Ocupation = (string)dr[12];
					rowItem.ZodiacID = (int)dr[13];
					rowItem.SearchID = (string)dr[14];
					rowItem.SearchIDDescription = (string)dr[15];
					rowItem.LanguagesID = (string)dr[16];
					rowItem.LanguagesIDDescription = (string)dr[17];
					rowItem.Height = (int)dr[18];
					rowItem.Weight = (int)dr[19];
					rowItem.FigureID = (int)dr[20];
					rowItem.SmookerID = (int)dr[21];
					rowItem.HairID = (int)dr[22];
					rowItem.EyesID = (int)dr[23];
					rowItem.PiercingsID = (string)dr[24];
					rowItem.PiercingsIDDescription = (string)dr[25];
					rowItem.TatoosID = (string)dr[26];
					rowItem.TatoosIDDescription = (string)dr[27];
					rowItem.InterestsID = (string)dr[28];
					rowItem.InterestsIDDescription = (string)dr[29];
					rowItem.SportID = (string)dr[30];
					rowItem.SportIDDescription = (string)dr[31];
					rowItem.MusikID = (string)dr[32];
					rowItem.MusikIDDescription = (string)dr[33];
					rowItem.KitchenID = (string)dr[34];
					rowItem.KitchenIDDescription = (string)dr[35];
					rowItem.VotingResult = (decimal)dr[36];
					rowItem.VotingCount = (int)dr[37];
					rowItem.ViewsCount = (int)dr[38];
					rowItem.HasEmailNotification = (bool)dr[39];
					rowItem.HasNewsletterNotification = (bool)dr[40];
					rowItem.LastActivityDate = (DateTime)dr[41];

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

			System.Web.HttpContext.Current.Cache["KEY_GetAllCriProfileCache"]=returnItem;
			return returnItem;

		}

		public static List<BOL.CriProfileInfo> SearchCriProfile(int? userID, string username, string firstname, string lastname, bool? gender, DateTime? birthday, string statement, int? zIP, string location, int? countryID, string originLocation, int? originCountryID, string ocupation, int? zodiacID, string searchID, string searchIDDescription, string languagesID, string languagesIDDescription, int? height, int? weight, int? figureID, int? smookerID, int? hairID, int? eyesID, string piercingsID, string piercingsIDDescription, string tatoosID, string tatoosIDDescription, string interestsID, string interestsIDDescription, string sportID, string sportIDDescription, string musikID, string musikIDDescription, string kitchenID, string kitchenIDDescription, decimal? votingResult, int? votingCount, int? viewsCount, bool? hasEmailNotification, bool? hasNewsletterNotification, DateTime? lastActivityDate)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileSearch]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			List<BOL.CriProfileInfo> returnItem = new List<BOL.CriProfileInfo>();

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileSearch]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileSearch]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileSearch]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				IDataReader dr = command.ExecuteReader();

				while (dr.Read())
				{
					BOL.CriProfileInfo rowItem = new BOL.CriProfileInfo();

					rowItem.UserID = (int)dr[0];
					rowItem.Username = (string)dr[1];
					rowItem.Firstname = (string)dr[2];
					rowItem.Lastname = (string)dr[3];
					rowItem.Gender = (bool)dr[4];
					rowItem.Birthday = (DateTime)dr[5];
					rowItem.Statement = (string)dr[6];
					rowItem.ZIP = (int)dr[7];
					rowItem.Location = (string)dr[8];
					rowItem.CountryID = (int)dr[9];
					rowItem.OriginLocation = (string)dr[10];
					rowItem.OriginCountryID = (int)dr[11];
					rowItem.Ocupation = (string)dr[12];
					rowItem.ZodiacID = (int)dr[13];
					rowItem.SearchID = (string)dr[14];
					rowItem.SearchIDDescription = (string)dr[15];
					rowItem.LanguagesID = (string)dr[16];
					rowItem.LanguagesIDDescription = (string)dr[17];
					rowItem.Height = (int)dr[18];
					rowItem.Weight = (int)dr[19];
					rowItem.FigureID = (int)dr[20];
					rowItem.SmookerID = (int)dr[21];
					rowItem.HairID = (int)dr[22];
					rowItem.EyesID = (int)dr[23];
					rowItem.PiercingsID = (string)dr[24];
					rowItem.PiercingsIDDescription = (string)dr[25];
					rowItem.TatoosID = (string)dr[26];
					rowItem.TatoosIDDescription = (string)dr[27];
					rowItem.InterestsID = (string)dr[28];
					rowItem.InterestsIDDescription = (string)dr[29];
					rowItem.SportID = (string)dr[30];
					rowItem.SportIDDescription = (string)dr[31];
					rowItem.MusikID = (string)dr[32];
					rowItem.MusikIDDescription = (string)dr[33];
					rowItem.KitchenID = (string)dr[34];
					rowItem.KitchenIDDescription = (string)dr[35];
					rowItem.VotingResult = (decimal)dr[36];
					rowItem.VotingCount = (int)dr[37];
					rowItem.ViewsCount = (int)dr[38];
					rowItem.HasEmailNotification = (bool)dr[39];
					rowItem.HasNewsletterNotification = (bool)dr[40];
					rowItem.LastActivityDate = (DateTime)dr[41];

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

		public static bool AddCriProfile(BOL.CriProfileInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileInsert]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileInsert]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileInsert]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileInsert]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.UserID;
				command.Parameters[2].Value = item.Username;
				command.Parameters[3].Value = item.Firstname;
				command.Parameters[4].Value = item.Lastname;
				command.Parameters[5].Value = item.Gender;
				command.Parameters[6].Value = item.Birthday;
				command.Parameters[7].Value = item.Statement;
				command.Parameters[8].Value = item.ZIP;
				command.Parameters[9].Value = item.Location;
				command.Parameters[10].Value = item.CountryID;
				command.Parameters[11].Value = item.OriginLocation;
				command.Parameters[12].Value = item.OriginCountryID;
				command.Parameters[13].Value = item.Ocupation;
				command.Parameters[14].Value = item.ZodiacID;
				command.Parameters[15].Value = item.SearchID;
				command.Parameters[16].Value = item.SearchIDDescription;
				command.Parameters[17].Value = item.LanguagesID;
				command.Parameters[18].Value = item.LanguagesIDDescription;
				command.Parameters[19].Value = item.Height;
				command.Parameters[20].Value = item.Weight;
				command.Parameters[21].Value = item.FigureID;
				command.Parameters[22].Value = item.SmookerID;
				command.Parameters[23].Value = item.HairID;
				command.Parameters[24].Value = item.EyesID;
				command.Parameters[25].Value = item.PiercingsID;
				command.Parameters[26].Value = item.PiercingsIDDescription;
				command.Parameters[27].Value = item.TatoosID;
				command.Parameters[28].Value = item.TatoosIDDescription;
				command.Parameters[29].Value = item.InterestsID;
				command.Parameters[30].Value = item.InterestsIDDescription;
				command.Parameters[31].Value = item.SportID;
				command.Parameters[32].Value = item.SportIDDescription;
				command.Parameters[33].Value = item.MusikID;
				command.Parameters[34].Value = item.MusikIDDescription;
				command.Parameters[35].Value = item.KitchenID;
				command.Parameters[36].Value = item.KitchenIDDescription;
				command.Parameters[37].Value = item.VotingResult;
				command.Parameters[38].Value = item.VotingCount;
				command.Parameters[39].Value = item.ViewsCount;
				command.Parameters[40].Value = item.HasEmailNotification;
				command.Parameters[41].Value = item.HasNewsletterNotification;
				command.Parameters[42].Value = item.LastActivityDate;

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

		public static bool UpdateCriProfile(BOL.CriProfileInfo item)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileUpdate]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileUpdate]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileUpdate]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileUpdate]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = item.UserID;
				command.Parameters[2].Value = item.Username;
				command.Parameters[3].Value = item.Firstname;
				command.Parameters[4].Value = item.Lastname;
				command.Parameters[5].Value = item.Gender;
				command.Parameters[6].Value = item.Birthday;
				command.Parameters[7].Value = item.Statement;
				command.Parameters[8].Value = item.ZIP;
				command.Parameters[9].Value = item.Location;
				command.Parameters[10].Value = item.CountryID;
				command.Parameters[11].Value = item.OriginLocation;
				command.Parameters[12].Value = item.OriginCountryID;
				command.Parameters[13].Value = item.Ocupation;
				command.Parameters[14].Value = item.ZodiacID;
				command.Parameters[15].Value = item.SearchID;
				command.Parameters[16].Value = item.SearchIDDescription;
				command.Parameters[17].Value = item.LanguagesID;
				command.Parameters[18].Value = item.LanguagesIDDescription;
				command.Parameters[19].Value = item.Height;
				command.Parameters[20].Value = item.Weight;
				command.Parameters[21].Value = item.FigureID;
				command.Parameters[22].Value = item.SmookerID;
				command.Parameters[23].Value = item.HairID;
				command.Parameters[24].Value = item.EyesID;
				command.Parameters[25].Value = item.PiercingsID;
				command.Parameters[26].Value = item.PiercingsIDDescription;
				command.Parameters[27].Value = item.TatoosID;
				command.Parameters[28].Value = item.TatoosIDDescription;
				command.Parameters[29].Value = item.InterestsID;
				command.Parameters[30].Value = item.InterestsIDDescription;
				command.Parameters[31].Value = item.SportID;
				command.Parameters[32].Value = item.SportIDDescription;
				command.Parameters[33].Value = item.MusikID;
				command.Parameters[34].Value = item.MusikIDDescription;
				command.Parameters[35].Value = item.KitchenID;
				command.Parameters[36].Value = item.KitchenIDDescription;
				command.Parameters[37].Value = item.VotingResult;
				command.Parameters[38].Value = item.VotingCount;
				command.Parameters[39].Value = item.ViewsCount;
				command.Parameters[40].Value = item.HasEmailNotification;
				command.Parameters[41].Value = item.HasNewsletterNotification;
				command.Parameters[42].Value = item.LastActivityDate;

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

		public static bool DeleteCriProfile(int userID)
		{


			SqlConnection connection = new SqlConnection(GlobalHelper.CONNECTIONSTRING);
			SqlCommand command = new SqlCommand();

			command.CommandText = "[P_CriProfileDelete]";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;

			bool returnItem = true;

			try
			{
				connection.Open();

				if (System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileDelete]"] == null)
				{
					SqlCommandBuilder.DeriveParameters(command);
					SqlParameterCollection paramCollection = command.Parameters;
					SqlParameter[] sqlParam = new SqlParameter[paramCollection.Count];
					paramCollection.CopyTo(sqlParam, 0);
					System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileDelete]"]=sqlParam;
				}
				else
				{
					SqlParameter[] sqlParam = (SqlParameter[])System.Web.HttpContext.Current.Cache["KEYSP_[P_CriProfileDelete]"];
					for (int iPar = 0; iPar < sqlParam.Length; iPar++)
					{
						SqlParameter parItem = (SqlParameter)((ICloneable)sqlParam[iPar]).Clone();
						command.Parameters.Add(parItem);
					}
				}

				command.Parameters[1].Value = userID;

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

