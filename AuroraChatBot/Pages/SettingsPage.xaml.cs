using System.IO;
using System.Text.Json;
using Page = System.Windows.Controls.Page;

namespace AuroraChatBot.Pages;

public partial class SettingsPage : Page
{
    public static string OPEN_API_KEY { get; set; }
    public static string OPEN_API_URL { get; set; }
    private readonly string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "acb", "api_config.json");

    public SettingsPage()
    {
        InitializeComponent();
        LoadSettings(); // 每次启动时加载设置
    }

    void DefaultSettings()
    {
        SetTheme_ComboBox.SelectedIndex = 0;
    }

    void LoadSettings()
    {
        if (File.Exists(configFilePath))
        {
            var json = File.ReadAllText(configFilePath);
            var config = JsonSerializer.Deserialize<ApiConfig>(json);
            OPEN_API_KEY = config?.ApiKey;
            OPEN_API_URL = config?.ApiUrl;

            OPENAPIURL.Text = OPEN_API_URL;
            OPENAPIKEY.Password = OPEN_API_KEY;
        }
        else
        {
            // 如果文件不存在，可以处理默认设置
            DefaultSettings();
        }
    }

    void SaveSettings()
    {
        var config = new ApiConfig
        {
            ApiKey = OPEN_API_KEY,
            ApiUrl = OPEN_API_URL
        };

        var json = JsonSerializer.Serialize(config);
        Directory.CreateDirectory(Path.GetDirectoryName(configFilePath)); // 确保目录存在
        File.WriteAllText(configFilePath, json);
    }

    // 用于序列化和反序列化的类
    private class ApiConfig
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}