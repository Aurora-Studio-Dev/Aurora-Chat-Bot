using StarsAICopilot.CS;
using System.Windows;
using System.Windows.Controls;

namespace StarsAICopilot.Pages.Welcome;

public partial class APIConfig : Page
{
    public APIConfig()
    {
        InitializeComponent();
        LoadSettings();
    }

    void LoadSettings()
    {
        ApiUrl.Text = ConfigHelper.CurrentConfig.ApiUrl;
        ApiKey.Password = ConfigHelper.CurrentConfig.ApiKey;
        Mod.Text = ConfigHelper.CurrentConfig.Mod;
    }
    void SaveApiConfiguration()
    {
        ConfigHelper.CurrentConfig.ApiUrl = ApiUrl.Text;
        ConfigHelper.CurrentConfig.ApiKey = ApiKey.Password;
        ConfigHelper.CurrentConfig.Mod = Mod.Text;
        ConfigHelper.SaveConfig();
    }

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
}