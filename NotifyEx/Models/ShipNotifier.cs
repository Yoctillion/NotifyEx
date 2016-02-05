using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Livet;
using NotifyEx.Models.NotifyType;
using NotifyEx.Properties;

namespace NotifyEx.Models
{
    /// <summary>
    /// 船位不足通知
    /// </summary>
    internal class ShipNotifier
    {
        private static readonly Settings Settings = Settings.Default;

        public bool Enabled
        {
            get { return Settings.EnabledShipNotifier; }
            set
            {
                if (Settings.EnabledShipNotifier != value)
                {
                    Settings.EnabledShipNotifier = value;
                    Settings.Save();
                }
            }
        }

        public uint WarningCount
        {
            get { return Settings.WarningShipCount; }
            set
            {
                if (Settings.WarningShipCount != value)
                {
                    Settings.WarningShipCount = value;
                    Settings.Save();
                }
            }
        }

        public ShipNotifier()
        {
            NotifyHost.Current.Register("/kcsapi/api_get_member/mapinfo", WarningType.Instance, s => CheckReminding());
        }

        private string CheckReminding()
        {
            if (!Enabled) return null;

            var port = KanColleClient.Current.Homeport;
            var maxShipCount = port.Admiral.MaxShipCount;
            var currentShipCount = port.Organization.Ships.Count;
            var reminding = maxShipCount - currentShipCount;

            if (reminding > WarningCount) return null;

            return $"母港仅剩余 {reminding} 空位";
        }
    }
}
