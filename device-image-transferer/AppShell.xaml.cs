namespace device_image_transferer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RecieverPage), typeof(RecieverPage));
            Routing.RegisterRoute(nameof(TransmitterPage), typeof(TransmitterPage));

            //Application.Current.UserAppTheme = AppTheme.Dark;
        }
    }
}
