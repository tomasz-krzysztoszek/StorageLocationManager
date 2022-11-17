using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Rack : INotifyPropertyChanged
    {
        public int IloscMiejsc { get; set; }
        public int Zajetych { get; set; }
        public int Wolne
        {
            get
            {
                return IloscMiejsc - Zajetych;
            }
        }
        public double ProcZajetych
        {
            get
            {
                if (IloscMiejsc == 0)
                    return 0;
                return Math.Round((double)Zajetych / IloscMiejsc * 100, 2);
            }
        }
        public string KodMag { get; set; }
        public string Regal { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
