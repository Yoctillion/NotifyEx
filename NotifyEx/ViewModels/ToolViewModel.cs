using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using NotifyEx.Models;

namespace NotifyEx.ViewModels
{
	public class ToolViewModel : ViewModel
	{
		private readonly ShipNotifier _shipNotifier;
		private readonly SlotNotifier _slotNotifier;
		private readonly HpNotifier _hpNotifier;

		public bool EnabledShipNotifier
		{
			get { return _shipNotifier.Enabled; }
			set
			{
				if (_shipNotifier.Enabled != value)
				{
					_shipNotifier.Enabled = value;
					RaisePropertyChanged();
				}
			}
		}

		public uint ShipWarningCount
		{
			get { return _shipNotifier.WarningCount; }
			set
			{
				if (_shipNotifier.WarningCount != value)
				{
					_shipNotifier.WarningCount = value;
					RaisePropertyChanged();
				}
			}
		}

		public bool EnabledSlotNotifier
		{
			get { return _slotNotifier.Enabled; }
			set
			{
				if (_slotNotifier.Enabled != value)
				{
					_slotNotifier.Enabled = value;
					RaisePropertyChanged();
				}
			}
		}

		public uint SlotWarningCount
		{
			get { return _slotNotifier.WarningCount; }
			set
			{
				if (_slotNotifier.WarningCount != value)
				{
					_slotNotifier.WarningCount = value;
					RaisePropertyChanged();
				}
			}
		}

		public bool EnabledHpNotifier
		{
			get { return _hpNotifier.Enabled; }
			set
			{
				if (_hpNotifier.Enabled != value)
				{
					_hpNotifier.Enabled = value;
					RaisePropertyChanged();
				}
			}
		}

		public bool EnabledShowDamageControl
		{
			get { return _hpNotifier.EnabledShowDamageControl; }
			set
			{
				if (_hpNotifier.EnabledShowDamageControl != value)
				{
					_hpNotifier.EnabledShowDamageControl = value;
					RaisePropertyChanged();
				}
			}
		}

		public ToolViewModel(Plugin plugin)
		{
			_shipNotifier = new ShipNotifier(plugin);
			_slotNotifier = new SlotNotifier(plugin);
			_hpNotifier = new HpNotifier(plugin);
		}
	}
}
