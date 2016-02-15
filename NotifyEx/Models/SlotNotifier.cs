using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using NotifyEx.Models.NotifyType;
using NotifyEx.Models.Settings;
using NotifyEx.Properties;
using StatefulModel;

namespace NotifyEx.Models
{
    internal class SlotNotifier : NotificationObject, IWarningCounter, IDisposableHolder
    {
        public static SlotNotifier Current { get; } = new SlotNotifier();

        public bool Enabled
        {
            get { return NotifierSettings.EnabledSlotNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledSlotNotifier.Value != value)
                {
                    NotifierSettings.EnabledSlotNotifier.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint WarningCount =>
            this.EnabledEvent && Util.IsInEvent
                ? this.EventWarningCount
                : this.NormalWarningCount;

        private uint _remain = uint.MaxValue;

        public uint Remain
        {
            get { return this._remain; }
            private set
            {
                if (this._remain != value)
                {
                    this._remain = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint NormalWarningCount
        {
            get { return NotifierSettings.SlotWarningCount.Value; }
            set
            {
                if (NotifierSettings.SlotWarningCount.Value != value)
                {
                    NotifierSettings.SlotWarningCount.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public bool EnabledEvent
        {
            get { return NotifierSettings.EnabledEventSlotNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledEventSlotNotifier.Value != value)
                {
                    NotifierSettings.EnabledEventSlotNotifier.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public uint EventWarningCount
        {
            get { return NotifierSettings.EventSlotWarningCount.Value; }
            set
            {
                if (NotifierSettings.EventSlotWarningCount.Value != value)
                {
                    NotifierSettings.EventSlotWarningCount.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }


        private SlotNotifier()
        {
            NotifyHost.Register("/kcsapi/api_get_member/mapinfo", WarningType.Instance, s => this.CheckReminding());

            KanColleClient.Current
                .Subscribe(nameof(KanColleClient.IsStarted), this.Initialize, false)
                .AddTo(this);
        }

        private bool _initialized;

        private void Initialize()
        {
            if (this._initialized) return;

            var homeport = KanColleClient.Current.Homeport;
            if (homeport != null)
            {
                this._initialized = true;

                homeport.Itemyard
                    .Subscribe(nameof(Itemyard.SlotItemsCount), this.UpdateRemainCount)
                    .AddTo(this);
                homeport.Admiral
                    .Subscribe(nameof(Admiral.MaxSlotItemCount), this.UpdateRemainCount)
                    .AddTo(this);
            }
        }

        private void UpdateRemainCount()
        {
            var port = KanColleClient.Current.Homeport;
            var maxCount = port.Admiral.MaxSlotItemCount;
            var currentCount = port.Itemyard.SlotItemsCount;
            this.Remain = (uint)(maxCount - currentCount);
        }

        private string CheckReminding()
        {
            if (!Enabled) return null;

            if (this.Remain > WarningCount) return null;

            return Remain > 0
                ? $"装备仅剩余 {this.Remain} 空位"
                : "装备保有数已满！";
        }


        private readonly MultipleDisposable _compositeDisposable = new MultipleDisposable();

        public void Dispose() => this._compositeDisposable.Dispose();

        public ICollection<IDisposable> CompositeDisposable => this._compositeDisposable;
    }
}
