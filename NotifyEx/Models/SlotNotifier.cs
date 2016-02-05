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
    internal class SlotNotifier
    {
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
                }
            }
        }

        public uint WarningCount
        {
            get { return Settings.WarningSlotCount; }
            set
            {
                if (Settings.WarningSlotCount != value)
                {
                    Settings.WarningSlotCount = value;
                    Settings.Save();
                }
            }
        }

        public SlotNotifier()
        {
            NotifyHost.Current.Register("/kcsapi/api_get_member/mapinfo", WarningType.Instance, s => this.CheckReminding());
        }

        private string CheckReminding()
        {
            if (!Enabled) return null;

            var port = KanColleClient.Current.Homeport;
            var max = port.Admiral.MaxSlotItemCount;
            var current = port.Itemyard.SlotItemsCount;
            var reminding = max - current;

            if (reminding > WarningCount) return null;

            return $"装备仅剩余 {reminding} 空位";
        }
    }
}
