using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteLab.ComputerManagement
{
    public class FakeComputerManagement : IComputerManagement
    {
        public async Task<bool> ConnectToTcpPortAsync(string NetworkAddress, int TcpPort)
        {
            return await Task.Run(() => true);
        }

        public async Task<bool> RebootComputerAsync(string NetworkAddress, string AdminUser, string AdminPassword, string UserDomain)

        {
            return await Task.Run( () => true );
        }
    }
}
