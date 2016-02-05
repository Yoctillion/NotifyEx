using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using NotifyEx.Models.NotifyType;
using NotifyEx.Properties;

namespace NotifyEx.Models
{
    /// <summary>
    /// 补给通知
    /// </summary>
    internal class SupplyNotifier
    {
        private static readonly Settings Settings = Settings.Default;

        public bool Enabled
        {
            get { return Settings.EnabledSupplyNotifier; }
            set
            {
                if (Settings.EnabledSupplyNotifier != value)
                {
                    Settings.EnabledSupplyNotifier = value;
                    Settings.Save();
                }
            }
        }

        public bool EnabledSortie
        {
            get { return Settings.EnabledSortieSupplyNotifier; }
            set
            {
                if (Settings.EnabledSortieSupplyNotifier != value)
                {
                    Settings.EnabledSortieSupplyNotifier = value;
                    Settings.Save();
                }
            }
        }

        public bool EnabledExercise
        {
            get { return Settings.EnabledExerciseSupplyNotifier; }
            set
            {
                if (Settings.EnabledExerciseSupplyNotifier != value)
                {
                    Settings.EnabledExerciseSupplyNotifier = value;
                    Settings.Save();
                }
            }
        }

        public bool EnabledExpendition
        {
            get { return Settings.EnabledExpenditionSupplyNotifier; }
            set
            {
                if (Settings.EnabledExpenditionSupplyNotifier != value)
                {
                    Settings.EnabledExpenditionSupplyNotifier = value;
                    Settings.Save();
                }
            }
        }


        internal SupplyNotifier()
        {
            var host = NotifyHost.Current;
            var type = WarningType.Instance;
            host.Register("/kcsapi/api_get_member/mapinfo", type, s => this.CheckSortie());
            host.Register("/kcsapi/api_get_member/practice", type, s => this.CheckExercise());
            host.Register("/kcsapi/api_get_member/get_practice_enemyinfo", type, s => this.CheckExercise());
            host.Register("/kcsapi/api_get_member/mission", type, s => this.CheckExpendition());
        }

        private string CheckSortie()
        {
            if (!(Enabled && EnabledSortie)) return null;

            return this.GetNotificationMsg(false);
        }

        private string CheckExercise()
        {
            if (!(Enabled && EnabledExercise)) return null;

            return this.GetNotificationMsg(false);
        }

        private string CheckExpendition()
        {
            if (!(Enabled && EnabledExpendition)) return null;

            return this.GetNotificationMsg(true);
        }


        private string GetNotificationMsg(bool isExpendition)
        {
            var fleets = this.GetLackSupplyFleets(isExpendition);

            if (fleets.Length == 0) return null;
            return string.Join(", ", fleets.Select(f => f.Name)) + " 补给不足！";
        }

        private Fleet[] GetLackSupplyFleets(bool isExpendition)
        {
            var fleets = KanColleClient.Current.Homeport.Organization.Fleets.Values;
            if (isExpendition) fleets = fleets.Skip(1);

            return fleets.Where(f => f.State.Situation != FleetSituation.Expedition &&
                                     f.Ships.Any(s => s.Fuel.Current < s.Fuel.Maximum || s.Bull.Current < s.Bull.Maximum))
                .ToArray();
        }
    }
}
