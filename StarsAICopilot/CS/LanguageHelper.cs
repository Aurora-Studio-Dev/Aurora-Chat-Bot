using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace StarsAICopilot.CS;

public static class LanguageHelper
{
    private const string DefaultLanguage = "en";
    private const string ResourcePathFormat = "pack://application:,,,/StarsAICopilot;component/Language/{0}.xaml";

    public static void ApplyLanguage(string langCode)
    {
        // 参数防御性检查
        if (string.IsNullOrWhiteSpace(langCode))
        {
            Debug.WriteLine("Warning: langCode is empty, using default");
            langCode = DefaultLanguage;
        }

        try
        {
            // 设置文化信息
            var culture = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            // 清理旧的语言资源
            var app = Application.Current;
            var oldDict = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source?.OriginalString?.Contains("/Language/") ?? false);

            if (oldDict != null) app.Resources.MergedDictionaries.Remove(oldDict);

            // 加载新资源
            var uri = new Uri(string.Format(ResourcePathFormat, langCode));
            Debug.WriteLine($"Loading language resource: {uri}");

            var newDict = new ResourceDictionary { Source = uri };
            app.Resources.MergedDictionaries.Add(newDict);
        }
        catch (CultureNotFoundException ex)
        {
            Debug.WriteLine($"Culture not found: {ex.Message}");
            ApplyLanguage(DefaultLanguage); // 回退到默认语言
        }
        catch (IOException ex)
        {
            Debug.WriteLine($"Resource load failed: {ex.Message}");
            // 这里可以添加邮件通知或日志记录
        }
    }

    public static void SaveAndApplyLanguage(string langCode)
    {
        // 确保保存的语言有效
        if (string.IsNullOrWhiteSpace(langCode) || !IsValidCulture(langCode)) langCode = DefaultLanguage;

        ConfigHelper.CurrentConfig.Language = langCode;
        ConfigHelper.SaveConfig();
        ApplyLanguage(langCode);
    }

    private static bool IsValidCulture(string code)
    {
        try
        {
            var culture = new CultureInfo(code);
            return true;
        }
        catch
        {
            return false;
        }
    }
}