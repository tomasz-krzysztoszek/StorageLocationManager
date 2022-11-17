using Model;
using Repository.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAritDostawaRepository : IBaseRepository
    {
        Task<IEnumerable<Delivery>> GetDeliveryByIdMscSklAsync(int idMscSkl);
        Task UpdateAsync(Delivery delivery);
        Task ClearMscSklAsync(int idDelivery);
        Task AddMscSkl(int idDelivery, int idMscSkl);
    }
}
