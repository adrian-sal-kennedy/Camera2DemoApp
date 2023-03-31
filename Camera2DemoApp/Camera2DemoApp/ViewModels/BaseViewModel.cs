namespace Camera2DemoApp.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
#region INotifiyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
#endregion
    }
}
