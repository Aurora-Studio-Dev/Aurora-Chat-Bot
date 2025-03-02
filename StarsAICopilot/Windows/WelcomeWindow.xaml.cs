using System.Windows;
using iNKORE.UI.WPF.Modern.Controls;
using StarsAICopilot.CS;
using StarsAICopilot.Windows.Tools;

namespace StarsAICopilot.Windows;

public partial class WelcomeWindow
{
    private static WelcomeWindow _instance;
    private bool _isWelcomeClosing;
    
    private readonly string[] _pageUris = new[]
    {
        "Pages/Welcome/Language.xaml",
        "Pages/Welcome/Main.xaml",
        "Pages/Welcome/Personalize.xaml",
        "Pages/Welcome/APIConfig.xaml",
        "Pages/Welcome/StartUse.xaml"
    };

    private int _currentPageIndex;

    public WelcomeWindow()
    {
        InitializeComponent();
        NavigateToPage(_currentPageIndex);
        this.Closing += WelcomeWindow_Closing; // 新增关闭事件
    }

    public void WelcomeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_isWelcomeClosing) return;
        _isWelcomeClosing = true;

        Dispatcher.BeginInvoke(new Action(() => {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null && !mainWindow.IsMainClosing)
            {
                mainWindow.DetachCloseHandler();
                mainWindow.Close();
            }
        }));
    }
    public static WelcomeWindow Instance
    {
        get
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new WelcomeWindow();
                _instance.Closed += (s, e) => _instance = null;
            }

            return _instance;
        }
    }
    private void ChangeNav()
    {
        if (_currentPageIndex == 0)
            Nav.SelectedItem = Nav.MenuItems[0];
        else if (_currentPageIndex == 1)
            Nav.SelectedItem = Nav.MenuItems[1];
        else if (_currentPageIndex == 2)
            Nav.SelectedItem = Nav.MenuItems[2];
        else if (_currentPageIndex == 3)
            Nav.SelectedItem = Nav.MenuItems[3];
        else if (_currentPageIndex == 4) Nav.SelectedItem = Nav.MenuItems[4];
    }

    private void NavigateToPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < _pageUris.Length)
        {
            _currentPageIndex = pageIndex;
            FrameRoot.Navigate(new Uri(_pageUris[pageIndex], UriKind.Relative));
        }
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        NavigateToPage(_currentPageIndex - 1);
        ChangeNav();
        NextButton.IsEnabled = true;
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPageIndex == 4)
        {
            ConfigHelper.CurrentConfig.IsWelcome = "ok";
            ConfigHelper.SaveConfig();
            
            Dispatcher.BeginInvoke(new Action(() => {
                Close();
                var newMain = new MainWindow();
                Application.Current.MainWindow = newMain;
                newMain.Show();
            }));
        }
        else
        {
            NavigateToPage(_currentPageIndex + 1);
        }
        ChangeNav();
    }

    private void NavigationViewSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem selectedItem
            && sender is NavigationView navView)
        {
            var index = navView.MenuItems.IndexOf(selectedItem);
            NavigateToPage(index);
        }
    }
}