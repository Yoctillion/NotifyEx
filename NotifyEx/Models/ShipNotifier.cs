using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Livet;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using NotifyEx.Models.NotifyType;
using NotifyEx.Models.Settings;
using NotifyEx.Properties;
using StatefulModel;

namespace NotifyEx.Models
{
    /// <summary>
    /// 船位不足通知
    /// </summary>
    internal class ShipNotifier : NotificationObject, IWarningCounter, IDisposableHolder
    {
        public static ShipNotifier Current { get; } = new ShipNotifier();
        

        public bool Enabled
        {
            get { return NotifierSettings.EnabledShipNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledShipNotifier.Value != value)
                {
                    NotifierSettings.EnabledShipNotifier.Value = value;
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
            get { return NotifierSettings.ShipWarningCount.Value; }
            set
            {
                if (NotifierSettings.ShipWarningCount.Value != value)
                {
                    NotifierSettings.ShipWarningCount.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public bool EnabledEvent
        {
            get { return NotifierSettings.EnabledEventShipNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledEventShipNotifier.Value != value)
                {
                    NotifierSettings.EnabledEventShipNotifier.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }

        public uint EventWarningCount
        {
            get { return NotifierSettings.EventShipWarningCount.Value; }
            set
            {
                if (NotifierSettings.EventShipWarningCount.Value != value)
                {
                    NotifierSettings.EventShipWarningCount.Value = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.WarningCount));
                }
            }
        }


        private ShipNotifier()
        {
            NotifyHost.Register("/kcsapi/api_get_member/mapinfo", WarningType.Instance, s => CheckReminding());

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

                homeport.Organization
                    .Subscribe(nameof(Organization.Ships), this.UpdateRemainCount)
                    .AddTo(this);
                homeport
                    .Subscribe(nameof(Homeport.Admiral), this.UpdateRemainCount)
                    .AddTo(this);
            }
        }

        private void UpdateRemainCount()
        {
            var port = KanColleClient.Current.Homeport;
            var maxShipCount = port.Admiral.MaxShipCount;
            var currentShipCount = port.Organization.Ships.Count;
            this.Remain = (uint)(maxShipCount - currentShipCount);
        }

        private string CheckReminding()
        {
            if (!Enabled) return null;

            if (this.Remain > WarningCount) return null;

            return this.Remain > 0
                ? $"母港仅剩余 {this.Remain} 空位"
                : "舰娘保有数已满！";
        }


        private readonly MultipleDisposable _compositeDisposable = new MultipleDisposable();

        public void Dispose() => this._compositeDisposable.Dispose();

        public ICollection<IDisposable> CompositeDisposable => this._compositeDisposable;
    }
}
