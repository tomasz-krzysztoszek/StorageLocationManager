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
    public interface IEditMscSklService
    {
        Task<IEnumerable<Delivery>> GetDeliveryByIdMscSklAsync(int idMscSkl);
        Task UpdateDelivery(Delivery delivery);
        Task ChangeMscSklAsync(IEnumerable<Delivery> delivery, int idMscSkl);
    }
    public class EditMscSklService : IEditMscSklService
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IAritMscSklRepository _aritMscSklRepository;
        private readonly IAritDostawaRepository _aritDostawaRepository;

        public EditMscSklService(
            AppSettings settings,
            ILogger logger,
            IAritMscSklRepository aritMscSklRepository,
            IAritDostawaRepository aritDostawaRepository)
        {
            _settings = settings;
            _logger = logger;
            _aritMscSklRepository = aritMscSklRepository;
            _aritDostawaRepository = aritDostawaRepository;
        }

        public async Task ChangeMscSklAsync(IEnumerable<Delivery> delivery, int idMscSkl)
        {
            await _aritDostawaRepository.BeginTransactionAsync();
            try
            {
                foreach(var item in delivery)
                {
                    await _aritDostawaRepository.ClearMscSklAsync(item.IdDostawa);
                    await _aritDostawaRepository.AddMscSkl(item.IdDostawa, idMscSkl);
                }
                await _aritDostawaRepository.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                await _aritDostawaRepository.RollbackTransactionAsync();
                _logger.Log(LogLevel.Error, ex);
            }
        }

        public async Task<IEnumerable<Delivery>> GetDeliveryByIdMscSklAsync(int idMscSkl)
        {
            var result = await _aritDostawaRepository.GetDeliveryByIdMscSklAsync(idMscSkl);
            return result;
        }

        public async Task UpdateDelivery(Delivery delivery)
        {
            await _aritDostawaRepository.UpdateAsync(delivery);
        }
    }
}
