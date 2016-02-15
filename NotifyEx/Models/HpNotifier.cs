using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Livet;
using NotifyEx.Properties;
using System.Reactive.Linq;
using Grabacr07.KanColleWrapper.Models;
using NotifyEx.Models.NotifyType;
using NotifyEx.Models.Settings;

namespace NotifyEx.Models
{
    /// <summary>
    /// 大破通知
    /// </summary>
    internal class HpNotifier
    {
        public static HpNotifier Current = new HpNotifier();

        public bool Enabled
        {
            get { return NotifierSettings.EnabledLowHpNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledLowHpNotifier.Value != value)
                {
                    NotifierSettings.EnabledLowHpNotifier.Value = value;
                }
            }
        }

        /// <summary>
        /// 是否显示装备损管的舰娘
        /// </summary>
        public bool EnabledShowDamageControl
        {
            get { return NotifierSettings.EnabledShowDamageControl.Value; }
            set
            {
                if (NotifierSettings.EnabledShowDamageControl.Value != value)
                {
                    NotifierSettings.EnabledShowDamageControl.Value = value;
                }
            }
        }

        private HpNotifier()
        {
            NotifyHost.Register("/kcsapi/api_get_member/mapinfo", WarningType.Instance, s => this.CheckSituation(false));
            NotifyHost.Register("/kcsapi/api_req_map/start", WarningType.Instance, s => this.CheckSituation(true));
            NotifyHost.Register("/kcsapi/api_req_map/next", WarningType.Instance, s => this.CheckSituation(true));
        }

        private string CheckSituation(bool isInSortie)
        {
            if (!Enabled) return null;

            var fleets = KanColleClient.Current.Homeport.Organization.Fleets.Values
                .Where(f => isInSortie ? f.IsInSortie : !f.Expedition.IsInExecution)
                .Select(f => new
                {
                    f.Name,
                    Ships = f.Ships.Where(this.FilterShip)
                        .Select(s => s.Info.Name + (s.Situation.HasFlag(ShipSituation.DamageControlled) ? "(损管)" : ""))
                        .ToArray()
                })
                .Where(f => f.Ships.Length > 0)
                .ToArray();

            if (fleets.Length == 0) return null;

            return string.Join(", ", fleets.Select(f => $"{f.Name} {string.Join(" ", f.Ships)}")) + " 大破！";
        }

        private bool FilterShip(Ship ship)
        {
            var situation = ship.Situation;
            return !(situation.HasFlag(ShipSituation.Tow) || situation.HasFlag(ShipSituation.Evacuation))
                   && situation.HasFlag(ShipSituation.HeavilyDamaged)
                   && (this.EnabledShowDamageControl || !situation.HasFlag(ShipSituation.DamageControlled));
        }
    }
}
