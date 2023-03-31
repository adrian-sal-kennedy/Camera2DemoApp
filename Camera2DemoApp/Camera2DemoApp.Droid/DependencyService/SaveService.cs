using Camera2DemoApp.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(SaveService))]

namespace Camera2DemoApp.Droid.DependencyService
{
    using System.IO;
    using Android.App;
    using Android.Provider;
    using Camera2DemoApp.DependencyService;

    public class SaveService : ISaveService
    {
        void ISaveService.SaveFile(string fileName, byte[] data)
        {
            var filePath = MediaStore.Images.Media.ExternalContentUri;
            File.WriteAllBytes($"{filePath}", data);
        }
    }
}
