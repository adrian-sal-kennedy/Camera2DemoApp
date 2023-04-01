using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Camera2DemoApp.Components
{
    using System;
    using System.Threading.Tasks;
    using DependencyService;
    using ViewModels;
    using Xamarin.CommunityToolkit.UI.Views;
    using DependencyService = Xamarin.Forms.DependencyService;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (BindingContext is not CameraViewModel vm) { return; }

            // check we have camera permission, divert to about page if somehow we can't get it
            base.OnAppearing();
            string text = string.Format(AppResources.ZoomFactor, 1);
            ZoomLabel.Text = text;
            ZoomLabelBg.Text = text;
            // Debug.WriteLine(cameraView.Zoom);
            bool canUseCamera = await vm.CheckCameraPermission();
            if (!canUseCamera)
            {
                await Shell.Current.GoToAsync("//AboutPage");
            }
        }

        private void OnCameraFrontBackToggle(object sender, EventArgs e)
        {
            Camera.CameraOptions =
                Camera.CameraOptions != CameraOptions.Front ? CameraOptions.Front : CameraOptions.Back;
            if (sender is Button btn)
            {
                btn.Text = Camera.CameraOptions == CameraOptions.Front ? AppResources.FrontCam : AppResources.BackCam;
            }
        }

        private void OnAvailable(object sender, bool e)
        {
            if (sender is CameraView c)
            {
                FrontBackToggle.Text = c.CameraOptions == CameraOptions.Front ? AppResources.FrontCam : AppResources.BackCam;
            }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            double zoom = e.Scale * Camera.Zoom;
            UpdateZoom(zoom);
        }

        private void UpdateZoom(double zoom)
        {
            zoom = Math.Min(Math.Max(zoom, 1), Camera.MaxZoom);
            Camera.Zoom = zoom;
            string text = string.Format(AppResources.ZoomFactor, $"{zoom:0.00}");
            ZoomLabel.Text = text;
            ZoomLabelBg.Text = text;
        }

        private void FireShutter(object sender, EventArgs e)
        {
            Camera.Shutter();
        }

        private void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            switch (Camera.CaptureMode)
            {
                default:
                case CameraCaptureMode.Default:
                case CameraCaptureMode.Photo:
                    PreviewPic.Source = e.Image;
                    PreviewBox.Rotation = e.Rotation;
                    PreviewBox.IsVisible = true;
                    // I should use another DependencyService to inspect the native image from each platform and allow
                    // me to choose the file extension that way, but brevity demands I just do that inside the file save
                    // method and sacrifice a little modularity here.
                    Task.Run(async () =>
                    {
                        await DependencyService.Get<ISaveService>()
                            .SaveImageFile($"CameraDemoPicture_{DateTimeOffset.Now:yyMMdd-HH-mm-ss}", e.Image);
                    }).Wait();
                    break;
                case CameraCaptureMode.Video:
                    PreviewBox.IsVisible = false;
                    break;
            }
        }

        protected override void OnBindingContextChanged()
        {
            PreviewBox.IsVisible = false;
            base.OnBindingContextChanged();
        }
    }
}
