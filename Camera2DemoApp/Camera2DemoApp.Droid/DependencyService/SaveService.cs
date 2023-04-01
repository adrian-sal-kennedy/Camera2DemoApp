using Camera2DemoApp.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(SaveService))]

namespace Camera2DemoApp.Droid.DependencyService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Graphics;
    using Android.OS;
    using Android.Provider;
    using Camera2DemoApp.DependencyService;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Application = Android.App.Application;
    using Debug = System.Diagnostics.Debug;
    using Environment = Android.OS.Environment;
    using Path = System.IO.Path;

    public class SaveService : ISaveService
    {
        private bool SaveByteArrayToFile(string fileName, byte[] data)
        {
            if (!(Application.Context.GetExternalFilesDir(Environment.DirectoryDcim)?.AbsolutePath is { } picsDir)) { return false; }

            string filePath = Path.Combine(picsDir, fileName);
            try
            {
                File.WriteAllBytes(filePath, data);
                Debug.WriteLine($"SUCCESS! File written to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FAILED! {ex.Message}");
                return false;
            }

            return true;
        }

        public async Task<bool> SaveImageFile(string fileName, ImageSource? data)
        {
            if (!(data is StreamImageSource imgSrc)) { return false; }

            var handler = new StreamImagesourceHandler();
            Bitmap img = await handler.LoadImageAsync(imgSrc, Application.Context);
            using var ms = new MemoryStream();
            if (await img.CompressAsync(Bitmap.CompressFormat.Jpeg, 71, ms))
            {
                ms.Position = 0;
                return SaveByteArrayToFile($"{fileName}.jpg", ms.GetBuffer());
            }

            return false;
        }
    }
}
