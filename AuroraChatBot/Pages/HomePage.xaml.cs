using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox ;

namespace AuroraChatBot.Pages
{
    public partial class HomePage
    {
        // 替换为你的高德 Web 服务 API 密钥
        private const string ApiKey = "4359f5724ebb1f5bd672a188d02e1f6c";
        private string CityCode;

        public HomePage()
        {
            InitializeComponent();
            FetchDailyQuoteAsync(); // 获取每日金句
            GetCityNameAsync(); // 自动获取城市名
        }
        
        private async void GetCityNameAsync()
        {
            
        }

        private async void FetchDailyQuoteAsync()
        {
            string quoteText = "无法获取每日金句。";
            string authorText = "";
            string originText = "";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://api.xygeng.cn/one"; // 获取每日金句的 API
                    var response = await client.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    if ((int)json["code"] == 200)
                    {
                        var data = json["data"];
                        quoteText = data["content"].ToString();
                        authorText = data["tag"].ToString();
                        originText = data["origin"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取每日金句时出错: {ex.Message}");
            }

            QuoteTextBlock.Text = quoteText;
            AuthorTextBlock.Text = authorText;
            OriginTextBlock.Text = $"来自: {originText}";
        }

        private async Task FetchWeatherAsync() // 修改为Task以适应async调用
        {
            string weatherInfo = "无法获取天气信息。";
            string temperatureInfo = "";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://restapi.amap.com/v3/weather/weatherInfo?key={ApiKey}&city={CityCode}&extensions=all";
                    var response = await client.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    if (json["status"].ToString() == "1") // 状态码为1表示成功
                    {
                        var liveData = json["lives"][0]; // 获取第一个城市的天气数据
                        weatherInfo = liveData["weather"].ToString(); // 天气状况
                        temperatureInfo = liveData["temperature"].ToString() + " °C"; // 当前温度
                    }
                    else
                    {
                        MessageBox.Show($"获取天气信息失败: {json["info"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取天气信息时出错: {ex.Message}");
            }

            WeatherTextBlock.Text = $"天气情况: {weatherInfo}";
            TemperatureTextBlock.Text = $"当前温度: {temperatureInfo}";
        }
    }
}
