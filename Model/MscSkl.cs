using Model.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum StatusMscSkl
    {
        Undefined = 0,
        Empty = 1,
        Busy = 2,
        TpsLessThan6Month = 4,
        TpsLessThan3Month = 5
    }
    public class MscSkl : INotifyPropertyChanged
    {
        public int IdMiejsceSkl { get; set; }
        public string KodMSkl { get; set; }
        public string Regal { get; set; }
        public int Poziom { get; set; }
        public int Kolumna { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(); }
        }
        private bool _isBlocked;
        public bool IsBlocked
        {
            get { return _isBlocked; }
            set { _isBlocked = value; NotifyPropertyChanged("BlockedVisibility"); }
        }
        //TODO:Do zmiany na converter
        public string BlockedVisibility
        {
            get { return _isBlocked ? "Visible" : "Hidden"; }
        }
        private StatusMscSkl _status;
        public StatusMscSkl Status
        {
            get { return _status; }
            set{ _status = value; NotifyPropertyChanged(); }
        }
        public Delivery MaxTpsDelivery
        {
            get { return Deliverys.OrderBy(x => x.TPS).FirstOrDefault(); }
        }
        public IEnumerable<Delivery> Deliverys { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
