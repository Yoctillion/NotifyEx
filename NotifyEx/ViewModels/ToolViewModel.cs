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
