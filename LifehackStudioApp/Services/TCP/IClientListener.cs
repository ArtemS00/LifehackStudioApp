using System;

namespace LifehackStudioApp.Services.TCP
{
    public interface IClientListener
    {
        void StartListening();
        void StopListening();
        void AddClient(IClientItemTCP client);
        void RemoveClient(IClientItemTCP client);
        event Action<IClientItemTCP, string> OnMessageSent;
        event Action<IClientItemTCP> OnClientDisconnected;
    }
}
