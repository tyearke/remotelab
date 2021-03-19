using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RemoteLab.CustomValidators
{
    public class NoDomainPrefixAttribute : ValidationAttribute
    {
        public NoDomainPrefixAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            String stringValue = (String)value;
            String userNameLabel = RemoteLab.Properties.Settings.Default.LoginFormUsernameLabel;
            String defaultDomain = RemoteLab.Properties.Settings.Default.ActiveDirectoryDomain;
            if (stringValue.Contains(@"\"))
            {
                return new ValidationResult(String.Format(
                    @"Please enter your {0} without a domain prefix. (e.g. {1}, not {2}\{1})",
                    userNameLabel,
                    userNameLabel.ToLower(),
                    defaultDomain
                ));
            }
            return null;
        }
    }
}