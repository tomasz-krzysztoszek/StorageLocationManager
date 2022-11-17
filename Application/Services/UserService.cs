using Application.Helpers;
using Model;
using Model.Settings;
using NLog;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string login, string password); 
    }
    public class UserService : IUserService
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IAritUzytkownikRepository _aritUsytkownikRepository;

        public UserService(AppSettings settings, ILogger logger, IAritUzytkownikRepository aritUsytkownikRepository)
        {
            _settings = settings;
            _logger = logger;
            _aritUsytkownikRepository = aritUsytkownikRepository;
        }

        public async Task<User> GetUserAsync(string login, string password)
        {
            var encypredPassword = PasswordManager.Encrypt(password);
            return await _aritUsytkownikRepository.GetUserAsync(login, encypredPassword);
        }
    }
}
