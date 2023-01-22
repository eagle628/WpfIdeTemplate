using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;

namespace SampleCompany.SampleProduct.CommonLibrary.UserSettings
{
    public class UserSettingsManager
    {
        public UserSettings UserSettings { get; }
        public UserSettingsManager(IConfiguration configration)
        {
            UserSettings = configration.GetRequiredSection("UserSettings").Get<UserSettings>()
                ?? throw new ArgumentNullException(nameof(configration));
        }
        public void UpdateSource()
        {
            var jsonWriteOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var newJson = JsonSerializer.Serialize(UserSettings, jsonWriteOptions);
            var userSettingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usersettings.json");
            File.WriteAllText(userSettingFilePath, newJson);
        }
    }
}
