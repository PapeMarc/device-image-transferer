using device_image_transferer.ViewModels;

namespace device_image_transferer
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mpvm)
        {
            InitializeComponent();
            BindingContext = mpvm;
        }
    }
}
