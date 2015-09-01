using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Activities;

namespace Contoso.MailManager.Services
{
    [ExternalDataExchange]
	public interface IEmailDataService
	{
        event EventHandler<ExternalDataEventArgs> ConfirmationReceived;

        void InsertSubscription(string email, Guid id);
        void RemoveSubscription(string email);
        void ConfirmSubscription(Guid id);        
	}
}