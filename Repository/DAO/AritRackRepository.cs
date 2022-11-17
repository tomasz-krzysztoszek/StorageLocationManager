using AutoMapper;
using DAO.DataModels;
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
    public class AritRackRepository : IAritRackRepository
    {
        private readonly AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly FBDBDB _dbContext;

        public AritRackRepository(AppSettings settings, FBDBDB dbContext, ILogger logger, IMapper mapper)
        {
            _settings = settings;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Rack> GetRackByNumber(string wearehouseNumber, string rackNumber)
        {
            var result = await Task.Run(() => _dbContext
                .AritSlmRackViews
                .Where(x => x.REGAL == rackNumber && x.KodMag == wearehouseNumber)
                .ToList());

            if (result.Any())
                return _mapper.Map<Rack>(result.First());
            else
                return new Rack();
        }

        public async Task<IEnumerable<Rack>> GetRacksByWearehouseAsync(string wearehouseNumber)
        {
            var result = await Task.Run(() => _dbContext
                .AritSlmRackViews
                .Where(x => x.KodMag == wearehouseNumber)
                .ToList());

            if (result.Any())
                return _mapper.Map<List<Rack>>(result);
            else
                return Enumerable.Empty<Rack>();
        }

        public async Task<IEnumerable<Wearehouse>> GetWearehouseAsync()
        {
            var result = Task.Run(() => _dbContext
                .AritSlmMagazynViews
                .ToList()).Result;

            if (result.Any())
                return _mapper.Map<List<Wearehouse>>(result);
            else
                return Enumerable.Empty<Wearehouse>();
        }
    }
}
