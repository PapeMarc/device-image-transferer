namespace device_image_transferer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window _mainWindow = new Window(new AppShell());
            _mainWindow.Title = "Tyche the Transferer";
            return _mainWindow;
        }
    }
}