using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAritMscSklRepository
    {
        Task<IEnumerable<MscSkl>> GetAllAsync(string rackCode);
        Task<int> GetMaxRow();
        Task<int> GetMaxColumn();
        void AritMscSklBlock(int idMscSkl);
        void AritMscSklUnBlock(int idMscSkl);
    }
}
