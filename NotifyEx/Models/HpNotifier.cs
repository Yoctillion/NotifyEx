﻿using System;
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
            NotifyHost.Register("/kcsapi/api_req_map/start", WarningType.Instance, s => this.CheckSituation());
            NotifyHost.Register("/kcsapi/api_req_map/next", WarningType.Instance, s => this.CheckSituation());
        }

        private string CheckSituation()
        {
            if (!Enabled) return null;

            var fleets = KanColleClient.Current.Homeport.Organization.Fleets.Values.Where(fleet => fleet.IsInSortie);

            var lowHpList = (from fleet in fleets
                             let ships = fleet.Ships
                             from ship in ships
                             where !(ship.Situation.HasFlag(ShipSituation.Tow) || ship.Situation.HasFlag(ShipSituation.Evacuation))
                                   && ship.Situation.HasFlag(ShipSituation.HeavilyDamaged)
                                   && (EnabledShowDamageControl || !ship.Situation.HasFlag(ShipSituation.DamageControlled))
                             group ship.Info.Name + (ship.Situation.HasFlag(ShipSituation.DamageControlled) ? "(损管)" : "") by fleet.Name
                ).ToArray();

            if (lowHpList.Length == 0) return null;

            return string.Join(", ", lowHpList.Select(fleet => $"{fleet.Key} {string.Join(" ", fleet)}")) + " 大破！";
        }
    }
}
