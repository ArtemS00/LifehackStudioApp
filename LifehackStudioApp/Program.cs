using LifehackStudioApp.Services;
using LifehackStudioApp.Services.RespondCases;
using LifehackStudioApp.Services.TCP;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LifehackStudioApp
{
    public class Program
    {
        private const int port = 8888;
        private const string ip = "127.0.0.1";
        static void Main(string[] args)
        {
            var manager = CreateMessageManager();

            // Create and run server thread
            var serverThread = new Thread(new ThreadStart(() => new ServerTCP(ip, port, new ClientListener(), manager).Start()));
            serverThread.Start();

            // Create and run client thread
            RunNewClient();
        }

        static void RunNewClient()
        {
            var clientThread = new Thread(new ThreadStart(() => new ClientTCP(ip, port).Start()));
            clientThread.Start();
        }

        static IMessageManager CreateMessageManager()
        {
            var manager = new MessageManager();
            manager.AddCase(new SimpleRespondCase("Привет", (c, m) => $"Привет, {c.Name}!"));
            manager.AddCase(new SimpleRespondCase("Как дела?", (c, m) => $"Отлично!"));
            manager.AddCase(new SimpleRespondCase("Сколько времени?", (c, m) => $"Время {DateTime.Now.ToShortTimeString()}"));
            manager.AddCase(new SimpleRespondCase("Покажи всех клиентов",
                (c, m) => String.Join('\n', manager.Clients.Select(c => $"[{c.Id}] {c.Name}"))));
            manager.AddCase(new RandomRespondCase());
            return manager;
        }
    }
}
