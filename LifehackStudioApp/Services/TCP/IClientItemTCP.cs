using System.Net.Sockets;

namespace LifehackStudioApp.Services.TCP
{
    public interface IClientItemTCP
    {
        TcpClient Client { get; set; }
    }
}
