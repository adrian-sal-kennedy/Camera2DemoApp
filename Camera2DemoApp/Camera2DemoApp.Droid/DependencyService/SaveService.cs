using Camera2DemoApp.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(SaveService))]

namespace Camera2DemoApp.Droid.DependencyService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Android.Content;
    using Android.Graphics;
    using Android.OS;
    using Android.Provider;
    using Camera2DemoApp.DependencyService;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Application = Android.App.Application;
    using Debug = System.Diagnostics.Debug;
    using Environment = Android.OS.Environment;
    using Path = System.IO.Path;
    using Uri = Android.Net.Uri;

    public class SaveService : ISaveService
    {
        [Obsolete("Obsolete")]
        private string? GetMediaPathOld()
        {
            if (!(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim)?.AbsolutePath is
                    { } picsDir)) { return null; }

            return picsDir;
        }

        private async Task<bool> SaveByteArrayToFile(string fileName, MemoryStream imageStream)
        {
            if ((int)Build.VERSION.SdkInt < 29)
            {
                string path = GetMediaPathOld();
                // Yes we know it'll be a jpg file, trust me bruh. I don't feel super comfortable just assuming
                // anything, but in this case there's just no way it isn't a jpg unless I change the part where I
                // compress to jpg, and I'm not going to do that.
                string filePath = Path.Combine(path!, $"{fileName}.jpg");
                try
                {
                    await File.WriteAllBytesAsync(filePath, imageStream.GetBuffer());
                    Debug.WriteLine($"SUCCESS! File written to {filePath}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"FAILED! {ex.Message}");
                    return false;
                }

                return true;
            }

            var resolver = Application.Context.ContentResolver;
            var contentValues = new ContentValues();
            contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
            contentValues.Put(MediaStore.IMediaColumns.MimeType, "image/jpeg");
            contentValues.Put(
                MediaStore.IMediaColumns.RelativePath,
                Path.Combine(Environment.DirectoryDcim ?? "DCIM", AppInfo.Name)
            );
            if (!(resolver?.Insert(MediaStore.Images.Media.ExternalContentUri ?? Uri.Empty!, contentValues) is
                    { } uri))
            {
                return false;
            }

            try
            {
                using (var os = resolver.OpenOutputStream(uri))
                {
                    if (os is null) { return false; }

                    imageStream.Position = 0;
                    await imageStream.CopyToAsync(os);
                }

                Debug.WriteLine($"SUCCESS! File written to {uri.EncodedPath}");
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
                return await SaveByteArrayToFile(fileName, ms);
            }

            return false;
        }
    }
}
