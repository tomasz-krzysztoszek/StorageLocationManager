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
    public class MainWindowViewModel : ViewModelBase
    {
        public ISplashScreenService SplashScreenService => ServiceContainer.GetService<ISplashScreenService>("WaitIndicatorService");
        public virtual IDialogService EditSettingsWindowDialogService { get { return GetService<IDialogService>("EditSettingsDialogService"); } }

        public IList<Wearehouse> Wearehouses { get; set; }
        public Wearehouse CurrentWearehouse { get; set; }
        public IList<Rack> Racks { get; set; }
        public Rack CurrentRack { get; set; }
        public bool ContolsEnabled { get; set; }
        public string LoggedUserName { get; set; }

        private readonly AppSettings _settings;
        private readonly User _useer;
        private readonly ILogger _logger;
        private readonly IRackService _rackService;
        private readonly IEditSettingsViewModel _editSettingsViewModel;
        private readonly ISessionContext _sessionContext;

        public MainWindowViewModel(
            AppSettings settings,
            User user,
            ILogger logger,
            IRackService rackService,
            IEditSettingsViewModel editSettingsViewModel,
            ISessionContext sessionContext)
        {
            _settings = settings;
            _useer = user;
            _logger = logger;
            _rackService = rackService;
            _editSettingsViewModel = editSettingsViewModel;
            _sessionContext = sessionContext;
            _sessionContext.RackChangedDone += _sessionContext_RackChangedDone;

            ContolsEnabled = true;
            LoggedUserName = _useer.Login;
            var wearehouse = _rackService.GetWearehouseAsync().Result;
            Wearehouses = wearehouse.ToList();
            RaisePropertyChanged(() => Wearehouses);
            RaisePropertyChanged(() => LoggedUserName);
        }

        [AsyncCommand]
        public async Task CurrentWearehouseChange()
        {
            await LoadRacks(CurrentWearehouse.KodMscSkl);
        }
        [AsyncCommand]
        public async Task CurrentRackChange()
        {
            ContolsEnabled = false;
            RaisePropertyChanged(() => ContolsEnabled);
            await _sessionContext.RackChange(CurrentRack);
        }
        [AsyncCommand]
        public async Task EditSettings()
        {
            EditSettingsWindowDialogService.ShowDialog(null, "USTAWIENIA", _editSettingsViewModel);
            if (CurrentRack != null)
                await _sessionContext.RackChange(CurrentRack);
        }

        private void _sessionContext_RackChangedDone()
        {
            ContolsEnabled = true;
            RaisePropertyChanged(() => ContolsEnabled);
        }
        private async Task LoadRacks(string wearehouseNumber)
        {
            SplashScreenService.ShowSplashScreen();
            var racks = await _rackService.GetRacksByWearehouseAsync(wearehouseNumber);
            Racks = racks.ToList();
            RaisePropertyChanged(() => Racks);
            SplashScreenService.HideSplashScreen();
        }
    }
}
