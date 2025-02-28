using Page = System.Windows.Controls.Page;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using System.Windows;
using StarsAICopilot.Windows.Tools;

namespace StarsAICopilot.Pages;

public partial class ToolsPage : Page
{
    public ToolsPage()
    {
        InitializeComponent();
    }
    
    private void ToolsTranslate(object sender, RoutedEventArgs e)
    {
        TranslateTools translate = TranslateTools.Instance;
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