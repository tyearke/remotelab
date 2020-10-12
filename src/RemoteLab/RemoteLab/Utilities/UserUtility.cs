using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RemoteLab.Utilities
{
    public static class UserUtility
    {
        private static readonly Regex UserComponentsRegex = new Regex(@"(?:(?<domain>[^\\]+)\\)?(?<username>.*)", RegexOptions.Compiled);

        public static IDictionary<string, string> GetUserComponents(string user)
        {
            var userComponentsGroups = UserComponentsRegex.Match(user).Groups;
            var userComponents = new Dictionary<string, string>
            {
                { "username", userComponentsGroups["username"].Value },
            };

            var domainGroup = userComponentsGroups["domain"];
            userComponents["domain"] =
            (
                domainGroup.Success
                ? domainGroup.Value
                : Properties.Settings.Default.ActiveDirectoryDomain
            );

            return userComponents;
        }
    }
}