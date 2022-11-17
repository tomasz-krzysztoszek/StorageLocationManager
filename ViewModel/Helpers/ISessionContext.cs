using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Helpers
{
    public interface ISessionContext
    {
        event Func<Rack, Task> RackChanged;
        Task RackChange(Rack rackCode);
        event Action RackChangedDone;
        void RackChangeDone();
        event Action<int> MscSklEdited;
        void MscSklEdit(int idMscSkl);
        event Action<byte[]> PdfSrcChanged;
        void PdfSrcChange(byte[] pdfBuffer);
    }
}
