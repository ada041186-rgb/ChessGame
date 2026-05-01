using ChessApplication.DTO;
using ChessApplication.Interfaces.Utils;
using ChessGame.Commands;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChessGame.ViewModel.UserControlViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;
        public ObservableCollection<string> AvailableResolutions { get; } = new()
        {
            "800x600",
            "1024x768",
            "1280x720",
            "1600x900",
            "1920x1080"
        };

        private string _selectedResolution;
        public string SelectedResolution
        {
            get => _selectedResolution;
            set
            {
                _selectedResolution = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isFullScreen;
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                _isFullScreen = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ApplyCommand { get; }
        public ICommand BackCommand { get; }

        public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;

            var data = _settingsService.Load();
            _isFullScreen = data.IsFullScreen;
            _selectedResolution = $"{data.Width}x{data.Height}";

            ApplyCommand = new RelayCommand(ApplySettings);
            BackCommand = new RelayCommand(BackToMenu);
        }

        private void BackToMenu(object param)
        {
            _navigationService.NavigateTo<MenuViewModel>();
        }

        private void ApplySettings(object obj)
        {
            var parts = SelectedResolution.Split('x');
            if (parts.Length != 2) return;

            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);

            _settingsService.Save(new SettingsData
            {
                Width = width,
                Height = height,
                IsFullScreen = IsFullScreen
            });

            UpdateApplicationWindow(width, height, IsFullScreen);
        }

        private void UpdateApplicationWindow(int width, int height, bool fullScreen)
        {
            var window = Application.Current.MainWindow;
            if (window == null) return;

            if (fullScreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                window.Width = width;
                window.Height = height;

                window.Left = (SystemParameters.PrimaryScreenWidth - width) / 2;
                window.Top = (SystemParameters.PrimaryScreenHeight - height) / 2;
            }
        }
    }
}