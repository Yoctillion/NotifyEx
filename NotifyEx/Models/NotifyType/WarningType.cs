using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyEx.Models.NotifyType
{
    public sealed class WarningType : INotifyType
    {
        public static WarningType Instance { get; } = new WarningType();

        private WarningType() {}

        public string Name { get; } = "警告";
    }
}
