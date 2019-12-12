using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoUi.Core.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string name = null, params string[] otherNames)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(name);
            foreach (var n in otherNames)
            {
                OnPropertyChanged(n);
            }
            return true;
        }

    }
}