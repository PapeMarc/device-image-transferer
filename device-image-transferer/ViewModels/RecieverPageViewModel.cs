
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using device_image_transferer.Model;
using QRCoder;
using System.Net;
using System.Net.Sockets;

namespace device_image_transferer.ViewModels
{
    public partial class RecieverPageViewModel: ObservableObject
    {
        [ObservableProperty]
        ImageSource qrImage;

        [RelayCommand]
        public async Task GenerateQRCodeAndAwaitImage(RecieverPage page)
        {
            string networkInfo = GetNetworkInformation();
            int appPort = 2003;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{networkInfo};{appPort}", QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeBytes = qrCode.GetGraphic(
                            pixelsPerModule: 20,
                            drawQuietZones: false
                        );
                QrImage = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            }
            OnPropertyChanged(nameof(QrImage));
            ImageReciever reciever = new ImageReciever();
            
            string message = await reciever.AwaitImageFromNetwork(appPort);

            Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                await page.DisplayAlert("Recieved a message.", message, "Ok.");
            });
        }

        private string GetNetworkInformation()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return "Error";

        }

    }
}
