using System.Net.Sockets;
using System.Text;

namespace LifehackStudioApp
{
    public static class Extensions
    {
        public static string ReadAll(this NetworkStream stream)
        {
            StringBuilder data = new StringBuilder();
            byte[] buffer = new byte[1024];
            while (stream.DataAvailable)
            {
                int count = stream.Read(buffer, 0, buffer.Length);
                data.Append(Encoding.UTF8.GetString(buffer, 0, count));
            };
            return data.ToString();
        }

        public static void Send(this NetworkStream stream, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            stream.Flush();
            stream.Write(data, 0, data.Length);
        }
    }
}
