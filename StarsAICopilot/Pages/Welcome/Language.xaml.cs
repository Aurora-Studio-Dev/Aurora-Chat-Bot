using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using StarsAICopilot.CS;
using StarsAICopilot.Windows;

namespace StarsAICopilot.Pages.Welcome;

public partial class Language : INotifyPropertyChanged
{
    public Language()
    {
        InitializeComponent();
        Loaded += (s, e) => LoadLanguageSettings();
        LoadLanguageSettings();
    }

    public string CurrentLanguage
    {
        get => ConfigHelper.CurrentConfig.Language ?? "zh-CN";
        set
        {
            if (ConfigHelper.CurrentConfig.Language != value)
            {
                ConfigHelper.CurrentConfig.Language = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadLanguageSettings()
    {
        LanguageComboBox.SelectedValue = CurrentLanguage;
        LanguageHelper.ApplyLanguage(CurrentLanguage);
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedValue is string selectedLang && !string.IsNullOrEmpty(selectedLang))
        {
            CurrentLanguage = selectedLang;
            ConfigHelper.SaveConfig();
            LanguageHelper.SaveAndApplyLanguage(selectedLang);

            var frame = Parent as Frame;
            frame?.Navigate(new Language());
            frame?.Navigate(new WelcomeWindow());
        }
    }
}