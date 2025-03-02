using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern;
using StarsAICopilot.CS;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace StarsAICopilot.Pages;

public partial class SettingsPage : INotifyPropertyChanged
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = this;
        LoadSettings();
        Loaded += (s, e) => LoadLanguageSettings();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    #region 事件处理

    private void SaveApi_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ApiKey.Password) || string.IsNullOrWhiteSpace(Mod.Text) ||
            string.IsNullOrWhiteSpace(ApiUrl.Text))
        {
            MessageBox.Show(
                (string)Application.Current.FindResource("Settings.MsgBox.EmptyKeyOrMod"),
                $"Stars AI Copilot - {(string)Application.Current.FindResource("Settings.Title")}",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var result = MessageBox.Show(
            (string)Application.Current.FindResource("Settings.MsgBox.IsOK"),
            $"Stars AI Copilot - {(string)Application.Current.FindResource("Settings.Title")}",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes) SaveApiConfiguration();
    }

    #endregion

    #region INotifyPropertyChanged 实现

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region 语言设置

    public string CurrentLanguage
    {
        get => ConfigHelper.CurrentConfig.Language ?? "zh-CN";
        set
        {
            if (ConfigHelper.CurrentConfig.Language != value)
            {
                ConfigHelper.CurrentConfig.Language = value;
                OnPropertyChanged();
            }
        }
    }

    private void LoadLanguageSettings()
    {
        // 初始化语言选择
        LanguageComboBox.SelectedValue = CurrentLanguage;
        LanguageHelper.ApplyLanguage(CurrentLanguage);
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedValue is string selectedLang && !string.IsNullOrEmpty(selectedLang))
        {
            CurrentLanguage = selectedLang;
            ConfigHelper.SaveConfig();
            LanguageHelper.SaveAndApplyLanguage(selectedLang);

            // 刷新当前页面应用语言
            var frame = Parent as Frame;
            frame?.Navigate(new SettingsPage());
        }
    }

    #endregion

    #region 配置管理

    private void LoadSettings()
    {
        ApiUrl.Text = ConfigHelper.CurrentConfig.ApiUrl;
        ApiKey.Password = ConfigHelper.CurrentConfig.ApiKey;
        Mod.Text = ConfigHelper.CurrentConfig.Mod;
    }

    private void SaveApiConfiguration()
    {
        ConfigHelper.CurrentConfig.ApiUrl = ApiUrl.Text;
        ConfigHelper.CurrentConfig.ApiKey = ApiKey.Password;
        ConfigHelper.CurrentConfig.Mod = Mod.Text;
        ConfigHelper.SaveConfig();
    }

    #endregion

    #region 主题设置

    private void ThemeDark_OnChecked(object sender, RoutedEventArgs e)
    {
        try
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            ThemeLight.IsChecked = false;
            ConfigHelper.CurrentConfig.Theme = "Dark";
            ConfigHelper.SaveConfig();
        }
        catch
        {
        }
    }

    private void ThemeLight_OnChecked(object sender, RoutedEventArgs e)
    {
        try
        {
            // 检查主题管理器是否初始化
            if (ThemeManager.Current != null &&
                ThemeManager.Current.ApplicationTheme != ApplicationTheme.Light)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;

                // 确保配置对象已初始化
                if (ConfigHelper.CurrentConfig != null)
                {
                    ConfigHelper.CurrentConfig.Theme = ApplicationTheme.Light.ToString();
                    ConfigHelper.SaveConfig();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion
}