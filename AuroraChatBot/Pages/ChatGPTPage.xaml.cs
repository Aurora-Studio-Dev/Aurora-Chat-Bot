using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;


namespace AuroraChatBot.Pages;

public partial class ChatGPTPage
{
    public class ChatMessage
    {
        public string Content { get; set; } // 消息内容
        public string Sender { get; set; }  // 发送者（用户或 AI）
    }
    private static readonly HttpClient httpClient = new HttpClient();
    //public static string OpenAIApiKey { get; set; }
    ///public static string OpenAIApiUrl { get; set; }
    private string OpenAIApiKey = "sk-fR7Ho6pRPuXrDpkNK1VKJWzrctdGz1Z453jUYREpQz7c4kO3"; // 替换为您的API密钥
    private string OpenAIApiUrl = "https://api.chatanywhere.tech/v1/chat/completions"; // GPT API的URL
    public ObservableCollection<ChatMessage> Messages { get; set; } // 消息集合
    
    public ChatGPTPage()
    {
        InitializeComponent();
        Messages = new ObservableCollection<ChatMessage>();
        ChatHistory.ItemsSource = Messages; // 绑定消息集合
    }
    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string userMessage = UserInput.Text.Trim();
        if (!string.IsNullOrEmpty(userMessage))
        {
            // 添加用户消息
            Messages.Add(new ChatMessage { Content = userMessage, Sender = "User" });
            UserInput.Clear();
            SendButton.IsEnabled = false; // 禁用按钮

            // 获取 AI 回复
            string gptResponse = await GetGptResponse(userMessage);

            // 添加 AI 消息
            Messages.Add(new ChatMessage { Content = gptResponse, Sender = "AI" });
            SendButton.IsEnabled = true; // 重新启用按钮
        }
    }

    private async Task<string> GetGptResponse(string input)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIApiKey);

        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = input }
            }
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(OpenAIApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
                
                // 验证响应结构
                if (responseObject.choices != null && responseObject.choices.Count > 0)
                {
                    return responseObject.choices[0].message.content.ToString();
                }
                else
                {
                    return "错误: 无法获取有效响应";
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return $"错误: {response.StatusCode} - {errorContent}";
            }
        }
        catch (Exception ex)
        {
            return $"异常: {ex.Message}"; // 返回异常信息
        }
    }
}
