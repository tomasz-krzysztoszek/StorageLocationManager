using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Settings
{
    public class PcbRestParam
    {
        public _Parameters[] _parameters { get; set; }
    }

    public class _Parameters
    {
        public Param[] Params { get; set; }
    }

    public class Param
    {
        public string Nazwa { get; set; }
        public int Wartosc { get; set; }
        public bool Wszystkie { get; set; }
    }
}
