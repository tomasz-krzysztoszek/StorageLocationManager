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
using System.Threading;
using System.Threading.Tasks;
using ToastNotifications.Messages;
using ViewModel.Extensions;
using ViewModel.Helpers;

namespace ViewModel
{
    public class RackVisualizationViewModel : ViewModelBase
    {
        public virtual IDialogService EditMscSklWindowDialogService { get { return GetService<IDialogService>("EditMscSklDialogService"); } }
        public virtual IDialogService PrintPdfWindowDialogService { get { return GetService<IDialogService>("PrintPdfDialogService"); } }
        public ISplashScreenService SplashScreenService => ServiceContainer.GetService<ISplashScreenService>("WaitIndicatorService");

        public IList<MscSkl> MscSkls { get; set; }
        public MscSkl CurrentMscSkl { get; set; }
        public Rack CurrentRack { get; set; }
        public IEnumerable<Delivery> MscSklToChange { get; set; }
        public MscSklSearchParameters SearchParams { get; set; }
        public Statuscolors Colors => _profile.StatusColors;

        public bool IsMscSklChangeDisabled => MscSklToChange == null;

        private readonly AppSettings _settings;
        private readonly AppProfile _profile;
        private readonly CustomNotifier _notifier;
        private readonly ILogger _logger;
        private readonly IMscSklService _mscSklService;
        private readonly IEditMscSklService _editMscSklService;
        private readonly ISessionContext _sessionContext;
        private readonly IEditMscSklViewModel _editMscSklViewModel;
        private readonly IPdfViewerViewModel _pdfViewerViewModel;
        private readonly IPrintDocService _printDocService;

        public RackVisualizationViewModel(
            AppSettings settings,
            AppProfile profile,
            CustomNotifier notifier,
            ILogger logger,
            IMscSklService mscSklService,
            IEditMscSklService editMscSklService,
            ISessionContext sessionContext,
            IEditMscSklViewModel editMscSklViewModel,
            IPdfViewerViewModel pdfViewerViewModel,
            IPrintDocService printDocService)
        {
            _settings = settings;
            _profile = profile;
            _notifier = notifier;
            _logger = logger;
            _mscSklService = mscSklService;
            _editMscSklService = editMscSklService;
            _editMscSklViewModel = editMscSklViewModel;
            _pdfViewerViewModel = pdfViewerViewModel;
            _sessionContext = sessionContext;
            _sessionContext.RackChanged += _sessionContext_RackChanged;
            _printDocService = printDocService;
            SearchParams = new MscSklSearchParameters();
        }

        private async Task _sessionContext_RackChanged(Rack rack)
        {
            if (CurrentRack != rack)
            {
                CurrentRack = rack;
                await DrawRackAsync(rack.Regal, false);
            }
            foreach (var mscskl in MscSkls)
                mscskl.NotifyPropertyChanged(nameof(mscskl.Status));
            RaisePropertyChanged(() => MscSkls);
        }
        public async Task DrawRackAsync(string rackCode, bool witchSearchParams)
        {
            SplashScreenService.ShowSplashScreen();
            IEnumerable<MscSkl> lstResult;
            if (witchSearchParams)
                lstResult = await _mscSklService.GetAllAsync(rackCode, SearchParams);
            else
                lstResult = await _mscSklService.GetAllAsync(rackCode);
            MscSkls = lstResult.ToList();
            RaisePropertyChanged(() => MscSkls);
            await RefreshStats();
            CurrentMscSkl = null;
            SplashScreenService.HideSplashScreen();
            _sessionContext.RackChangeDone();
        }
        [Command]
        public void CurrentMscSklChanged(MscSkl sender)
        {
            if (CurrentMscSkl != null)
                CurrentMscSkl.IsSelected = !CurrentMscSkl.IsSelected;
            sender.IsSelected = !sender.IsSelected;
            CurrentMscSkl = sender;
            RaisePropertyChanged(() => CurrentMscSkl);
            RefreshStats();
        }
        [AsyncCommand]
        public async Task MscSklEditAsync(MscSkl sender)
        {
            if (MscSklToChange == null)
            {
                _sessionContext.MscSklEdit(sender.IdMiejsceSkl);
                EditMscSklWindowDialogService.ShowDialog(null, "EDYTUJ", _editMscSklViewModel);
            }
            else
            {
                if(sender.Status != StatusMscSkl.Empty)
                {
                    _notifier.Notifier.ShowWarning($"Miejsce zajęte");
                    return;
                }
                await _editMscSklService.ChangeMscSklAsync(MscSklToChange, sender.IdMiejsceSkl);
                MscSklToChange = null;
                await Refresh();
                RaisePropertyChanged(() => IsMscSklChangeDisabled);
                _notifier.Notifier.ShowInformation($"Zmieniono miejsce na: {sender.KodMSkl}");
            }
            await RefreshStats();
        }
        [Command]
        public void MscSklBlock(MscSkl sender)
        {
            if (CurrentMscSkl != null)
            {
                _mscSklService.AritMscSklBlock(CurrentMscSkl.IdMiejsceSkl);
                CurrentMscSkl.IsBlocked = true;
                _notifier.Notifier.ShowWarning($"Zablokowano miejsce: {CurrentMscSkl.KodMSkl}");
            }
        }
        [Command]
        public void MscSklUnBlock(MscSkl sender)
        {
            if (CurrentMscSkl != null)
            {
                _mscSklService.AritMscSklUnBlock(CurrentMscSkl.IdMiejsceSkl);
                CurrentMscSkl.IsBlocked = false;
                _notifier.Notifier.ShowWarning($"Odblokowano miejsce: {CurrentMscSkl.KodMSkl}");
            }
        }
        [AsyncCommand]
        public async Task ChangeMscSkl()
        {
            if(CurrentMscSkl == null || CurrentMscSkl.Status == StatusMscSkl.Empty)
            {
                _notifier.Notifier.ShowWarning($"Miejsce puste");
                return;
            }
            MscSklToChange = await _editMscSklService.GetDeliveryByIdMscSklAsync(CurrentMscSkl.IdMiejsceSkl);
            RaisePropertyChanged(() => IsMscSklChangeDisabled);
            _notifier.Notifier.ShowInformation($"Miejsce do zmiany: {CurrentMscSkl.KodMSkl}");
        }
        [AsyncCommand]
        public async Task Refresh()
        {
            if (CurrentRack != null)
            {
                await DrawRackAsync(CurrentRack.Regal, false);
                MscSklToChange = null;
                RaisePropertyChanged(() => IsMscSklChangeDisabled);
            }
        }
        [AsyncCommand]
        public async Task Print()
        {
            if (CurrentMscSkl != null)
            {
                var result = await _printDocService.PrintLabel(CurrentMscSkl.IdMiejsceSkl);
                _sessionContext.PdfSrcChange(result);
                PrintPdfWindowDialogService.ShowDialog(null, "DRUKUJ", _pdfViewerViewModel);
            }
        }
        [AsyncCommand]
        public async Task Search()
        {
            await DrawRackAsync(CurrentRack.Regal, true);
        }
        [AsyncCommand]
        public async Task ClearSearch()
        {
            await DrawRackAsync(CurrentRack.Regal, false);
        }

        private async Task RefreshStats()
        {
            CurrentRack.Zajetych = MscSkls.Where(x => x.Status != StatusMscSkl.Empty).Count();
            RaisePropertyChanged(() => CurrentRack);
        }
    }
}
