using System;

namespace UserGroup.DemoProject.BOL
{
	public class CriProfileInfo:IReturnObject
	{
		public int UserID = 0;
		public string Username = string.Empty;
		public string Firstname = string.Empty;
		public string Lastname = string.Empty;
		public bool Gender = false;
		public DateTime Birthday;
		public string Statement = string.Empty;
		public int ZIP = 0;
		public string Location = string.Empty;
		public int CountryID = 0;
		public string OriginLocation = string.Empty;
		public int OriginCountryID = 0;
		public string Ocupation = string.Empty;
		public int ZodiacID = 0;
		public string SearchID = string.Empty;
		public string SearchIDDescription = string.Empty;
		public string LanguagesID = string.Empty;
		public string LanguagesIDDescription = string.Empty;
		public int Height = 0;
		public int Weight = 0;
		public int FigureID = 0;
		public int SmookerID = 0;
		public int HairID = 0;
		public int EyesID = 0;
		public string PiercingsID = string.Empty;
		public string PiercingsIDDescription = string.Empty;
		public string TatoosID = string.Empty;
		public string TatoosIDDescription = string.Empty;
		public string InterestsID = string.Empty;
		public string InterestsIDDescription = string.Empty;
		public string SportID = string.Empty;
		public string SportIDDescription = string.Empty;
		public string MusikID = string.Empty;
		public string MusikIDDescription = string.Empty;
		public string KitchenID = string.Empty;
		public string KitchenIDDescription = string.Empty;
		public decimal VotingResult = 0;
		public int VotingCount = 0;
		public int ViewsCount = 0;
		public bool HasEmailNotification = false;
		public bool HasNewsletterNotification = false;
		public DateTime LastActivityDate;
	}
}

