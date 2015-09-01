using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.ComponentModel.Compiler;
using System.Text.RegularExpressions;

namespace ContosoMailManager.Activities
{
	class SendConfirmationEmailValidator : ActivityValidator
	{
        public override ValidationErrorCollection ValidateProperties(ValidationManager manager, object obj)
        {
            if (manager == null)
                throw new ArgumentNullException("manager", "manager is null.");
            if (obj == null)
                throw new ArgumentNullException("obj", "obj is null.");            

            SendConfirmationEmailActivity toBeValidated = obj as SendConfirmationEmailActivity;

            if (toBeValidated == null)
            {
                throw new InvalidOperationException("Parameter obj is not of type SendConfirmationEmailActivity");
            }

            ValidationErrorCollection validationErrors = new ValidationErrorCollection(base.ValidateProperties(manager, obj));

            if (!IsValidEmailAddress(toBeValidated.To))
            {
                ValidationError error =
                    new ValidationError(String.Format("\'{0}\' is an invalid destination e-mail address", toBeValidated.To), 1);

                validationErrors.Add(error);
            }

            if (!IsValidEmailAddress(toBeValidated.From))
            {
                ValidationError error =
                    new ValidationError(String.Format("\'{0}\' is             ender email address.", toBeValidated.From), 1);

                validationErrors.Add(error);
            }

            if (!IsValidUrl(toBeValidated.ConfirmationLink))
            {
                ValidationError error =
                    new ValidationError(String.Format("\'{0}\' is an invalid URL.", toBeValidated.ConfirmationLink), 1);

                validationErrors.Add(error);
            }            

            return validationErrors;
        }

        private bool IsValidEmailAddress(string emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
                return true;

            string regExp = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            return Regex.IsMatch(emailAddress, regExp, RegexOptions.Compiled);            
        }

        private bool IsValidUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
                return true;

            string regExp = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

            return Regex.IsMatch(url, regExp, RegexOptions.Compiled);       
        }
	}
}