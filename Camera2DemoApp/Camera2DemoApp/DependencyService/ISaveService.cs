namespace Camera2DemoApp.DependencyService
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public interface ISaveService
    {
        Task<bool> SaveImageFile(string fileName, ImageSource? data); 
    }
}
