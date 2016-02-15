using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using NotifyEx.Models.NotifyType;
using NotifyEx.Models.Settings;
using NotifyEx.Properties;

namespace NotifyEx.Models
{
    /// <summary>
    /// 补给通知
    /// </summary>
    internal class SupplyNotifier
    {
        public static SupplyNotifier Current { get; } = new SupplyNotifier();

        public bool Enabled
        {
            get { return NotifierSettings.EnabledSupplyNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledSupplyNotifier.Value != value)
                {
                    NotifierSettings.EnabledSupplyNotifier.Value = value;
                }
            }
        }

        public bool EnabledSortie
        {
            get { return NotifierSettings.EnabledSortieSupplyNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledSortieSupplyNotifier.Value != value)
                {
                    NotifierSettings.EnabledSortieSupplyNotifier.Value = value;
                }
            }
        }

        public bool EnabledExercise
        {
            get { return NotifierSettings.EnabledExerciseSupplyNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledExerciseSupplyNotifier.Value != value)
                {
                    NotifierSettings.EnabledExerciseSupplyNotifier.Value = value;
                }
            }
        }

        public bool EnabledExpendition
        {
            get { return NotifierSettings.EnabledExpenditionSupplyNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledExpenditionSupplyNotifier.Value != value)
                {
                    NotifierSettings.EnabledExpenditionSupplyNotifier.Value = value;
                }
            }
        }


        private SupplyNotifier()
        {
            var type = WarningType.Instance;
            NotifyHost.Register("/kcsapi/api_get_member/mapinfo", type, s => this.CheckSortie());
            NotifyHost.Register("/kcsapi/api_get_member/practice", type, s => this.CheckExercise());
            NotifyHost.Register("/kcsapi/api_get_member/get_practice_enemyinfo", type, s => this.CheckExercise());
            NotifyHost.Register("/kcsapi/api_get_member/mission", type, s => this.CheckExpendition());
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
