using DevExpress.Mvvm;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helpers;

namespace ViewModel
{
    public interface IPdfViewerViewModel
    {

    }
    public class PdfViewerViewModel : ViewModelBase, IPdfViewerViewModel
    {
        public Stream DocSrcStream
        {
            get { return GetProperty(() => DocSrcStream); }
            set { SetProperty(() => DocSrcStream, value); }
        }

        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly ISessionContext _sessionContext;

        public PdfViewerViewModel(
            AppSettings settings,
            ILogger logger,
            ISessionContext sessionContext)
        {
            _settings = settings;
            _logger = logger;
            _sessionContext = sessionContext;
            _sessionContext.PdfSrcChanged += _sessionContext_PdfSrcChanged;
        }

        private void _sessionContext_PdfSrcChanged(byte[] obj)
        {
            DocSrcStream = new MemoryStream(obj);
        }
    }
}
