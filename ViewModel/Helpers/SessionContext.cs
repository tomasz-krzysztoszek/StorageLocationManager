using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Helpers
{
    public class SessionContext : ISessionContext
    {

        public event Func<Rack, Task> RackChanged;
        public event Action RackChangedDone;
        public event Action<int> MscSklEdited;
        public event Action<byte[]> PdfSrcChanged;

        public async Task RackChange(Rack rack)
        {
            await RackChanged?.Invoke(rack);
        }
        public void RackChangeDone()
        {
            RackChangedDone?.Invoke();
        }

        public void MscSklEdit(int idMscSkl)
        {
            MscSklEdited?.Invoke(idMscSkl);
        }

        public void PdfSrcChange(byte[] pdfBuffer)
        {
            PdfSrcChanged?.Invoke(pdfBuffer);
        }
    }
}
