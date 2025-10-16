
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace device_image_transferer.Model
{
    public class ImageTransmitter
    {
        public async Task SendMessageAsync(string ipAddress, int port, string message, CancellationToken ct = default)
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
                return;

            using TcpClient client = new();
            await client.ConnectAsync(IPAddress.Parse(ipAddress), port, ct);

            await using NetworkStream stream = client.GetStream();

            //await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
            var data = Encoding.UTF8.GetBytes(message + "\n");
            await stream.WriteAsync(data, 0, data.Length, ct);
            await stream.FlushAsync(ct);
        }
    }
}
