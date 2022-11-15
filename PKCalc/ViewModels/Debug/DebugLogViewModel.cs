using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PKCalc.ViewModels.Debug
{
    internal class DebugLogViewModel : ViewModelBase
    {
        public ObservableCollection<Logger.InternalLogItem> LogItems { get; private set; }
        public ICommand Grid_SelectionChanged { get; }
        private int lastItemIndex;
        public int LastItemIndex
        {
            get => lastItemIndex;
            set => Set(ref lastItemIndex, value, suppressLogging: true);
        }

        public DebugLogViewModel()
        {
            this.LogItems = App.InternalLog;
            foreach (Logger.InternalLogItem item in App.Service.InternalLog)
            {
                this.LogItems.Add(item);
            }
            App.Service.InternalLog.CollectionChanged += other_OnLogChanged;
            this.Grid_SelectionChanged = new UI.ActionCommand<SelectionChangedEventArgs>(g => {
                ((DataGrid)g.Source).ScrollIntoView(LogItems[^1]);
            });
        }

        private void other_OnLogChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Logger.InternalLogItem item in e.NewItems.Cast<Logger.InternalLogItem>())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.LogItems.Add(item);
                    this.LastItemIndex = this.LogItems.Count - 1;
                });
            }
        }

        protected override void Cleanup()
        {
            App.Service.InternalLog.CollectionChanged -= other_OnLogChanged;
        }
    }
}
