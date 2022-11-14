using System.Net.Http.Headers;

namespace NCBack.Services;

public class PushSms
{
    private static Random rnd = new Random();
    public int code = rnd.Next(100000, 999999);

    public async Task Sms(string phone)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://api.mobizon.kz/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // New code:
            HttpResponseMessage response = await client
                .GetAsync(
                    $"service/message/sendsmsmessage?recipient={phone}&text=Добро пожаловать это ваш код {code}&apiKey=kz9fbea4a753eb49d37bd0249e0994fca8af387144ef14f4825a0fc0ccffac6fdc3cf4");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                Console.WriteLine(product);
            }
        }
    }
}