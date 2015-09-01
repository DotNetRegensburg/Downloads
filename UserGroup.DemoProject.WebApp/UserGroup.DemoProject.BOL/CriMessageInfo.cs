using System;

namespace UserGroup.DemoProject.BOL
{
	public class CriMessageInfo:IReturnObject
	{
		public int MessageID = 0;
		public int UserID = 0;
		public string UserIDDescription = string.Empty;
		public int UserIDReceiver = 0;
		public string UserIDReceiverDescription = string.Empty;
		public int StatusID = 0;
		public DateTime? AnsweredAt;
		public DateTime SendAt;
		public string Subject = string.Empty;
		public string SubjectShort = string.Empty;
		public string PostText = string.Empty;
		public DateTime? ReadAt;
		public bool IsAktivSender = false;
		public bool IsAktivReceiver = false;
	}
}

