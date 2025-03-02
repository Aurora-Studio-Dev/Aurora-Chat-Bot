using iNKORE.UI.WPF.Modern;
using StarsAICopilot.CS;
using System.Windows.Controls;

namespace StarsAICopilot.Pages.Welcome;

public partial class Personalize : Page
{
    public Personalize()
    {
        InitializeComponent();
    }

    private void LightClick(object sender, System.Windows.RoutedEventArgs e)
    {
        ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        ConfigHelper.CurrentConfig.Theme = "Light";
        ConfigHelper.SaveConfig();
    }

    private void DarkClick(object sender, System.Windows.RoutedEventArgs e)
    {
        ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        ConfigHelper.CurrentConfig.Theme = "Dark";
        ConfigHelper.SaveConfig();
    }
}