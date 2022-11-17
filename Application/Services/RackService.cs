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
    public interface IRackService
    {
        Task<IEnumerable<Wearehouse>> GetWearehouseAsync();
        Task<Rack> GetRackByNumber(string wearehouseNumber, string rackNumber);
        Task<IEnumerable<Rack>> GetRacksByWearehouseAsync(string wearehouseNumber);
    }
    public class RackService : IRackService
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IAritRackRepository _aritRackRepository;

        public RackService(AppSettings settings, ILogger logger, IAritRackRepository aritRackRepository)
        {
            _settings = settings;
            _logger = logger;
            _aritRackRepository = aritRackRepository;
        }

        public async Task<Rack> GetRackByNumber(string wearehouseNumber, string rackNumber)
        {
            var result = await _aritRackRepository.GetRackByNumber(wearehouseNumber, rackNumber);
            return result;
        }

        public async Task<IEnumerable<Rack>> GetRacksByWearehouseAsync(string wearehouseNumber)
        {
            var result = await _aritRackRepository.GetRacksByWearehouseAsync(wearehouseNumber);
            return result;
        }

        public async Task<IEnumerable<Wearehouse>> GetWearehouseAsync()
        {
            var result = await _aritRackRepository.GetWearehouseAsync();
            return result;
        }
    }
}
