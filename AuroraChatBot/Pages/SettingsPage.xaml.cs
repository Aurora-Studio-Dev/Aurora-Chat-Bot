using System.IO;
using System.Text.Json;
using Page = System.Windows.Controls.Page;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;


namespace AuroraChatBot.Pages;

public partial class SettingsPage : Page
{
    public static string DEEPSEEK_API_KEY { get; set; }
    public static string DEEPSEEK_API_URL { get; set; }
    private readonly string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "acb", "api_config.json");

    public SettingsPage()
    {
        InitializeComponent();
        LoadSettings();
        SettingsTheme();
    }

    void SettingsTheme()
    {
        switch (SetTheme_ComboBox.SelectedIndex)
        {
            case 0:
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                break;
            case 1:
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                break;
            default:
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                break;
        }
    }
    void LoadSettings()
    {
        if (File.Exists(configFilePath))
        {
            var json = File.ReadAllText(configFilePath);
            var config = JsonSerializer.Deserialize<ApiConfig>(json);
            DEEPSEEK_API_KEY = config?.DsApiKey;
            DEEPSEEK_API_URL = config?.DsApiUrl;

            DSAPIURL.Text = DEEPSEEK_API_URL;
            DSAPIKEY.Password = DEEPSEEK_API_URL;
        }
        else
        {
        }
    }

    void SaveSettings()
    {
        var config = new ApiConfig
        {
            DsApiKey = DEEPSEEK_API_KEY,
            DsApiUrl = DEEPSEEK_API_URL
        };

        var json = JsonSerializer.Serialize(config);
        Directory.CreateDirectory(Path.GetDirectoryName(configFilePath)); // 确保目录存在
        File.WriteAllText(configFilePath, json);
    }

    // 用于序列化和反序列化的类
    private class ApiConfig
    {
        public string DsApiKey { get; set; }
        public string DsApiUrl { get; set; }
    }
}