using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace Contoso.MailManager.Services
{
    public class EmailDataService : IEmailDataService
    {
        public event EventHandler<System.Workflow.Activities.ExternalDataEventArgs> ConfirmationReceived;
        
        public void InsertSubscription(string email, Guid id)
        {
            string sql = "INSERT INTO tblSubscriptions (SubscriptionID, EmailAdress,SubscriptionStatus) VALUES (@SubscriptionID,@EmailAddress,@SubscriptionStatus)";
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MailManagerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("SubscriptionID", SqlDbType.UniqueIdentifier);
                    cmd.Parameters[0].Value = id;
                    cmd.Parameters.Add("EmailAddress", SqlDbType.NVarChar);
                    cmd.Parameters[1].Value = email;
                    cmd.Parameters.Add("SubscriptionStatus", SqlDbType.Int);
                    cmd.Parameters[2].Value = 1;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }                
            }
        }        

        public void ConfirmSubscription(Guid id)
        {
            string sql = "UPDATE tblSubscriptions SET SubscriptionStatus = 2 WHERE (SubscriptionID = @SubscriptionID)";

            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MailManagerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("SubscriptionID", SqlDbType.UniqueIdentifier);
                    cmd.Parameters[0].Value = id;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public void RemoveSubscription(string email)
        {
            string sql = "DELETE FROM tblSubscriptions WHERE (EmailAdress = @EmailAddress)";
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MailManagerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("EmailAddress", SqlDbType.NVarChar);
                    cmd.Parameters[0].Value = email;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public void RaiseConfirmationReceived(Guid id)
        {
            //Create the event args, passing the wf instance id as an argument
            ExternalDataEventArgs args = new ExternalDataEventArgs(id);

            //Raise the event
            this.OnConfirmationReceived(args);
        }

        protected virtual void OnConfirmationReceived(ExternalDataEventArgs e)
        {
            if (this.ConfirmationReceived != null)
            {
                this.ConfirmationReceived(null, e);
            }
        }
    }
}
