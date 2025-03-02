using System.IO;
using System.Reflection;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Media.Animation;
using StarsAICopilot.CS;
using StarsAICopilot.Pages;
using StarsAICopilot.Windows;
using Page = System.Windows.Controls.Page;

namespace StarsAICopilot;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private const string DefaultTheme = "Light";
    private bool _isMainClosing;
    public bool IsMainClosing => _isMainClosing;
    private static WelcomeWindow _instance;
    
    private readonly Page _about = new AboutPage();
    private readonly Page _chat = new ChatPage();

    private readonly Page _home = new HomePage();
    private readonly Page _settings = new SettingsPage();
    private readonly Page _test = new TestPage();
    private readonly Page _tools = new ToolsPage();

    public MainWindow()
    {
        InitializeComponent();
        CurrentPage.Navigate(_home);
        cfile();
        SettingTheme();
        Welcome();
        DetachCloseHandler();
    }
    
    public void DetachCloseHandler()
    {
        this.Closing -= All_Close;
    }
    public void All_Close(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_isMainClosing) return;
        _isMainClosing = true;

        Dispatcher.BeginInvoke(new Action(() => {
            if (WelcomeWindow.Instance != null && WelcomeWindow.Instance.IsVisible)
            {
                WelcomeWindow.Instance.Closing -= WelcomeWindow.Instance.WelcomeWindow_Closing;
                WelcomeWindow.Instance.Close();
            }
        }));

        e.Cancel = false;
    }

    private void Welcome()
    {
        if (ConfigHelper.CurrentConfig.IsWelcome == string.Empty)
        {
            Hide();
            var welcome = WelcomeWindow.Instance;
            if (!welcome.IsVisible)
            {
                welcome.Show();
            }
            else
            {
                welcome.Activate();
                welcome.Topmost = true;
                welcome.Topmost = false;
            }
        }
    }

    private void SettingTheme()
    {
        var theme = ConfigHelper.CurrentConfig.Theme ?? DefaultTheme;
        ThemeManager.Current.ApplicationTheme = theme switch
        {
            "Dark" => ApplicationTheme.Dark,
            _ => ApplicationTheme.Light
        };
    }

    private void cfile()
    {
        try
        {
            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var FolderName = "sac";
            var FolderPath = Path.Combine(applicationDirectory, FolderName);
            if (!Directory.Exists(FolderPath))
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch
                {
                }

            //生成Config.json文件
            var FileName = "config.json";
            var FilePath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(FilePath))
                try
                {
                    using (File.Create(FilePath))
                    {
                        ;
                    }
                }
                catch (Exception ex)
                {
                }
        }
        catch
        {
        }
    }

    /// <summary>
    ///     Navigation trigger handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NavigationTriggered(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            NavigateTo(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer?.Tag != null)
        {
            var pageType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
            if (pageType != null) NavigateTo(pageType, args.RecommendedNavigationTransitionInfo);
        }
    }

    /// <summary>
    ///     Navigation to a specific page.
    /// </summary>
    /// <param name="navPageType">Type of the page.</param>
    /// <param name="transitionInfo">Transition animation.</param>
    private void NavigateTo(Type navPageType, NavigationTransitionInfo transitionInfo)
    {
        // 空值保护
        if (CurrentPage?.Content == null || navPageType == null) return;

        var preNavPageType = CurrentPage.Content.GetType();
        if (navPageType == preNavPageType) return;

        var pageMapping = new Dictionary<Type, Page>
        {
            { typeof(HomePage), _home },
            { typeof(ChatPage), _chat },
            { typeof(ToolsPage), _tools },
            { typeof(SettingsPage), _settings },
            { typeof(AboutPage), _about },
            { typeof(TestPage), _test }
        };

        if (pageMapping.TryGetValue(navPageType, out var targetPage)) CurrentPage.Navigate(targetPage, transitionInfo);
    }
}