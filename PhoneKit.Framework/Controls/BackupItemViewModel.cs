using PhoneKit.Framework.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Controls
{
    public class BackupItemViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public DateTime BackupDate { get; set; }
    }
}
