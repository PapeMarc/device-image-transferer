
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace device_image_transferer.Model
{
    public class ImageReciever
    {
        public async Task<string> AwaitImageFromNetwork(int port, TimeSpan? timeout = null)
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                return "Error";

            var endpoint = new IPEndPoint(IPAddress.Any, port);
            TcpListener listener = new(endpoint);
            listener.Start();
            timeout ??= TimeSpan.FromMinutes(2);

            try
            {

                var acceptTask = listener.AcceptTcpClientAsync();
                var completed = await Task.WhenAny(acceptTask, Task.Delay(timeout.Value));

                if (completed != acceptTask)
                    return $"Timeout - no connection within {timeout.Value.TotalMinutes} minutes.";

                using var client = await acceptTask;
                await using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: false);

                var line = await reader.ReadLineAsync();
                if (line is null)
                    return "Connection closed without data.";

                return $"Received message: \"{line}\"";
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
