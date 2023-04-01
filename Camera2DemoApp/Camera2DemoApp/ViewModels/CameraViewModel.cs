namespace Camera2DemoApp.ViewModels
{
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class CameraViewModel : BaseViewModel
    {
        private static bool hasFileSystemPermission;

        public bool HasFileSystemPermission
        {
            get => hasFileSystemPermission;
            set => SetField(ref hasFileSystemPermission, value);
        }

        private static bool hasCameraPermission;

        public bool HasCameraPermission
        {
            get => hasCameraPermission;
            set => SetField(ref hasCameraPermission, value);
        }

        // this tuple represents an image and it's rotation as returned by the camera
        private static ImageSource? lastCapturedImage;

        public ImageSource? LastCapturedImage
        {
            get => lastCapturedImage;
            set => SetField(ref lastCapturedImage, value);
        }

        private static double lastCapturedImageRotation;

        public double LastCapturedImageRotation
        {
            get => lastCapturedImageRotation;
            set => SetField(ref lastCapturedImageRotation, value);
        }

        public async Task<bool> CheckCameraPermission()
        {
            if (hasCameraPermission) { return true; }

            HasCameraPermission = await Device.InvokeOnMainThreadAsync(async () =>
            {
                if (await Permissions.CheckStatusAsync<Permissions.Camera>() is PermissionStatus.Granted
                    || await Permissions.RequestAsync<Permissions.Camera>() is PermissionStatus.Granted)
                {
                    return true;
                }

                return false;
            });
            return hasCameraPermission;
        }

        public async Task<bool> CheckFileSystemPermission()
        {
            if (hasFileSystemPermission) { return true; }

            HasFileSystemPermission = await Device.InvokeOnMainThreadAsync(async () =>
            {
                if ((await Permissions.CheckStatusAsync<Permissions.StorageWrite>() is PermissionStatus.Granted
                     && await Permissions.CheckStatusAsync<Permissions.Media>() is PermissionStatus.Granted)
                    || (await Permissions.RequestAsync<Permissions.StorageWrite>() is PermissionStatus.Granted
                        && await Permissions.RequestAsync<Permissions.Media>() is PermissionStatus.Granted))
                {
                    return true;
                }

                return false;
            });
            return hasFileSystemPermission;
        }
    }
}
