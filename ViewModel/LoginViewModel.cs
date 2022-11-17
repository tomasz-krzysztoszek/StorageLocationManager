using Application.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Model;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        public bool IsViewVisible { get; set; }
        public User CurrentUser
        {
            get
            {
                RaisePropertyChanged(() => IsDataValid);
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        public bool IsDataValid => _user == null ? false : !IDataErrorInfoHelper.HasErrors(_user);

        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private User _user;

        public LoginViewModel(
            AppSettings settings,
            ILogger logger,
            IUserService userService,
            User user)
        {
            _settings = settings;
            _logger = logger;
            _userService = userService;
            _user = user;
            IsViewVisible = true;
        }

        [Command]
        public async void LogIn()
        {
            var loggedUser = await _userService.GetUserAsync(CurrentUser.Login, CurrentUser.Haslo);
            if (loggedUser != null)
            {
                _user.IdAritSlmUzytkownik = loggedUser.IdAritSlmUzytkownik;
                IsViewVisible = false;
                RaisePropertyChanged(() => IsViewVisible);
            }
            else
            {
                MessageBoxService.ShowMessage("Błędne dane logowania", "UWAGA", MessageButton.OK, MessageIcon.Error);
            }
        }

        [Command]
        public void Close()
        {
            Environment.Exit(0);
        }
    }
}
