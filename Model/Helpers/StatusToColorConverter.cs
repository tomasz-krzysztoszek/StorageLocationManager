using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Helpers
{
    internal static class StatusToColorConverter
    {
        public static string GetHexByEnum(StatusMscSkl status)
        {
            switch (status)
            {
                case StatusMscSkl.Empty:
                    return "#f7f7f7";
                case StatusMscSkl.Busy:
                    return "#5cb85c";
                case StatusMscSkl.TpsLessThan6Month:
                    return "#f0ad4e";
                case StatusMscSkl.TpsLessThan3Month:
                    return "#d9534f";
                default:
                    return "#f7f7f7";
            }
        }
    }
}
