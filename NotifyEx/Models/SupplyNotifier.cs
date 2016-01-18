using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using NotifyEx.Properties;

namespace NotifyEx.Models
{
    /// <summary>
    /// 补给通知
    /// </summary>
    public class SupplyNotifier
    {
        private static readonly Settings Settings = Settings.Default;

        private readonly Plugin _plugin;

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


        internal SupplyNotifier(Plugin plugin)
        {
            _plugin = plugin;

            var proxy = KanColleClient.Current.Proxy;

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_get_member/mapinfo")
                .Subscribe(x => CheckSortie());

            proxy.ApiSessionSource.Where(
                x =>
                    x.Request.PathAndQuery == "/kcsapi/api_get_member/practice" ||
                    x.Request.PathAndQuery == "/kcsapi/api_get_member/get_practice_enemyinfo")
                .Subscribe(x => CheckExercise());

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_get_member/mission")
                .Subscribe(x => CheckExpendition());
        }

        private void CheckSortie()
        {
            if (!(Enabled && EnabledSortie)) return;

            var lackSupplyFleets = GetLackSupplyFleets(false);
            Notify(lackSupplyFleets);
        }

        private void CheckExercise()
        {
            if (!(Enabled && EnabledExercise)) return;

            var lackSupplyFleets = GetLackSupplyFleets(false);
            Notify(lackSupplyFleets);
        }

        private void CheckExpendition()
        {
            if (!(Enabled && EnabledExpendition)) return;

            var lackSupplyFleets = GetLackSupplyFleets(true);
            Notify(lackSupplyFleets);
        }

        private Fleet[] GetLackSupplyFleets(bool isExpendition)
        {
            var fleets = KanColleClient.Current.Homeport.Organization.Fleets.Values;
            if (isExpendition) fleets = fleets.Skip(1);

            return fleets.Where(f => f.State.Situation != FleetSituation.Expedition && !f.State.IsReady).ToArray();
        }

        private void Notify(Fleet[] lackSupplyFleets)
        {
            if (lackSupplyFleets.Length == 0) return;

            var info = string.Join(", ", lackSupplyFleets.Select(f => f.Name)) + " 补给不足！";
            _plugin.Notify("SupplyNotify", "补给不足", info);
        }
    }
}
