using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MscSklSearchParameters : INotifyPropertyChanged
    {
        public DateTime? TpsOd { get; set; }
        public DateTime? TpsDo { get; set; }
        public bool Indeks { get; set; }
        public bool NazwaSkr { get; set; }
        public bool NazwaDl { get; set; }
        public string SearchPharse { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
