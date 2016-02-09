using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace NotifyEx.Models
{
    public interface IWarningCounter : INotifyPropertyChanged
    {
        bool Enabled { get; }
        uint WarningCount { get; }
        uint Remain { get; }
    }
}
