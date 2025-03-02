using System.Windows;
using StarsAICopilot.Windows.Tools;
using Page = System.Windows.Controls.Page;

namespace StarsAICopilot.Pages;

public partial class ToolsPage : Page
{
    public ToolsPage()
    {
        InitializeComponent();
    }

    private void ToolsTranslate(object sender, RoutedEventArgs e)
    {
        var translate = TranslateTools.Instance;
        if (!translate.IsVisible)
        {
            translate.Show();
        }
        else
        {
            translate.Activate();
            translate.Topmost = true;
            translate.Topmost = false;
        }
    }
}