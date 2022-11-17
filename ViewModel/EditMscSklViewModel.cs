using Application.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Model;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helpers;

namespace ViewModel
{
    public interface IEditMscSklViewModel
    {

    }
    public class EditMscSklViewModel : ViewModelBase, IEditMscSklViewModel
    {
        protected ICurrentWindowService CurrentWindowService { get { return GetService<ICurrentWindowService>(); } }
        public IList<Delivery> Deliverys { get; set; }
        private Delivery _currentDelivery;
        public Delivery CurrentDelivery
        {
            get
            {
                RaisePropertyChanged(() => IsDataValid);
                return _currentDelivery;
            }
            set
            {
                _currentDelivery = value;
            }
        }
        public bool IsDataValid => _currentDelivery == null ? false : !IDataErrorInfoHelper.HasErrors(_currentDelivery);

        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IEditMscSklService _editMscSklService;
        private readonly ISessionContext _sessionContext;

        public EditMscSklViewModel(
            AppSettings settings,
            ILogger logger,
            IEditMscSklService editMscSklService,
            ISessionContext sessionContext)
        {
            _settings = settings;
            _logger = logger;
            _editMscSklService = editMscSklService;
            _sessionContext = sessionContext;
            _sessionContext.MscSklEdited += _sessionContext_MscSklEdited;
        }

        private void _sessionContext_MscSklEdited(int idMscSkl)
        {
            var lstResult = _editMscSklService.GetDeliveryByIdMscSklAsync(idMscSkl).Result;

            Deliverys = lstResult.ToList();

            RaisePropertyChanged(() => Deliverys);
        }

        [Command]
        public void CurrentDeliveryChange()
        {
            RaisePropertyChanged(() => CurrentDelivery);
        }

        [Command]
        public void Accept()
        {
            if(CurrentDelivery != null)
            {
                _editMscSklService.UpdateDelivery(CurrentDelivery);
            }
        }

        [Command]
        public void Cancel()
        {
            CurrentWindowService.Close();
        }
    }
}
