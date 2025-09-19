
using System.Net;
using System.Net.Sockets;

namespace device_image_transferer.Model
{
    public class ImageTransmitter
    {
        public async Task SendImageToNetwork(string ipAddress, int port, byte[] imageBytes)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;
            try
            {
                using TcpClient client = new();
                await client.ConnectAsync(IPAddress.Parse(ipAddress), port);
                await using NetworkStream stream = client.GetStream();
                await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
