using System.Net.Sockets;

namespace LifehackStudioApp.Services.TCP
{
    public class ClientItemTCP : BaseClient, IClientItemTCP
    {
        public TcpClient Client { get; set; }
    }
}
