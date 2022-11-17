using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Settings
{
    public class AppProfile
    {
        public Statuscolors StatusColors { get; set; }
    }
    public class Statuscolors
    {
        public string Empty { get; set; }
        public string Busy { get; set; }
        public string TpsLessThan6Month { get; set; }
        public string TpsLessThan3Month { get; set; }
    }
}
