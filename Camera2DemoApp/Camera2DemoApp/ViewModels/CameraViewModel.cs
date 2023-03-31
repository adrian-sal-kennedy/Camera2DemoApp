namespace Camera2DemoApp.ViewModels
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class CameraViewModel : BaseViewModel
    {
        public ICommand MediaCapturedCommand { get; }
        // public ICommand CameraAvailableCommand { get; }

        public CameraViewModel()
        {
            MediaCapturedCommand = new Command(() =>
            {
                Debug.WriteLine("Media captured triggered!");
            });
            // CameraAvailableCommand = new Command(() =>
            // {
            //     Debug.WriteLine("Camera available!");
            // });
        }

        public async Task<bool> CheckCameraPermission()
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                if (await Permissions.CheckStatusAsync<Permissions.Camera>() is PermissionStatus.Granted
                    || await Permissions.RequestAsync<Permissions.Camera>() is PermissionStatus.Granted)
                {
                    return true;
                }
                return false;
            });
        }
    }
}
