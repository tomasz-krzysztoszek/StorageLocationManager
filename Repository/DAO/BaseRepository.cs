using DAO.DataModels;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAO
{
    public interface IBaseRepository
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
    public class BaseRepository : IBaseRepository
    {
        private readonly FBDBDB _dbContext;
        private readonly ILogger _logger;

        public BaseRepository(FBDBDB dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task BeginTransactionAsync()
        {
            await _dbContext.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.CommitTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            await _dbContext.RollbackTransactionAsync();
        }
    }
}
