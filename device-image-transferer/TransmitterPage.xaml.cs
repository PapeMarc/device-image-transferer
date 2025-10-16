using device_image_transferer.Model;
using device_image_transferer.ViewModels;
using Microsoft.Maui.Controls;

namespace device_image_transferer;

public partial class TransmitterPage : ContentPage
{
	public TransmitterPage(TransmitterPageViewModel tpvm)
	{
		InitializeComponent();
		BindingContext = tpvm;
        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };

        barcodeReader.CameraLocation = ZXing.Net.Maui.CameraLocation.Rear;
    }

    public void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first == null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {

            barcodeReader.IsDetecting = false;
            barcodeReader.IsEnabled = false;
            barcodeReader.IsVisible = false;

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            successIcon.IsVisible = true;
        });

        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("Detected QR-Code.", "Now connecting to other Device...", "Ok.");
            await Task.Delay(10000);
            barcodeReader.IsDetecting = true;
            barcodeReader.IsEnabled = true;
            barcodeReader.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            successIcon.IsVisible = false;
        });

        Application.Current!.Dispatcher.DispatchAsync(async () =>
        {
            var payload = first.Value;
            string[] targetInfo = payload.Split(';');

            if (targetInfo.Length.Equals(2))
            {
                ImageTransmitter transmitter = new ImageTransmitter();
                Console.WriteLine("Trying to send..");
                try
                {
                    await transmitter.SendMessageAsync(targetInfo[0], Convert.ToInt32(targetInfo[1]), "Erster Test!");
                    await DisplayAlert("Message Sent.", "The image was sent successfully.", "Ok.");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred while sending the image: {ex.Message}", "Ok.");
                }
            }
        });
    }
}