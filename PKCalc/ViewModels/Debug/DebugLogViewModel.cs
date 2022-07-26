using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.ViewModels.Debug
{
    internal class DebugLogViewModel : ViewModelBase
    {
        public ObservableCollection<Logger.InternalLogItem> LogItems { get; private set; }

        public DebugLogViewModel()
        {
            this.LogItems = App.InternalLog;
            this.LogItems.CollectionChanged += onLogChanged;
        }

        private void onLogChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnPropertyChanged(nameof(this.LogItems));
        }

        protected override void Cleanup()
        {
            this.LogItems.CollectionChanged -= onLogChanged;
        }
    }
}
