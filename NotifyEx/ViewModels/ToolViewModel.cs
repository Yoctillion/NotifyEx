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
        private readonly ShipNotifier _shipNotifier = ShipNotifier.Current;
        private readonly SlotNotifier _slotNotifier = SlotNotifier.Current;
        private readonly HpNotifier _hpNotifier = HpNotifier.Current;
        private readonly SupplyNotifier _supplyNotifier = SupplyNotifier.Current;

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
            get { return _shipNotifier.NormalWarningCount; }
            set
            {
                if (_shipNotifier.NormalWarningCount != value)
                {
                    _shipNotifier.NormalWarningCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool EnabledEventShipNotifier
        {
            get { return this._shipNotifier.EnabledEvent; }
            set
            {
                if (this._shipNotifier.EnabledEvent != value)
                {
                    this._shipNotifier.EnabledEvent = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint EventShipWarningCount
        {
            get { return this._shipNotifier.EventWarningCount; }
            set
            {
                if (this._shipNotifier.EventWarningCount != value)
                {
                    this._shipNotifier.EventWarningCount = value;
                    this.RaisePropertyChanged();
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
            get { return _slotNotifier.NormalWarningCount; }
            set
            {
                if (_slotNotifier.NormalWarningCount != value)
                {
                    _slotNotifier.NormalWarningCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool EnabledEventSlotNotifier
        {
            get { return this._slotNotifier.EnabledEvent; }
            set
            {
                if (this._slotNotifier.EnabledEvent != value)
                {
                    this._slotNotifier.EnabledEvent = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint EventSlotWarningCount
        {
            get { return this._slotNotifier.EventWarningCount; }
            set
            {
                if (this._slotNotifier.EventWarningCount != value)
                {
                    this._slotNotifier.EventWarningCount = value;
                    this.RaisePropertyChanged();
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

        public bool EnabledSupplyNotifier
        {
            get { return _supplyNotifier.Enabled; }
            set
            {
                if (_supplyNotifier.Enabled != value)
                {
                    _supplyNotifier.Enabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool EnabledSortieSupplyNotifier
        {
            get { return _supplyNotifier.EnabledSortie; }
            set
            {
                if (_supplyNotifier.EnabledSortie != value)
                {
                    _supplyNotifier.EnabledSortie = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool EnabledExerciseSupplyNotifier
        {
            get { return _supplyNotifier.EnabledExercise; }
            set
            {
                if (_supplyNotifier.EnabledExercise != value)
                {
                    _supplyNotifier.EnabledExercise = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool EnabledExpenditionSupplyNotifier
        {
            get { return _supplyNotifier.EnabledExpendition; }
            set
            {
                if (_supplyNotifier.EnabledExpendition != value)
                {
                    _supplyNotifier.EnabledExpendition = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}
