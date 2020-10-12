using LifehackStudioApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifehackStudioApp.Services.TCP
{
    public class ClientListener : IClientListener
    {
        private volatile List<IClientItemTCP> _clientItems = new List<IClientItemTCP>();
        private bool _running;

        public event Action<IClientItemTCP, string> OnMessageSent;
        public event Action<IClientItemTCP> OnClientDisconnected;

        public void StartListening()
        {
            if (_running)
                return;
            _running = true;

            Task.Run(() =>
            {
                while (_running)
                {
                    for (int i = 0; i < _clientItems.Count; i++)
                    {
                        var client = _clientItems[i]?.Client;
                        if (client == null)
                            continue;

                        if (!client.Connected)
                        {
                            OnClientDisconnected?.Invoke(_clientItems[i]);
                            continue;
                        }

                        var stream = client.GetStream();
                        if (stream != null && !stream.DataAvailable)
                            continue;

                        var message = Encoding.UTF8.GetString(Packet.ReadFromStream(stream));
                        OnMessageSent?.Invoke(_clientItems[i], message);
                    }
                }
            });
        }

        public void StopListening()
        {
            _running = false;
        }

        public void AddClient(IClientItemTCP client)
        {
            _clientItems.Add(client);
        }

        public void RemoveClient(IClientItemTCP client)
        {
            _clientItems.Remove(client);
        }
    }
}
