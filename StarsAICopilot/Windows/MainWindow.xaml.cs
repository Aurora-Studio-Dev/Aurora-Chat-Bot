using iNKORE.UI.WPF.Modern.Media.Animation;
using System.IO;
using System.Reflection;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using StarsAICopilot.Pages;  
using Page = System.Windows.Controls.Page;
using StarsAICopilot.CS;

namespace StarsAICopilot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private Page _home = new HomePage();
    private Page _chat = new ChatPage();
    private Page _tools = new ToolsPage();
    private Page _settings = new SettingsPage();
    private Page _about = new AboutPage();
    private Page _test = new TestPage();
    
    private const string DefaultTheme = "Light";
    
    public MainWindow()
    {
        InitializeComponent();
        CurrentPage.Navigate(_home);
        cfile();
        AppVersionShow.Text = "Release Version 1.0.1";
        SettingTheme();
    }

    void SettingTheme()
    {
        var theme = ConfigHelper.CurrentConfig.Theme ?? DefaultTheme;
        ThemeManager.Current.ApplicationTheme = theme switch
        {
            "Dark" => ApplicationTheme.Dark,
            _      => ApplicationTheme.Light 
        };
    }

    void cfile()
    {
        try
        {
            string applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string FolderName = "sac";
            string FolderPath = Path.Combine(applicationDirectory, FolderName);
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch
                {
                }
            }

            //生成Config.json文件
            string FileName = "config.json";
            string FilePath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(FilePath))
            {
                try
                {
                    using (File.Create(FilePath)) ;
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch
        {

        }
    }

    /// <summary>
    ///    Navigation trigger handler.
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
                if (pageType != null)
                {
                    NavigateTo(pageType, args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        /// <summary>
        ///    Navigation to a specific page.
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

            if (pageMapping.TryGetValue(navPageType, out var targetPage))
            {
                CurrentPage.Navigate(targetPage, transitionInfo);
            }
        }
}