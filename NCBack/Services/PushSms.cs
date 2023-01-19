using System.Net.Http.Headers;
using NCBack.NotificationModels;

namespace NCBack.Services;

public class PushSms
{
    private static Random rnd = new Random();
    public int code = rnd.Next(100000, 999999);
    
    private readonly IConfiguration _configuration;

    public PushSms(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Sms(string phone)
    {
        using (var client = new HttpClient())
        {
            var result = _configuration.GetValue<string>("MobizonNotification:ServerKey");
            
            client.BaseAddress = new Uri("https://api.mobizon.kz/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // New code:
            HttpResponseMessage response = await client
                .GetAsync(
                    $"service/message/sendsmsmessage?recipient={phone}&text=Добро пожаловать это ваш код {code}&apiKey={result}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                Console.WriteLine(product);
            }
        }
    }
}