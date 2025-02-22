using iNKORE.UI.WPF.Modern.Media.Animation;
using iNKORE.UI.WPF.Modern;
using System.IO;
using System.Reflection;
using iNKORE.UI.WPF.Modern.Controls;
using AuroraChatBot.Pages;  
using Page = System.Windows.Controls.Page;

namespace AuroraChatBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private Page _home = new HomePage();
    private Page _chat = new ChatPage();
    private Page _tools = new ToolsPage();
    private Page _helps = new HelpsPage();
    private Page _settings = new SettingsPage();
    private Page _about = new AboutPage();
    private Page _test = new TestPage();

    public MainWindow()
    {
        InitializeComponent();
        cfile();
        AppVersionShow.Text = "Release Version 1.0.1";
        CurrentPage.Content = new HomePage();
    }

    void cfile()
    {
        try
        {
            //生成ASL文件夹
            string applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string FolderName = "acb";
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
                NavigateTo(typeof(int), args.RecommendedNavigationTransitionInfo);
            else if (args.InvokedItemContainer != null)
                NavigateTo(Type.GetType(args.InvokedItemContainer.Tag.ToString()),
                    args.RecommendedNavigationTransitionInfo);
        }

        /// <summary>
        ///    Navigation to a specific page.
        /// </summary>
        /// <param name="navPageType">Type of the page.</param>
        /// <param name="transitionInfo">Transition animation.</param>
        private void NavigateTo(Type navPageType, NavigationTransitionInfo transitionInfo)
        {
            var preNavPageType = CurrentPage.Content.GetType();
            if (navPageType == preNavPageType) return;
            switch (navPageType)
            {
                case not null when navPageType == typeof(HomePage):
                    CurrentPage.Navigate(_home);
                    break;
                case not null when navPageType == typeof(ChatPage):
                    CurrentPage.Navigate(_chat);
                    break;
                case not null when navPageType == typeof(ToolsPage):
                    CurrentPage.Navigate(_tools);
                    break;
                case not null when navPageType == typeof(HelpsPage):
                    CurrentPage.Navigate(_helps);
                    break;
                case not null when navPageType == typeof(SettingsPage):
                    CurrentPage.Navigate(_settings);
                    break;
                case not null when navPageType == typeof(AboutPage):
                    CurrentPage.Navigate(_about);
                    break;
                case not null when navPageType == typeof(TestPage):
                    CurrentPage.Navigate(_test);
                    break;
            }
        }
}