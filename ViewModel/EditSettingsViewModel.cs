using Application.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public interface IEditSettingsViewModel
    {

    }
    public class EditSettingsViewModel : ViewModelBase, IEditSettingsViewModel
    {
        protected ICurrentWindowService CurrentWindowService { get { return GetService<ICurrentWindowService>(); } }

        private Statuscolors _statusColors;
        public string EmptyColor
        {
            get
            {
                return _statusColors.Empty;
            }
            set
            {
                _statusColors.Empty = value;
            }
        }
        public string BusyColor
        {
            get
            {
                return _statusColors.Busy;
            }
            set
            {
                _statusColors.Busy = value;
            }
        }
        public string TpsLessThan6MonthColor
        {
            get
            {
                return _statusColors.TpsLessThan6Month;
            }
            set
            {
                _statusColors.TpsLessThan6Month = value;
            }
        }
        public string TpsLessThan3MonthColor
        {
            get
            {
                return _statusColors.TpsLessThan3Month;
            }
            set
            {
                _statusColors.TpsLessThan3Month = value;
            }
        }

        private readonly AppProfile _profile;
        private readonly ILogger _logger;
        private readonly IEditSettingsService _editSettingsService;
        public EditSettingsViewModel(
            AppProfile profile,
            ILogger logger,
            IEditSettingsService editSettingsService
            )
        {
            _profile = profile;
            _statusColors = _profile.StatusColors;
            _logger = logger;
            _editSettingsService = editSettingsService;
        }

        [Command]
        public void ReturnDefault()
        {
            _statusColors.Empty = "#f7f7f7";
            _statusColors.Busy = "#5cb85c";
            _statusColors.TpsLessThan6Month = "#f0ad4e";
            _statusColors.TpsLessThan3Month = "#d9534f";
            RaisePropertiesChanged(
                nameof(EmptyColor),
                nameof(BusyColor),
                nameof(TpsLessThan6MonthColor),
                nameof(TpsLessThan3MonthColor));
        }
        [Command]
        public void Accept()
        {
            try
            {
                _editSettingsService.SaveAppProfile(_profile);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }
            CurrentWindowService.Close();
        }
        [Command]
        public void Cancel()
        {
            CurrentWindowService.Close();
        }
    }
}
