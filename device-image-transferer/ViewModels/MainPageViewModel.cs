using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace device_image_transferer.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        async Task NavigateToRecieverPage()
        {
            await Shell.Current.GoToAsync($"{nameof(RecieverPage)}");
        }

        [RelayCommand]
        async Task NavigateToTransmitterPage()
        {
            await Shell.Current.GoToAsync($"{nameof(TransmitterPage)}");
        }
    }
}
