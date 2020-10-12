using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace LifehackStudioApp.Models
{
    /// <summary>
    /// Class for sending data through streams
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// Number of bytes which will contain sending data size
        /// </summary>
        private const int _countBufferSize = 4;

        private byte[] _data;

        public Packet(string data) 
            : this(Encoding.UTF8.GetBytes(data)) { }

        public Packet(byte[] data)
        {
            _data = GetValueInBytes(data.Length, _countBufferSize)
                .Concat(data)
                .ToArray();
        }

        public void WriteOnStream(Stream stream)
        {
            stream.Write(_data);
        }

        public static byte[] ReadFromStream(Stream stream)
        {
            // Gets data size
            var sizeBytes = new byte[_countBufferSize];
            stream.Read(sizeBytes, 0, sizeBytes.Length);
            int bufferSize = GetValueFromBytes(sizeBytes);

            // Reads data
            var buffer = new byte[bufferSize];
            stream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public static void BeginRead(NetworkStream stream, Action<byte[]> callback)
        {
            var sizeBytes = new byte[_countBufferSize];
            stream.BeginRead(sizeBytes, 0, sizeBytes.Length, (result) => OnBeginRead(stream, sizeBytes, result, callback), null);
        }

        private static void OnBeginRead(NetworkStream stream, byte[] sizeBytes, IAsyncResult result, Action<byte[]> callback)
        {
            // Gets data size
            int size = GetValueFromBytes(sizeBytes);

            // Reads data
            byte[] buffer = new byte[size];
            stream.Read(buffer, 0, buffer.Length);

            // Invokes a callback function
            callback?.Invoke(buffer);
        }

        /// <summary>
        /// Converts integer value to a byte array
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="byteCount">Size of a byte array</param>
        /// <returns>A byte array</returns>
        private static byte[] GetValueInBytes(int value, int byteCount)
        {
            if (byteCount <= 0)
                throw new ArgumentException("Must be positive", nameof(byteCount));
            if (value > Math.Pow(256, byteCount) - 1)
                throw new ArgumentException("Not enough bytes to write value");

            var bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                bytes[i] = (byte)(value / Math.Pow(256, byteCount - i - 1) % 256);
            }
            return bytes;
        }

        /// <summary>
        /// Converts byte array to an integer value
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <returns>An integer value</returns>
        private static int GetValueFromBytes(byte[] bytes)
        {
            int value = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                value += (int)(bytes[i] * Math.Pow(256, bytes.Length - 1 - i));
            }
            return value;
        }
    }
}
