using System.Windows;
using iNKORE.UI.WPF.Modern;
using StarsAICopilot.CS;

namespace StarsAICopilot;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // 触发静态构造函数加载配置
        var _ = ConfigHelper.CurrentConfig;
        ThemeManager.Current.ApplicationTheme = ConfigHelper.CurrentConfig.Theme == "Dark"
            ? ApplicationTheme.Dark
            : ApplicationTheme.Light;
        base.OnStartup(e);
        LanguageHelper.ApplyLanguage(ConfigHelper.CurrentConfig.Language ?? "zh-CN");
    }
}