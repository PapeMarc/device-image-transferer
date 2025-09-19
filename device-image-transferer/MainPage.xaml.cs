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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Internet Connection Error", "No internet connection detected. Please connect to a network and try again.", "Ok.");
            }
        }
    }
}
