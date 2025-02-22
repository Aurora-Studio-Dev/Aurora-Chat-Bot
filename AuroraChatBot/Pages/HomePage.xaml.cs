using System.Net.Http;
using Newtonsoft.Json.Linq;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace StarsAICopilot.Pages
{
    public partial class HomePage
    {
        // 替换为你的高德 Web 服务 API 密钥
        private const string ApiKey = "4359f5724ebb1f5bd672a188d02e1f6c";

        public HomePage()
        {
            InitializeComponent();
            FetchDailyQuoteAsync();
        }

        private async void FetchDailyQuoteAsync()
        {
            string quoteText = "无法获取每日金句。";
            string authorText = "";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://api.aurorastudio.top/v1/open_stars_api/daily-sentence"; // 获取每日金句的 API
                    client.DefaultRequestHeaders.Add("type", "zenquotes"); // 将 type 参数添加到请求头

                    var response = await client.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    if (json["error"] == null)
                    {
                        quoteText = json["quote"].ToString();
                        authorText = json["author"].ToString();
                    }
                    else
                    {
                        Console.WriteLine("Error: " + json["error"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取每日金句时出错: {ex.Message}");
            }

            QuoteTextBlock.Text = quoteText;
            AuthorTextBlock.Text = authorText;
        }
    }
}
