using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Livet;
using NotifyEx.Properties;

namespace NotifyEx.Models
{
	/// <summary>
	/// 船位不足通知
	/// </summary>
	class ShipNotifier : NotificationObject
	{
		private static readonly Settings Settings = Settings.Default;

		private readonly Plugin _plugin;

		public bool Enabled
		{
			get { return Settings.EnabledShipNotifier; }
			set
			{
				if (Settings.EnabledShipNotifier != value)
				{
					Settings.EnabledShipNotifier = value;
					Settings.Save();
					RaisePropertyChanged();
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
					RaisePropertyChanged();
				}
			}
		}

		public ShipNotifier(Plugin plugin)
		{
			_plugin = plugin;

			var proxy = KanColleClient.Current.Proxy;
			proxy.ApiSessionSource.Where(s => s.Request.PathAndQuery.StartsWith("/kcsapi/api_get_member/mapinfo"))
				.Subscribe(x => CheckReminding());
		}

		private void CheckReminding()
		{
			if (!Enabled) return;
			
			var port = KanColleClient.Current.Homeport;
			var maxShipCount = port.Admiral.MaxShipCount;
			var currentShipCount = port.Organization.Ships.Count;
			var reminding = maxShipCount - currentShipCount;
			if (reminding <= WarningCount)
				_plugin.Notify("ShipNotify", "母港空位警告", $"母港仅剩余 {reminding} 空位");
		}
	}
}
