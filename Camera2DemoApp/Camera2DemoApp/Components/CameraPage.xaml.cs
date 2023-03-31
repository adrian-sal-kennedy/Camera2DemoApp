using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Camera2DemoApp.Components
{
    using Xamarin.CommunityToolkit.UI.Views;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
        }
        // private void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        // {
        //     // throw new NotImplementedException();
        // }
        //
        // private void CameraView_OnAvailable(object sender, bool e)
        // {
        //     // throw new NotImplementedException();
        // }
    }
}