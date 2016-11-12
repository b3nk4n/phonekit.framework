using PhoneKit.Framework.Core.MVVM;
using System;

namespace PhoneKit.Framework.Controls
{
    public class BackupItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public DateTime BackupDate { get; set; }
    }
}
