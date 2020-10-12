using LifehackStudioApp.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LifehackStudioApp.Services.TCP
{
    public class ClientTCP
    {
        private IPAddress _ipAddress;
        private int _port;

        public ClientTCP(string ip, int port)
        {
            if (!IPAddress.TryParse(ip, out _ipAddress))
                throw new ArgumentException("Incorrect IP", nameof(ip));
            if (port < 1024 || port > 65535)
                throw new ArgumentException("Incorrect port", nameof(port));

            _port = port;
        }

        public void Start()
        {
            using (var client = new TcpClient())
            {
                client.Connect(_ipAddress, _port);
                var stream = client.GetStream();

                Packet.BeginRead(stream, (data) => OnDataReaded(stream, data));

                while (true)
                {
                    var message = Console.ReadLine();
                    if (message.ToLower() == "пока")
                        break;
                    var packet = new Packet(message);
                    packet.WriteOnStream(stream);
                }

                client.Close();
            }
        }

        private void OnDataReaded(NetworkStream stream, byte[] data)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data));
            Packet.BeginRead(stream, (data) => OnDataReaded(stream, data));
        }
    }
}
