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
    public class AritMscSklRepository : IAritMscSklRepository
    {
        private readonly AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly FBDBDB _dbContext;

        public AritMscSklRepository(AppSettings settings, FBDBDB dbContext, ILogger logger, IMapper mapper)
        {
            _settings = settings;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode)
        {
            var result = await Task.Run(() => _dbContext
                .AritSlmMiejscaSklViews
                .Where(x => x.REGAL == rackCode)
                .ToList());

            if (result.Any())
                return _mapper.Map<List<MscSkl>>(result);
            else
                return Enumerable.Empty<MscSkl>();
        }
        public async Task<int> GetMaxRow()
        {
            var result = await Task.Run(() => _dbContext
                .AritSlmMiejscaSklViews
                .Max(x => x.POZIOM) ?? 0);
            return result;
        }
        public async Task<int> GetMaxColumn()
        {
            var result = await Task.Run(() => _dbContext
                .AritSlmMiejscaSklViews
                .Max(x => x.KOLUMNA) ?? 0);
            return result;
        }
        public void AritMscSklBlock(int idMscSkl)
        {
            _dbContext.AritSlmBlokadas
                .InsertOrUpdate(
                () =>
                    new AritSlmBlokada()
                    {
                        IdMiejsceskl = idMscSkl,
                        DataBlokady = DateTime.Now
                    },
                u =>
                    new AritSlmBlokada()
                    {
                        DataBlokady = DateTime.Now
                    }
                );
        }
        public void AritMscSklUnBlock(int idMscSkl)
        {
            _dbContext.AritSlmBlokadas
                .Where(x => x.IdMiejsceskl == idMscSkl)
                .Delete();
        }
    }
}
