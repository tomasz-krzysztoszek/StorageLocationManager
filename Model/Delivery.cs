using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Delivery : INotifyPropertyChanged, IDataErrorInfo
	{
        public int IdMiejsceskl { get; set; }
		public int IdDostawa { get; set; }
		public int IdPoz { get; set; }
		public int IdKartoteka { get; set; }
		public string KodMsKl { get; set; }
		public string Indeks { get; set; }
		public string NazwaSkr { get; set; }
		public DateTime? TPS { get; set; }
		[Required, MaxLength(60, ErrorMessage = "Pole wymagane. Maksymalna ilość znaków: 25")]
		public string Partia { get; set; }
		public decimal WagaNetto { get; set; }
		public string WagaBrutto { get; set; }
		public string DataUboju { get; set; }
		public string DataMrozenia { get; set; }
		public string RodzajOpakowania { get; set; }
		public string IloscOpakowan { get; set; }
		public string RodzajPalety { get; set; }
		public string TempPrzechowywania { get; set; }
        public int SumaOpk { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		public string Error => string.Empty;
		public string this[string columnName] => IDataErrorInfoHelper.GetErrorText(this, columnName);
	}
}
