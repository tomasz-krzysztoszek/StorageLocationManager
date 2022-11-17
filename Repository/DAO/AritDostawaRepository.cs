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
    public class AritDostawaRepository : BaseRepository, IAritDostawaRepository
    {
        private readonly AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly FBDBDB _dbContext;

        public AritDostawaRepository(AppSettings settings, FBDBDB dbContext, ILogger logger, IMapper mapper) : base(dbContext, logger)
        {
            _settings = settings;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddMscSkl(int idDelivery, int idMscSkl)
        {
            await _dbContext.Dostawas
                .Where(x => x.IdDostawa == idDelivery)
                .Set(x => x.IdMiejsceskl, () => idMscSkl)
                .UpdateAsync();
        }

        public async Task ClearMscSklAsync(int idDelivery)
        {
            await _dbContext.Dostawas
                .Where(x => x.IdDostawa == idDelivery)
                .Set(x => x.IdMiejsceskl, () => null)
                .UpdateAsync();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveryByIdMscSklAsync(int idMscSkl)
        {
            var result = _dbContext
                .AritSlmDostawaViews
                .Where(x => x.IdMiejsceskl == idMscSkl)
                .ToListAsync().Result;

            if (result.Any())
                return _mapper.Map<List<Delivery>>(result);
            else
                return Enumerable.Empty<Delivery>();
        }

        public async Task UpdateAsync(Delivery delivery)
        {
            await Task.Run(() =>
                _dbContext.AritSlmEditDostawa(
                    delivery.IdDostawa,
                    delivery.TPS,
                    delivery.Partia,
                    delivery.DataUboju,
                    delivery.DataMrozenia,
                    delivery.TempPrzechowywania)
            );
        }
    }
}
