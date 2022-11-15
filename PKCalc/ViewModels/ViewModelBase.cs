using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PKCalc.ViewModels
{
    internal abstract class ViewModelBase : Client.DisposableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected bool Set<T>(ref T? field, T? newValue = default, [CallerMemberName] string? propertyName = null, bool suppressLogging = false)
        {
            if (EqualityComparer<T>.Default.Equals(field!, newValue!))
            {
                return false;
            }

            field = newValue;
            this.OnPropertyChanged(propertyName);
            if (!suppressLogging)
                App.Logger.Trace("Property {0} changed.", propertyName);

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentException(propertyName + " Binding doesn't exist in " + GetType().Name + " ");

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
