using Model.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEditSettingsService
    {
        void SaveAppProfile(AppProfile appProfile);
    }
    public class EditSettingsService : IEditSettingsService
    {
        public EditSettingsService()
        {

        }
        public void SaveAppProfile(AppProfile appProfile)
        {
            var appProfileInstance = JsonConvert.SerializeObject(appProfile);
            File.WriteAllText($"{Environment.CurrentDirectory}/appprofile.json", appProfileInstance);
        }
    }
}
