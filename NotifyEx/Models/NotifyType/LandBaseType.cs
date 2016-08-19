using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyEx.Models.NotifyType
{
    public class LandBaseType : INotifyType
    {
        public static LandBaseType Instance { get; } = new LandBaseType();

        public string Name => "基地航空队";

        private LandBaseType() { }
    }
}
