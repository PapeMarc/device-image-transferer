using device_image_transferer.ViewModels;

namespace device_image_transferer;

public partial class RecieverPage : ContentPage
{
	public RecieverPage(RecieverPageViewModel rpvm)
	{
		InitializeComponent();
		BindingContext = rpvm;
    }
}