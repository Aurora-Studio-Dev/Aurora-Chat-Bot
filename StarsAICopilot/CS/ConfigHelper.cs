using System.IO;
using System.Text.Json;
using iNKORE.UI.WPF.Modern.Controls;

namespace StarsAICopilot.CS;

public static class ConfigHelper
{
    private static readonly string ConfigPath = Path.Combine(AppContext.BaseDirectory, "sac", "config.json");
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    
    // 初始化默认配置，确保非null
    public static ApiConfig CurrentConfig { get; private set; } = new ApiConfig();

    static ConfigHelper()
    {
        LoadConfig();
    }

    private static void LoadConfig()
    {
        try
        {
            var configDir = Path.GetDirectoryName(ConfigPath);
        
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir!);
                SaveConfig();
                return;
            }

            if (!File.Exists(ConfigPath) || new FileInfo(ConfigPath).Length == 0)
            {
                SaveConfig();
                return;
            }

            var json = File.ReadAllText(ConfigPath);
        
            if (string.IsNullOrWhiteSpace(json))
            {
                SaveConfig();
                return;
            }

            var loadedConfig = JsonSerializer.Deserialize<ApiConfig>(json, Options);
            if (loadedConfig != null)
            {
                CurrentConfig = loadedConfig;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"配置加载异常：{ex.Message}\n已使用默认配置");
            SaveConfig(); // 用默认配置覆盖错误文件
        }
    }

    public static void SaveConfig()
    {
        var json = JsonSerializer.Serialize(CurrentConfig, Options);
        File.WriteAllText(ConfigPath, json);
    }

    public class ApiConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ApiUrl { get; set; } = string.Empty;
        public string Mod { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
    }
}
