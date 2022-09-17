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
                    $"service/message/sendsmsmessage?recipient={phone}&text=Добро пожаловать это ваш код {code}&apiKey=kzed9bd53a7b9a42af6a9331417cd13c936c357066882224d8a6f8f6b3610afa1ea60e");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                Console.WriteLine(product);
            }
        }
    }
}