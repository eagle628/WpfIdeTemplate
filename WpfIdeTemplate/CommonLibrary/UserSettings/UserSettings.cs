using System.Text.Json.Serialization;

namespace SampleCompany.SampleProduct.CommonLibrary.UserSettings
{
    public class UserSettings
    {
        public General General { get; set; }
    }
    public class General
    {
        public string CultureName { get; set; }
    }
    [JsonSerializable(typeof(UserSettings))]
    public partial class UserSettingsContext : JsonSerializerContext { }
}
