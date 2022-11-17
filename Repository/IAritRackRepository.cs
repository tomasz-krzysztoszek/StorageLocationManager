using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAritRackRepository
    {
        Task<IEnumerable<Wearehouse>> GetWearehouseAsync();
        Task<Rack> GetRackByNumber(string wearehouseNumber, string rackNumber);
        Task<IEnumerable<Rack>> GetRacksByWearehouseAsync(string wearehouseNumber);
    }
}
