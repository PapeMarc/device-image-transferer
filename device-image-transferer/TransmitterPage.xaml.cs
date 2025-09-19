using device_image_transferer.ViewModels;

namespace device_image_transferer;

public partial class TransmitterPage : ContentPage
{
	public TransmitterPage()
	{
		InitializeComponent();
		BindingContext = new TransmitterPageViewModel();
    }
}