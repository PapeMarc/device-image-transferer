using QRCoder;

namespace device_image_transferer.Model
{
    public static class QRCoder
    {
        public static ImageSource GenerateQRCodeImage(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrcode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrcode.GetGraphic(20);

            return ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }
    }
}
