using Flurl;
using Flurl.Http;
using Model.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IPrintDocService
    {
        Task<byte[]> PrintLabel(int id);
    }
    public class PrintDocService : IPrintDocService
    {
        private readonly AppSettings _settings;
        private readonly PcbRestParam _pcbRestParam;
        private readonly ILogger _logger;

        public PrintDocService(AppSettings settings, PcbRestParam pcbRestParam, ILogger logger)
        {
            _settings = settings;
            _pcbRestParam = pcbRestParam;
            _logger = logger;
        }

        public async Task<byte[]> PrintLabel(int id)
        {
            if (_pcbRestParam != null)
                _pcbRestParam._parameters[0].Params[0].Wartosc = id;
            else
                throw new Exception($"PrintOffer pcbRestParam from appsettings.json == null");

            var response =
                await _settings.PcbRestSerwerUrl
                .AppendPathSegment("\"PrintRB\"")
                .AppendPathSegment(_settings.PcbRestSerwerIdRaportRb)
                .AppendPathSegment($"PDF:{_settings.PcbRestSerwerFilesPath}\\{id}.pdf")
                .AppendPathSegment("/")
                .WithHeader("Accept", "application/json")
                .WithBasicAuth(_settings.PcbRestSerwerUsername, _settings.PcbRestSerwerPassword)
                .PostJsonAsync(_pcbRestParam);

            if (response != null)
            {
                if (response.ResponseMessage.Headers.Contains("Pragma"))
                    await EndSession(response.ResponseMessage.Headers.Pragma.ToString());
                if (response.StatusCode != 200)
                    throw new Exception($"EndSession error:  Code {response.StatusCode} - {response.ResponseMessage}");
            }
            else
                throw new Exception($"PrintOffer error response == null");

            _logger.Log(LogLevel.Info, $"PrintOffer {_settings.PcbRestSerwerFilesPath}\\{id}.pdf");
            return File.ReadAllBytes($"{_settings.PcbRestSerwerFilesPath}\\{id}.pdf");
        }

        private async Task EndSession(string ConnectionId)
        {
            var response =
                await _settings.PcbRestSerwerUrl
                .AppendPathSegment("\"EndSession\"")
                .WithHeader("ContentType", "application/json")
                .WithHeader("Pragma", ConnectionId)
                .WithBasicAuth(_settings.PcbRestSerwerUsername, _settings.PcbRestSerwerPassword)
                .PostAsync();

            if (response != null)
            {
                if (response.StatusCode != 200)
                    throw new Exception($"EndSession error:  Code {response.StatusCode} - {response.ResponseMessage}");
            }
            else
                throw new Exception($"EndSession error response == null");

            _logger.Log(LogLevel.Info, $"EndSession {ConnectionId}");
        }
    }
}
