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
    public interface IMscSklService
    {
        Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode);
        Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode, MscSklSearchParameters searchParams);
        void AritMscSklBlock(int idMscSkl);
        void AritMscSklUnBlock(int idMscSkl);
    }
    public class MscSklService : IMscSklService
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly IAritMscSklRepository _aritMscSklRepository;
        private readonly IAritDostawaRepository _aritDostawaRepository;

        public MscSklService(
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

        public void AritMscSklBlock(int idMscSkl)
        {
            _aritMscSklRepository.AritMscSklBlock(idMscSkl);
        }

        public void AritMscSklUnBlock(int idMscSkl)
        {
            _aritMscSklRepository.AritMscSklUnBlock(idMscSkl);
        }

        public async Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode)
        {
            var result = await _aritMscSklRepository.GetAllAsync(rackCode);
            var rowMax = await _aritMscSklRepository.GetMaxRow();
            var columnMax = await _aritMscSklRepository.GetMaxColumn();

            await PrepareDataVisualization(result, rowMax, columnMax);
            return result;
        }

        public async Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode, MscSklSearchParameters searchParams)
        {
            var result = await _aritMscSklRepository.GetAllAsync(rackCode);
            var rowMax = await _aritMscSklRepository.GetMaxRow();
            var columnMax = await _aritMscSklRepository.GetMaxColumn();

            await PrepareDataVisualization(result, rowMax, columnMax);

            var resultQuery = result.AsQueryable();

            if (searchParams != null)
            {
                if (searchParams.TpsOd.HasValue)
                    resultQuery = resultQuery.Where(x => x.Deliverys.Where(d => d.TPS > searchParams.TpsOd.Value).Any());
                if (searchParams.TpsDo.HasValue)
                    resultQuery = resultQuery.Where(x => x.Deliverys.Where(d => d.TPS < searchParams.TpsDo.Value).Any());
                if (searchParams.Indeks)
                    resultQuery = resultQuery.Where(x => x.Deliverys.Where(d => d.Indeks.ToUpper().Contains(searchParams.SearchPharse.ToUpper())).Any());
                if (searchParams.NazwaSkr)
                    resultQuery = resultQuery.Where(x => x.Deliverys.Where(d => d.NazwaSkr.ToUpper().Contains(searchParams.SearchPharse.ToUpper())).Any());
                if (searchParams.NazwaDl)
                    resultQuery = resultQuery.Where(x => x.Deliverys.Where(d => d.NazwaSkr.ToUpper().Contains(searchParams.SearchPharse.ToUpper())).Any());
            }

            return resultQuery.ToList();
        }

        private async Task<IEnumerable<MscSkl>> PrepareDataVisualization(IEnumerable<MscSkl> lstMscSkl, int rowMax, int columnMax)
        {
            var space = 5;
            var size = 100;
            for (int row = rowMax; row >= 0; row--)
            {
                for (int column = 0; column <= columnMax; column++)
                {
                    var currentMscSkl = lstMscSkl
                        .Where(x => x.Kolumna == column && x.Poziom == row)
                        .SingleOrDefault();

                    if (currentMscSkl != null)
                    {
                        currentMscSkl.Height = size;
                        currentMscSkl.Width = size;
                        currentMscSkl.X = (column - 1) * (size + space);
                        currentMscSkl.Y = row * (size + space);
                        currentMscSkl.Status = StatusMscSkl.Empty;
                        currentMscSkl.Deliverys = await _aritDostawaRepository.GetDeliveryByIdMscSklAsync(currentMscSkl.IdMiejsceSkl);
                        if (currentMscSkl.Deliverys.Any())
                        {
                            if (currentMscSkl.Deliverys.Where(x => x.TPS < DateTime.Now.AddMonths(3)).Any())
                                currentMscSkl.Status = StatusMscSkl.TpsLessThan3Month;
                            else if (currentMscSkl.Deliverys.Where(x => x.TPS < DateTime.Now.AddMonths(6)).Any())
                                currentMscSkl.Status = StatusMscSkl.TpsLessThan6Month;
                            else
                                currentMscSkl.Status = StatusMscSkl.Busy;
                        }
                    }
                }
            }

            return lstMscSkl;
        }
    }
}
