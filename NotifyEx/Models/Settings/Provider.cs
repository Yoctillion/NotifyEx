using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace NotifyEx.Models.Settings
{
    public static class Provider
    {
        public static string RoamingPath { get; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData),
            "grabacr.net", "KanColleViewer", "NotifyEx.xaml");

        public static ISerializationProvider Roaming { get; } = new FileSettingsProvider(RoamingPath);
    }
}
