using System;
using System.Threading.Tasks;

namespace RemoteLab.ComputerManagement
{
    public interface IComputerManagement
    {
        Task<bool> ConnectToTcpPortAsync(string NetworkAddress, int TcpPort);
        Task<bool> RebootComputerAsync(string NetworkAddress, string AdminUser, string AdminPassword, string UserDomain);
    }
}
