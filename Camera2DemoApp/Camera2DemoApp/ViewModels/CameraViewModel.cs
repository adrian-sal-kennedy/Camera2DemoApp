namespace Camera2DemoApp.ViewModels
{
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class CameraViewModel : BaseViewModel
    {
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
        public async Task<bool> CheckFileSystemPermission()
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                if (await Permissions.CheckStatusAsync<Permissions.StorageWrite>() is PermissionStatus.Granted
                    || await Permissions.RequestAsync<Permissions.StorageWrite>() is PermissionStatus.Granted)
                {
                    return true;
                }
                return false;
            });
        }
    }
}
