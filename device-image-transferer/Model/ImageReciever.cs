
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace device_image_transferer.Model
{
    public class ImageReciever
    {
        public async Task<string> AwaitImageFromNetwork(int port)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return "Error";

            var endpoint = new IPEndPoint(IPAddress.Any, port);
            TcpListener listener = new(endpoint);

            try
            {
                listener.Start();
                
                var acceptTask = listener.AcceptTcpClientAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromMinutes(2));
                var completedTask = await Task.WhenAny(acceptTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    return "Timeout - no connection within 2 minutes.";
                }

                using TcpClient handler = await listener.AcceptTcpClientAsync();
                await using NetworkStream stream = handler.GetStream();

                var buffer = new byte[1_024];
                int received = await stream.ReadAsync(buffer);

                var message = Encoding.UTF8.GetString(buffer, 0, received);

                return $"Recieved following message: \"{message}\"";
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
