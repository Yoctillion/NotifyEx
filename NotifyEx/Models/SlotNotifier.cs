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
using NotifyEx.Properties;
using StatefulModel;

namespace NotifyEx.Models
{
    internal class SlotNotifier : NotificationObject, IWarningCounter, IDisposableHolder
    {
        public static SlotNotifier Current { get; } = new SlotNotifier();

        private static readonly Settings Settings = Settings.Default;

        public bool Enabled
        {
            get { return Settings.EnabledSlotNotifier; }
            set
            {
                if (Settings.EnabledSlotNotifier != value)
                {
                    Settings.EnabledSlotNotifier = value;
                    Settings.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint WarningCount =>
            this.EnabledEvent && Util.IsInEvent
                ? this.EventWarningCount
                : Settings.WarningSlotCount;

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
            get { return Settings.WarningSlotCount; }
            set
            {
                if (Settings.WarningSlotCount != value)
                {
                    Settings.WarningSlotCount = value;
                    Settings.Save();
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public bool EnabledEvent
        {
            get { return Settings.EnabledEventSlotNotifier; }
            set
            {
                if (Settings.EnabledEventSlotNotifier != value)
                {
                    Settings.EnabledEventSlotNotifier = value;
                    Settings.Save();
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public uint EventWarningCount
        {
            get { return Settings.EventWarningSlotCount; }
            set
            {
                if (Settings.EventWarningSlotCount != value)
                {
                    Settings.EventWarningSlotCount = value;
                    Settings.Save();
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
