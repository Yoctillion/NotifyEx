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

namespace NotifyEx.Models
{
	/// <summary>
	/// 大破通知
	/// </summary>
	class HpNotifier : NotificationObject
	{
		private static readonly Settings Settings = Settings.Default;

		private readonly Plugin _plugin;

		public bool Enabled
		{
			get { return Settings.EnabledLowHpNotifier; }
			set
			{
				if (Settings.EnabledLowHpNotifier != value)
				{
					Settings.EnabledLowHpNotifier = value;
					Settings.Save();
					RaisePropertyChanged();
				}
			}
		}

		public HpNotifier(Plugin plugin)
		{
			_plugin = plugin;

			var proxy = KanColleClient.Current.Proxy;

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start")
				.TryParse().Subscribe(x => CheckSituation(x.Request["api_deck_id"]));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next")
				.Subscribe(x => CheckSituation());
		}

		private int CurrentDeckId { get; set; }

		private void CheckSituation(string api_deck_id = null)
		{
			if (api_deck_id != null) CurrentDeckId = int.Parse(api_deck_id);
			if (CurrentDeckId < 1) return;

			if (!Enabled) return;

			var organization = KanColleClient.Current.Homeport.Organization;

			var lowHpList = new List<LowHpInfo>();

			Fleet[] fleets;
			if (CurrentDeckId == 1 && organization.Combined)
				fleets = new[] { organization.Fleets[1], organization.Fleets[2] };
			else
				fleets = new[] { organization.Fleets[CurrentDeckId] };

			foreach (var fleet in fleets)
			{
				var lowHpShips = (from ship in fleet.Ships
								  where ship.Situation.HasFlag(ShipSituation.HeavilyDamaged)
								  select ship.Info.Name
										 + (ship.Situation.HasFlag(ShipSituation.DamageControlled) ? "(损管)" : "")
								  ).ToArray();

				if (lowHpShips.Length > 0)
					lowHpList.Add(new LowHpInfo { Name = fleet.Name, Ships = lowHpShips });
			}

			if (lowHpList.Count > 0)
				Notify(lowHpList);
		}

		private class LowHpInfo
		{
			public string Name { get; set; }
			public string[] Ships { get; set; }
		}

		private void Notify(List<LowHpInfo> list)
		{
			var info = string.Join(", ", list.Select(fleet => $"{fleet.Name} {string.Join(" ", fleet.Ships)}")) + " 大破！";
			_plugin.Notify("HpNotify", "大破警告", info);
		}
	}
}
