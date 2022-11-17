using AutoMapper;
using DAO.DataModels;
using LinqToDB;
using Model;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAO
{
    public class AritUzytkownikRepository : IAritUzytkownikRepository
    {
        private readonly AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly FBDBDB _dbContext;

        public AritUzytkownikRepository(AppSettings settings, FBDBDB dbContext, ILogger logger, IMapper mapper)
        {
            _settings = settings;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<User> GetUserAsync(string login, string password)
        {
            var result = await _dbContext
                .AritSlmUzytkowniks
                .Where(x => x.LOGIN == login && x.HASLO == password)
                .ToListAsync();

            if (result.Any())
                return _mapper.Map<User>(result.FirstOrDefault());
            else
                return null;
        }
    }
}
