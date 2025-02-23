using iNKORE.UI.WPF.Modern.Controls;
using Page = System.Windows.Controls.Page;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using System.Windows;

namespace StarsAICopilot.Pages;

public partial class ToolsPage : Page
{
    public ToolsPage()
    {
        InitializeComponent();
    }

    private void ToolsTranslate(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("你正在测试的是：SettingsCard Onclick");
    }
}