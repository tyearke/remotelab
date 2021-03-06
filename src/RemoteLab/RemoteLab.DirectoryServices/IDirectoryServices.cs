﻿using System;
using System.Collections.Generic;
namespace RemoteLab.DirectoryServices
{
    /// <summary>
    /// Implement this interface if you desire a different method of forms authentication such as LDAP
    /// </summary>
    public interface IDirectoryServices
    {
        bool Authenticate(string UserName, string Password);
        bool UserIsInGroup(string UserName, string GroupName);

        bool GroupExists(string GroupName);
    }
}
