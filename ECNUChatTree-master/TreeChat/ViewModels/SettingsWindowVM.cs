using System.Collections.ObjectModel;
using System.Windows;
using TreeChat.Commands;
using TreeChat.Services;

namespace TreeChat.ViewModels
{
    /// <summary>
    /// 设置窗口的ViewModel，处理API配置的编辑和保存
    /// </summary>
    public class SettingsWindowVM : BaseViewModel
    {
        private string _apiKey = string.Empty;
        private string _apiEndpoint = string.Empty;
        private string _modelName = string.Empty;
        private double _temperature;
        private double _topP;
        private int _topK;
        private string _selectedProfileName = string.Empty;

        public string ApiKey { get => _apiKey; set => SetProperty(ref _apiKey, value); }
        public string ApiEndpoint { get => _apiEndpoint; set => SetProperty(ref _apiEndpoint, value); }
        public string ModelName { get => _modelName; set => SetProperty(ref _modelName, value); }
        public double Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }
        public double TopP { get => _topP; set => SetProperty(ref _topP, value); }
        public int TopK { get => _topK; set => SetProperty(ref _topK, value); }
        public ObservableCollection<string> ProfileNames { get; } = new();

        public string SelectedProfileName
        {
            get => _selectedProfileName;
            set { if (SetProperty(ref _selectedProfileName, value)) LoadProfile(value); }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand AddProfileCommand { get; }
        public RelayCommand DeleteProfileCommand { get; }

        public SettingsWindowVM()
        {
            foreach (var name in SettingsService.Instance.Settings.Profiles.Keys)
                ProfileNames.Add(name);

            var currentName = SettingsService.Instance.Settings.CurrentProfileName;
            SelectedProfileName = ProfileNames.Contains(currentName) ? currentName : ProfileNames[0];

            SaveCommand = new RelayCommand(Save);
            AddProfileCommand = new RelayCommand(AddProfile);
            DeleteProfileCommand = new RelayCommand(DeleteProfile, _ => ProfileNames.Count > 1);
        }

        /// <summary>
        /// 保存当前配置并关闭窗口
        /// </summary>
        private void Save(object? parameter)
        {
            if (SettingsService.Instance.Settings.Profiles.TryGetValue(SelectedProfileName, out var profile))
            {
                profile.ApiKey = ApiKey;
                profile.ApiEndpoint = ApiEndpoint;
                profile.ModelName = ModelName;
                profile.Temperature = Temperature;
                profile.TopP = TopP;
                profile.TopK = TopK;
            }
            SettingsService.Instance.Settings.CurrentProfileName = SelectedProfileName;
            SettingsService.Instance.Save();
            OpenAIChat.Instance.UpdateAuthentication();
            if (parameter is Window window) window.Close();
        }

        private void AddProfile(object? parameter)
        {
            string newName = "新配置";
            int count = 1;
            while (SettingsService.Instance.Settings.Profiles.ContainsKey(newName))
                newName = $"新配置{++count}";

            SettingsService.Instance.Settings.Profiles[newName] = Models.ApiProfile.CreateDefault();
            SettingsService.Instance.Save();
            ProfileNames.Add(newName);
            SelectedProfileName = newName;
        }

        private void DeleteProfile(object? parameter)
        {
            if (ProfileNames.Count <= 1) return;
            SettingsService.Instance.Settings.Profiles.Remove(SelectedProfileName);
            SettingsService.Instance.Save();
            ProfileNames.Remove(SelectedProfileName);
            SelectedProfileName = ProfileNames[0];
        }

        private void LoadProfile(string name)
        {
            if (SettingsService.Instance.Settings.Profiles.TryGetValue(name, out var p))
            {
                ApiKey = p.ApiKey; ApiEndpoint = p.ApiEndpoint; ModelName = p.ModelName;
                Temperature = p.Temperature; TopP = p.TopP; TopK = p.TopK;
            }
        }
    }
}
