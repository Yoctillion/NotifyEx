using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace NotifyEx.Models.Settings
{
    public static class NotifierSettings
    {
        #region Ship count notifier settings

        public static SerializableProperty<bool> EnabledShipNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledShipNotifier) { AutoSave = true };

        public static SerializableProperty<uint> ShipWarningCount { get; }
            = new SerializableProperty<uint>(GetKey(), Provider.Roaming, Properties.Settings.Default.WarningShipCount) { AutoSave = true };

        public static SerializableProperty<bool> EnabledEventShipNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledEventShipNotifier) { AutoSave = true };

        public static SerializableProperty<uint> EventShipWarningCount { get; }
            = new SerializableProperty<uint>(GetKey(), Provider.Roaming, Properties.Settings.Default.EventWarningShipCount) { AutoSave = true };

        #endregion

        #region Slot count notifier settings

        public static SerializableProperty<bool> EnabledSlotNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledSlotNotifier) { AutoSave = true };

        public static SerializableProperty<uint> SlotWarningCount { get; }
            = new SerializableProperty<uint>(GetKey(), Provider.Roaming, Properties.Settings.Default.WarningShipCount) { AutoSave = true };

        public static SerializableProperty<bool> EnabledEventSlotNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledEventSlotNotifier) { AutoSave = true };

        public static SerializableProperty<uint> EventSlotWarningCount { get; }
            = new SerializableProperty<uint>(GetKey(), Provider.Roaming, Properties.Settings.Default.EventWarningSlotCount) { AutoSave = true };

        #endregion

        #region Heavily damaged notifier settings

        public static SerializableProperty<bool> EnabledLowHpNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledLowHpNotifier) { AutoSave = true };

        public static SerializableProperty<bool> EnabledShowDamageControl { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledShowDamageControl) { AutoSave = true };

        #endregion

        #region Lack of supply notifier

        public static SerializableProperty<bool> EnabledSupplyNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledSupplyNotifier) { AutoSave = true };

        public static SerializableProperty<bool> EnabledSortieSupplyNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledSortieSupplyNotifier) { AutoSave = true };

        public static SerializableProperty<bool> EnabledExerciseSupplyNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledExerciseSupplyNotifier) { AutoSave = true };

        public static SerializableProperty<bool> EnabledExpenditionSupplyNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, Properties.Settings.Default.EnabledExpenditionSupplyNotifier) { AutoSave = true };

        #endregion

        #region Land base

        public static SerializableProperty<bool> EnabledLandBaseNotifier { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, true) { AutoSave = true };

        public static SerializableProperty<bool> ShowLandBaseNotificationBeforeSelectMap { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, false) { AutoSave = true };

        public static SerializableProperty<bool> ShowUncompletedAirCorps { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, true) { AutoSave = true };

        public static SerializableProperty<bool> ShowNotReadyAirCorps { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, true) { AutoSave = true };

        public static SerializableProperty<bool> ShowNeedSupplyAirCorps { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, true) { AutoSave = true };

        public static SerializableProperty<bool> ShowTiredAirCorps { get; }
            = new SerializableProperty<bool>(GetKey(), Provider.Roaming, true) { AutoSave = true };

        #endregion

        private static string GetKey([CallerMemberName] string propertyName = "")
        {
            return $"{nameof(NotifierSettings)}.{propertyName}";
        }
    }
}
