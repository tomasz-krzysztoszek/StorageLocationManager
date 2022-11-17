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
    public class User : INotifyPropertyChanged, IDataErrorInfo
    {
        public int IdAritSlmUzytkownik { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimalna ilość znaków: 4")]
        public string Login { get; set; }
        [Required, MinLength(6, ErrorMessage = "Minimalna ilość znaków: 6")]
        public string Haslo { get; set; }
        public short? Aktywny { get; set; }

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
