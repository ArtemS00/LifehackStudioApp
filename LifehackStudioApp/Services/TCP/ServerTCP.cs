using LifehackStudioApp.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LifehackStudioApp.Services.TCP
{
    public class ServerTCP
    {
        private IPAddress _ipAddress;
        private int _port;
        private IClientListener _clientListener;
        private IMessageManager _manager;

        private int _lastId;

        public ServerTCP(string ip, int port, IClientListener clientListener, IMessageManager manager)
        {
            if (!IPAddress.TryParse(ip, out _ipAddress))
                throw new ArgumentException("Incorrect IP", nameof(ip));
            if (port < 1024 || port > 65535)
                throw new ArgumentException("Incorrect port", nameof(port));
            if (clientListener == null)
                throw new ArgumentNullException(nameof(clientListener));
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            _port = port;
            _clientListener = clientListener;
            _manager = manager;
        }

        public void Start(CancellationToken cancellationToken = new CancellationToken())
        {
            // Create server and start listening
            TcpListener server = new TcpListener(_ipAddress, _port);
            server.Start();
            _clientListener.OnMessageSent += OnMessageSent;
            _clientListener.OnClientDisconnected += OnClientDisconnected;
            _clientListener.StartListening();

            // On client connected
            server.BeginAcceptTcpClient((result) 
                => OnClientConnected(server, result, cancellationToken), null);
            
            // On cancellation called
            cancellationToken.Register(() =>
            {
                _clientListener.StopListening();
                server.Stop();
            });
        }

        private void OnClientDisconnected(IClientItemTCP client)
        {
            _manager.Disconnected(client as IClient);
        }

        private void OnClientConnected(TcpListener server, IAsyncResult r, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            // Create and add client item
            TcpClient client = server.EndAcceptTcpClient(r);
            var clientItem = new ClientItemTCP()
            {
                Id = ++_lastId,
                Client = client
            };
            _clientListener.AddClient(clientItem);

            // Get and send on connected message
            var packet = new Packet(_manager.Connected(clientItem));
            packet.WriteOnStream(client.GetStream());

            // Accept next client
            server.BeginAcceptTcpClient((result)
                => OnClientConnected(server, result, token),
                null);
        }

        private void OnMessageSent(IClientItemTCP client, string message)
        {
            var responce = _manager.RespondTo(client as IClient, message);
            var packet = new Packet(responce);
            packet.WriteOnStream(client.Client.GetStream());
        }
    }
}
