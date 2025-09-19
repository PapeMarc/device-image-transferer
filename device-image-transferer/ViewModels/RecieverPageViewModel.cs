
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public void GenerateQRCode()
        {
            string networkInfo = GetNetworkInformation();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(networkInfo, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeBytes = qrCode.GetGraphic(
                            pixelsPerModule: 20,
                            drawQuietZones: false
                        );
                QrImage = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
            }
            OnPropertyChanged(nameof(QrImage));
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
