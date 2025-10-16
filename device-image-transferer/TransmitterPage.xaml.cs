using device_image_transferer.Model;
using device_image_transferer.ViewModels;

namespace device_image_transferer;

public partial class TransmitterPage : ContentPage
{
    private string? targetHost = null;
    private int? targetPort = null;
    private bool barcodeDetected = false;

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

        if (first == null) return;

        Dispatcher.Dispatch(async () =>
        {

            barcodeReader.IsDetecting = false;
            barcodeReader.BarcodesDetected -= barcodeReader_BarcodesDetected;

            barcodeReader.Handler?.DisconnectHandler();

            barcodeReader.IsEnabled = false;
            barcodeReader.IsVisible = false;

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            successIcon.IsVisible = true;
        });

        Dispatcher.DispatchAsync(async () => { 
            await Task.Delay(4000);
            BarcodeReaderGrid.IsVisible = !barcodeDetected;
            CameraGrid.IsVisible = barcodeDetected;
        });

        /*Dispatcher.DispatchAsync(async () =>
        {
            await Task.Delay(10000);
            barcodeReader.IsDetecting = true;
            barcodeReader.IsEnabled = true;
            barcodeReader.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            successIcon.IsVisible = false;
        });*/

        Application.Current!.Dispatcher.DispatchAsync(async () =>
        {
            var payload = first.Value.Trim();
            string[] targetInfo = payload.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (targetInfo.Length != 2)
            {
                await DisplayAlert("Detected QR-Code", $"Unerwartet: {payload}", "OK");
                return;
            }

            targetHost = targetInfo[0];
            targetPort = int.Parse(targetInfo[1]);
            barcodeDetected = true;

            await DisplayAlert("Detected QR-Code", $"Target: {targetHost}, Port: {targetPort}", "Ok.");

            //await SendImageToTarget();
        });

    }

    public static async Task<byte[]> TakePhotoAsync(Image imagePreview = null)
    {
        // Laufzeitberechtigung (Android)
        var camStatus = await Permissions.RequestAsync<Permissions.Camera>();
        if (camStatus != PermissionStatus.Granted)
            throw new Exception("Kamera-Berechtigung verweigert.");

        var file = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
        {
            Title = $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.jpg"
        });
        if (file == null) throw new Exception("Aufnahme abgebrochen.");

        await using var inStream = await file.OpenReadAsync();

        // Vorschau anzeigen (optional)
        if (imagePreview != null)
            imagePreview.Source = ImageSource.FromStream(() => file.OpenReadAsync().Result);

        using var ms = new MemoryStream();
        await inStream.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task SendImageToTarget(byte[] image)
    {
        ImageTransmitter transmitter = new ImageTransmitter();
        Console.WriteLine("Trying to send..");
        try
        {
            if(targetPort == null || targetHost == null)
                return;

            await transmitter.SendMessageAsync(targetHost, (int)targetPort, "Testnachricht.");
            await DisplayAlert("Message Sent.", "The image was sent successfully.", "Ok.");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while sending the image: {ex.Message}", "Ok.");
        }
    }

    private async void OnTakeAndSendClicked(object sender, EventArgs e)
    {
        try
        {
            var bytes = await TakePhotoAsync(PerviewImage);

            await SendImageToTarget(bytes);

            await DisplayAlert("OK", "Foto gesendet.", "Schlieﬂen");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fehler", ex.Message, "OK");
        }
    }
}