using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;
using Grabacr07.KanColleViewer.Composition;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using StatefulModel;

namespace NotifyEx.Models
{
    [Export(typeof(IPlugin))]
    [Export(typeof(ITaskbarProgress))]
    [ExportMetadata("Guid", Guid)]
    [ExportMetadata("Title", "HomeportSpaceProcessIndicator")]
    [ExportMetadata("Description", "母港剩余空间（舰娘、装备）任务栏进度显示")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "@Yoctillion")]
    public class HomeportSpaceProcess : IPlugin, ITaskbarProgress, IDisposableHolder
    {
        private const string Guid = "93B50362-76AA-4F2C-B6BE-8B1C7FEEA2A5";

        private readonly MultipleDisposable _compositeDisposable = new MultipleDisposable();

        public string Id { get; } = Guid + "-1";

        public string DisplayName { get; } = "母港剩余空间";

        public TaskbarItemProgressState State { get; private set; }

        public double Value { get; private set; }

        public event EventHandler Updated;


        private readonly List<IWarningCounter> _counters = new List<IWarningCounter>();

        public void Initialize()
        {
            this._counters.Add(ShipNotifier.Current);
            this._counters.Add(SlotNotifier.Current);

            foreach (var counter in this._counters)
            {
                counter.Subscribe(nameof(counter.Enabled), this.Update).AddTo(this);
                counter.Subscribe(nameof(counter.WarningCount), this.Update).AddTo(this);
                counter.Subscribe(nameof(counter.Remain), this.Update).AddTo(this);
            }
        }

        private void Update()
        {
            // min means most dangerous(?)
            var minValue = this._counters.Min(c => GetValue(c));

            if (minValue == 0)
            {
                this.State = TaskbarItemProgressState.Error;
            }
            else if (minValue <= 1)
            {
                this.State = TaskbarItemProgressState.Paused;
            }
            else
            {
                this.State = TaskbarItemProgressState.Normal;
            }

            this.Value = Math.Max(0, 1 - minValue / 2);

            this.Updated?.Invoke(this, EventArgs.Empty);
        }

        private static double GetValue(IWarningCounter counter)
        {
            if (!counter.Enabled) return double.MaxValue;

            if (counter.WarningCount != 0)
            {
                return (double)counter.Remain / counter.WarningCount;
            }

            // WarningCount == 0 && Remain == 0
            if (counter.Remain == 0) return 0;

            return double.MaxValue;
        }

        public void Dispose() => this._compositeDisposable.Dispose();

        public ICollection<IDisposable> CompositeDisposable => this._compositeDisposable;
    }
}
