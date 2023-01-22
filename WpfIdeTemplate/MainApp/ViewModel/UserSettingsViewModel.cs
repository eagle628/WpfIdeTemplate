using Reactive.Bindings;
using SampleCompany.SampleProduct.CommonLibrary.UserSettings;
using System.Collections.Generic;

namespace SampleCompany.SampleProduct.MainApp.ViewModel
{
    public class UserSettingsViewModel
    {
        private readonly UserSettingsManager _userSettingsManager;
        public string CultureName
        {
            get => _userSettingsManager.UserSettings.General.CultureName;
            set => _userSettingsManager.UserSettings.General.CultureName = value; 
        }
        public List<string> CultureNameList { get; } = new List<string>() { "ja-JP", "en-US" };
        public ReactiveCommand UserSettingsUpdateCommand { get; }
        public UserSettingsViewModel(UserSettingsManager userSettingsManager)
        {
            _userSettingsManager = userSettingsManager;
            UserSettingsUpdateCommand = new ReactiveCommand()
                .WithSubscribe(() => _userSettingsManager.UpdateSource());
        }
    }
}
