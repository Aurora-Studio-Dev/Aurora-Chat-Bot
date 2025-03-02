using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using StarsAICopilot.CS;

namespace StarsAICopilot.Pages;

public partial class ChatPage
{
    private const string UserRole = "User";
    private const string AssistantRole = "Assistant";
    private static readonly HttpClient HttpClient = new();

    private readonly string _apikey = ConfigHelper.CurrentConfig.ApiKey;
    private readonly string _apiurl = ConfigHelper.CurrentConfig.ApiUrl;

    public ChatPage()
    {
        InitializeComponent();
        DataContext = this; // 启用数据绑定上下文
    }

    public ObservableCollection<ChatMessage> Messages { get; } = new();

    // 优化后的命令实现
    public ICommand CopyMessageCommand => new RelayCommand<ChatMessage>(msg =>
    {
        try
        {
            // 移除InputBindings相关代码（不需要在这里添加）
            if (msg?.Content != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clipboard.SetText(msg.Content);
                    Console.Beep(); // 可选提示音
                });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"复制失败: {ex.Message}",
                "错误",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    });

    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        await ProcessUserInputAsync();
    }

    private async Task ProcessUserInputAsync()
    {
        var userMessage = UserInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(userMessage)) return;

        // 添加用户消息
        Messages.Add(new ChatMessage
        {
            Content = userMessage,
            Sender = UserRole
        });
        UserInput.Clear();
        SetUiState(false);

        try
        {
            var response = await GetGptResponseAsync();
            Messages.Add(new ChatMessage
            {
                Content = response,
                Sender = AssistantRole
            });
            ScrollToBottom();
        }
        finally
        {
            SetUiState(true);
        }
    }

    private async Task<string> GetGptResponseAsync()
    {
        try
        {
            using var request = CreateApiRequest();
            using var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"⚠️ 请求失败({response.StatusCode}): {error}";
            }

            var json = await response.Content.ReadAsStringAsync();
            return ParseApiResponse(json) ?? "⚠️ 无法解析响应内容";
        }
        catch (Exception ex)
        {
            return $"⚠️ 网络请求异常: {ex.Message}";
        }
    }

    private HttpRequestMessage CreateApiRequest()
    {
        var messages = Messages.Select(msg => new
        {
            role = msg.Sender == UserRole ? "user" : "assistant",
            content = msg.Content
        });

        var requestBody = new
        {
            model = ConfigHelper.CurrentConfig.Mod,
            messages,
            temperature = 0.7
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _apiurl)
        {
            Content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apikey);
        return request;
    }

    private static string ParseApiResponse(string json)
    {
        try
        {
            dynamic obj = JsonConvert.DeserializeObject(json);
            return obj?.choices?[0]?.message?.content?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private void ScrollToBottom()
    {
        if (ChatHistory.Items.Count > 0) ChatHistory.ScrollIntoView(ChatHistory.Items[^1]);
    }

    private void SetUiState(bool isEnabled)
    {
        SendButton.IsEnabled = isEnabled;
        UserInput.IsEnabled = isEnabled;
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var currentWidth = e.NewSize.Width;

        // 小屏幕适配
        if (currentWidth < 800)
        {
            ChatHistory.FontSize = 14;
            UserInput.Margin = new Thickness(0, 0, 5, 0);
            SendButton.Width = 70;
            SendButton.FontSize = 14;
        }
        else
        {
            ChatHistory.FontSize = 15;
            UserInput.Margin = new Thickness(0, 0, 10, 0);
            SendButton.Width = 80;
            SendButton.FontSize = 16;
        }

        ChatHistory.InvalidateMeasure();
        ChatHistory.UpdateLayout();
    }

    public class ChatMessage
    {
        public required string Content { get; init; }
        public required string Sender { get; init; }
    }

    private class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}