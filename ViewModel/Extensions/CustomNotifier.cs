using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;

namespace ViewModel.Extensions
{
    public class CustomNotifier
    {
        private static Notifier _notifier;
        public Notifier Notifier => _notifier;
        public CustomNotifier()
        {
            _notifier = new Notifier(cfg => { });
        }
        public void SetNewInstance(Notifier notifier)
        {
            _notifier = notifier;
        }
    }
}
