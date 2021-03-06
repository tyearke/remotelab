// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace RemoteLab.DependencyResolution {
    using RemoteLab.ComputerManagement;
    using RemoteLab.DirectoryServices;
    using RemoteLab.Models;
    using RemoteLab.Services;
    using RemoteLab.Utilities;
    using StructureMap;
    using StructureMap.Graph;
    using StructureMap.Web;
    using StructureMap.Web.Pipeline;
    using System.Web;
    using System.Web.Mvc;
	
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
                x.For<IDirectoryServices>().Use(ctx => new ActiveDirectory(Properties.Settings.Default.ActiveDirectoryDNSDomain));
                x.For<PasswordUtility>().Use(ctx => new PasswordUtility(Properties.Settings.Default.EncryptionKeyForPasswords));
                x.For<RemoteLabContext>().Use(ctx => new RemoteLabContext("RemoteLabContext"));
#if (DEBUG)
//                            x.For<IComputerManagement>().Use(ctx => new FakeComputerManagement()); 
                            x.For<IComputerManagement>().Use(ctx => new WindowsComputerManagement(Elmah.ErrorLog.GetDefault(HttpContext.Current))); 
#else
                x.For<IComputerManagement>().Use(ctx => new WindowsComputerManagement(Elmah.ErrorLog.GetDefault(HttpContext.Current)));
#endif
                x.For<SmtpEmail>().Use(ctx => new SmtpEmail());
                x.For<RemoteLabService>().Use(ctx => new RemoteLabService(
                    ctx.GetInstance<RemoteLabContext>(),
                    ctx.GetInstance<IComputerManagement>(),
                    ctx.GetInstance<SmtpEmail>(),
                    ctx.GetInstance<PasswordUtility>()
                    ));
            });
            return ObjectFactory.Container;

        
        }
    }
}