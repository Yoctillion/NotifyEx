using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using MetroTrilithon.Mvvm;
using NotifyEx.Models.NotifyType;
using NotifyEx.Models.Settings;

namespace NotifyEx.Models
{
    public class LandBaseNotifier : NotificationObject
    {
        public static LandBaseNotifier Current { get; } = new LandBaseNotifier();

        public bool Enabled
        {
            get { return NotifierSettings.EnabledLandBaseNotifier.Value; }
            set
            {
                if (NotifierSettings.EnabledLandBaseNotifier.Value != value)
                {
                    NotifierSettings.EnabledLandBaseNotifier.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool ShowNotificationBeforeSelectMap
        {
            get { return NotifierSettings.ShowLandBaseNotificationBeforeSelectMap.Value; }
            set
            {
                if (NotifierSettings.ShowLandBaseNotificationBeforeSelectMap.Value != value)
                {
                    NotifierSettings.ShowLandBaseNotificationBeforeSelectMap.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool ShowUncompletedAirCorps
        {
            get { return NotifierSettings.ShowUncompletedAirCorps.Value; }
            set
            {
                if (NotifierSettings.ShowUncompletedAirCorps.Value != value)
                {
                    NotifierSettings.ShowUncompletedAirCorps.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool ShowNotReadyAirCorps
        {
            get { return NotifierSettings.ShowNotReadyAirCorps.Value; }
            set
            {
                if (NotifierSettings.ShowNotReadyAirCorps.Value != value)
                {
                    NotifierSettings.ShowNotReadyAirCorps.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool ShowNeedSupplyAirCorps
        {
            get { return NotifierSettings.ShowNeedSupplyAirCorps.Value; }
            set
            {
                if (NotifierSettings.ShowNeedSupplyAirCorps.Value != value)
                {
                    NotifierSettings.ShowNeedSupplyAirCorps.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public bool ShowTiredAirCorps
        {
            get { return NotifierSettings.ShowTiredAirCorps.Value; }
            set
            {
                if (NotifierSettings.ShowTiredAirCorps.Value != value)
                {
                    NotifierSettings.ShowTiredAirCorps.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private LandBaseNotifier()
        {
            NotifyHost.Register("/kcsapi/api_get_member/base_air_corps", LandBaseType.Instance, s =>
            {
                SvData<base_air_corps[]> svData;
                SvData.TryParse(s, out svData);
                this._data = svData?.Data;
                return this.CheckSituation(true);
            });
            NotifyHost.Register("/kcsapi/api_get_member/sortie_conditions", LandBaseType.Instance, s => this.CheckSituation());

            var proxy = KanColleClient.Current.Proxy;

            proxy.ApiSessionSource
                .Where(s => s.Request.PathAndQuery == "/kcsapi/api_req_air_corps/set_plane")
                .TryParse<req_air_corps_set_plane>()
                .Subscribe(d => this.SetPlane(int.Parse(d.Request["api_base_id"]), d.Data));

            proxy.ApiSessionSource
                .Where(s => s.Request.PathAndQuery == "/kcsapi/api_req_air_corps/change_name")
                .TryParse()
                .Subscribe(d => this.UpdateName(int.Parse(d.Request["api_base_id"]), d.Request["api_name"]));

            proxy.ApiSessionSource
                .Where(s => s.Request.PathAndQuery == "/kcsapi/api_req_air_corps/supply")
                .TryParse<req_air_corps_supply>()
                .Subscribe(d => this.Supply(int.Parse(d.Request["api_base_id"]), d.Data));

            proxy.ApiSessionSource
                .Where(s => s.Request.PathAndQuery == "/kcsapi/api_req_air_corps/set_action")
                .TryParse()
                .Subscribe(d => this.UpdateAction(
                    SplitParam(d.Request["api_base_id"]),
                    SplitParam(d.Request["api_action_kind"]))
                );
        }

        private base_air_corps[] _data;

        private string CheckSituation(bool beforeSelectMap = false)
        {
            if (!this.Enabled) return "";

            if (this._data == null) return "";

            if (beforeSelectMap && !this.ShowNotificationBeforeSelectMap) return "";

            var notificationContent = new List<string>();

            if (this.ShowUncompletedAirCorps)
            {
                var uncompleted = this._data.Where(s => s.api_plane_info.Any(p => p.api_state == 0));
                notificationContent.Add(this.FormatInfo(uncompleted, "有空位未配置"));
            }

            if (this.ShowNotReadyAirCorps)
            {
                var notReady = this._data.Where(s => s.api_action_kind != 1 && s.api_action_kind != 2);
                notificationContent.Add(this.FormatInfo(notReady, "未在出击/防空状态"));
            }

            if (this.ShowNeedSupplyAirCorps)
            {
                var needSupply = this._data.Where(s => s.api_plane_info.Any(p => p.api_count < p.api_max_count));
                notificationContent.Add(this.FormatInfo(needSupply, "需要补给"));
            }

            if (this.ShowTiredAirCorps)
            {
                var tired = this._data.Where(s => s.api_plane_info.Any(p => p.api_state > 1));
                notificationContent.Add(this.FormatInfo(tired, "疲劳"));
            }

            return string.Join(Environment.NewLine, notificationContent);
        }

        private string FormatInfo(IEnumerable<base_air_corps> airCorps, string info)
        {
            return airCorps.Any()
                ? $"{string.Join(", ", airCorps.Select(s => s.api_name))} {info}"
                : "";
        }

        private void SetPlane(int baseId, req_air_corps_set_plane data)
        {
            if (this._data == null) return;

            var airCorp = this.GetAirCorp(baseId);
            this.Update(airCorp, data.api_plane_info);
        }

        private void UpdateName(int baseId, string name)
        {
            if (this._data == null) return;

            var airCorp = this.GetAirCorp(baseId);
            airCorp.api_name = name;
        }

        private void Supply(int baseId, req_air_corps_supply data)
        {
            if (this._data == null) return;

            var airCorp = this.GetAirCorp(baseId);
            this.Update(airCorp, data.api_plane_info);
        }

        private base_air_corps GetAirCorp(int baseId)
        {
            return this._data.First(b => b.api_rid == baseId);
        }

        private void UpdateAction(IEnumerable<int> baseIds, IEnumerable<int> actionIds)
        {
            if (this._data == null) return;

            foreach (var pair in baseIds.Zip(actionIds, (b, a) => new { Id = b, Action = a }))
            {
                this.GetAirCorp(pair.Id).api_action_kind = pair.Action;
            }
        }

        private void Update(base_air_corps airCorp, api_plane_info[] squadrons)
        {
            foreach (var squadron in squadrons)
            {
                var squadronIndex = airCorp.api_plane_info.FirstIndex(s => s.api_squadron_id == squadron.api_squadron_id);
                airCorp.api_plane_info[squadronIndex] = squadron;
            }
        }

        private static IEnumerable<int> SplitParam(string param)
        {
            return param.Split(',').Select(int.Parse);
        }
    }
}
