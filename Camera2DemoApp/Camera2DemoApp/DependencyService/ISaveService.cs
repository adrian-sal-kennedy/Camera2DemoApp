namespace Camera2DemoApp.DependencyService
{
    public interface ISaveService
    {
        void SaveFile(string fileName, byte[] data); 
    }
}
