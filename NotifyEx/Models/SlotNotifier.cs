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
	public class SlotNotifier : NotificationObject
	{
		private static readonly Settings Settings = Settings.Default;

		private readonly Plugin _plugin;

		public bool Enabled
		{
			get { return Settings.EnabledSlotNotifier; }
			set
			{
				if (Settings.EnabledSlotNotifier != value)
				{
					Settings.EnabledSlotNotifier = value;
					Settings.Save();
					RaisePropertyChanged();
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
					RaisePropertyChanged();
				}
			}
		}

		public SlotNotifier(Plugin plugin)
		{
			_plugin = plugin;

			var proxy = KanColleClient.Current.Proxy;
			proxy.ApiSessionSource.Where(s => s.Request.PathAndQuery.StartsWith("/kcsapi/api_get_member/mapinfo"))
				.Subscribe(x => CheckReminding());
		}

		private void CheckReminding()
		{
			var port = KanColleClient.Current.Homeport;
			var max = port.Admiral.MaxSlotItemCount;
			var current = port.Itemyard.SlotItemsCount;
			var reminding = max - current;
			if (reminding <= WarningCount && Enabled)
			{
				_plugin.Notify("SlotNotify", "警告", $"装备剩余 {reminding} 空位");
			}
		}
	}
}
