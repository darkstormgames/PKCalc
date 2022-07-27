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
            foreach (Logger.InternalLogItem item in App.Service.InternalLog)
            {
                this.LogItems.Add(item);
            }
            App.Service.InternalLog.CollectionChanged += other_OnLogChanged;
        }

        private void other_OnLogChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Logger.InternalLogItem item in e.NewItems.Cast<Logger.InternalLogItem>())
            {
                this.LogItems.Add(item);
            }
        }

        protected override void Cleanup()
        {
            App.Service.InternalLog.CollectionChanged -= other_OnLogChanged;
        }
    }
}
