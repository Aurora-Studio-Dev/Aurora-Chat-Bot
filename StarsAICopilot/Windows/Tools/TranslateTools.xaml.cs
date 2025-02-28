using System.Windows;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using StarsAICopilot.CS;

namespace StarsAICopilot.Windows.Tools;

public partial class TranslateTools : Window
{
    private static TranslateTools _instance;
    //private const string OpenAIKey = "sk-";
    private string ApiKey => ConfigHelper.CurrentConfig.ApiKey;
    private readonly HttpClient _httpClient = new();

    public TranslateTools()
    {
        InitializeComponent();
        InitializeLanguages();
    }

    private void InitializeLanguages()
    {
        var presetLanguages = new[]
        {
            "自动检测", "简体中文", "繁體中文", 
            "英语（English）", "日语（日本語）", "韩语（한국어）",
            "法语（Français）", "西班牙语（Español）", "德语（Deutsch）"
        };
        
        SourceLangCombo.IsEditable = true;
        SourceLangCombo.ItemsSource = presetLanguages;
        SourceLangCombo.Text = "自动检测";
        SourceLangCombo.ToolTip = "选择或输入源语言";
    
        TargetLangCombo.IsEditable = true;
        TargetLangCombo.ItemsSource = presetLanguages.Concat(new[]{"العربية", "Русский"}); // 可扩展更多预置
        TargetLangCombo.Text = "简体中文";
        TargetLangCombo.ToolTip = "选择或输入目标语言";
        
        SourceLangCombo.SelectedIndex = 0;
        TargetLangCombo.SelectedIndex = 1;
    }
    private string BuildTranslationPrompt()
    {
        // 获取用户输入/选择的语言
        var srcLang = (SourceLangCombo.Text ?? "自动检测").Trim();
        var tgtLang = (TargetLangCombo.Text ?? "简体中文").Trim();

        // 空值处理
        if (string.IsNullOrWhiteSpace(srcLang)) srcLang = "自动检测";
        if (string.IsNullOrWhiteSpace(tgtLang)) tgtLang = "简体中文";

        return $"将{(srcLang == "自动检测" ? "自动检测的源语言" : $"【{srcLang}】")}翻译为【{tgtLang}】，保持专业准确，不要添加额外内容";
    }
    private async void TranslateButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InputText.Text))
        {
            OutputText.Text = "请输入要翻译的内容";
            return;
        }

        try
        {
            OutputText.Text = "翻译中...";
            var result = await TranslateWithGPT(InputText.Text);
            OutputText.Text = result;
        }
        catch (Exception ex)
        {
            OutputText.Text = $"翻译失败：{ex.Message}";
        }
    }

    private async Task<string> TranslateWithGPT(string text)
    {
        var prompt = BuildTranslationPrompt();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(ConfigHelper.CurrentConfig.ApiUrl),
            Headers =
            {
                { "Authorization", $"Bearer {ApiKey}" }
            },
            Content = new StringContent($@"{{
                ""model"": ""{ConfigHelper.CurrentConfig.Mod}"",
                ""messages"": [
                    {{""role"": ""system"", ""content"": ""你是一名专业翻译""}},
                    {{""role"": ""user"", ""content"": ""{prompt}: {EscapeJsonString(text)}""}}
                ]
            }}")
        };
        request.Content.Headers.ContentType = 
            new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var json = JObject.Parse(await response.Content.ReadAsStringAsync());
        return json["choices"]?[0]?["message"]?["content"]?.ToString() ?? "无返回结果";
    }

    private static string EscapeJsonString(string input) => 
        input.Replace("\\", "\\\\")
             .Replace("\"", "\\\"")
             .Replace("\n", "\\n")
             .Replace("\r", "\\r");

    // 保持原有单例逻辑
    public static TranslateTools Instance
    {
        get
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new TranslateTools();
                _instance.Closed += (s, e) => _instance = null;
            }
            return _instance;
        }
    }
}
