using System.Windows;
using iNKORE.UI.WPF.Modern;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using StarsAICopilot.CS;

namespace StarsAICopilot.Pages;

public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }
    private void LoadSettings()
    {
        ApiUrl.Text = ConfigHelper.CurrentConfig.ApiUrl;
        ApiKey.Password = ConfigHelper.CurrentConfig.ApiKey;
        Mod.Text = ConfigHelper.CurrentConfig.Mod;
    }

    private void SaveApi()
    {
        ConfigHelper.CurrentConfig.ApiUrl = ApiUrl.Text;
        ConfigHelper.CurrentConfig.ApiKey = ApiKey.Password;
        ConfigHelper.CurrentConfig.Mod = Mod.Text;
        ConfigHelper.SaveConfig();
    }
    

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
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            ThemeDark.IsChecked = false;
            ConfigHelper.CurrentConfig.Theme = "Light";
            ConfigHelper.SaveConfig();
        }
        catch
        {
            
        }
    }

    private void SaveApi(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ApiKey.Password) || string.IsNullOrWhiteSpace(Mod.Text))
        {
            MessageBox.Show("API 秘钥和模型不能为空！", // 提示语同步修改
                "Stars AI Copilot - 设置", 
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        // 补充 URL 格式验证
        if (string.IsNullOrWhiteSpace(ApiUrl.Text) || !Uri.IsWellFormedUriString(ApiUrl.Text, UriKind.Absolute))
        {
            ApiUrl.Text = "https://api.openai.com/v1/chat/completions";
        }

        var result = MessageBox.Show("是否保存 API 设置？",
            "Stars AI Copilot - 设置", 
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            SaveApi();
        }
    }
}