using System.Text;

namespace NCBack.Services;

public class PasswordGeneratorService
{
    public static Random rndm = new Random();
    
    private const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string Generate(int size = 8)
    {
        
        string psw = "";
        for (int i = 0; i < size; i++)
        {
            if (rndm.Next(3) == 0)
            {
                psw += rndm.Next(10);
            }
            else
            {
                if (rndm.Next(2) == 0)
                {
                    psw += letters[rndm.Next(letters.Length)].ToString().ToLower();
                }
                else
                {
                    psw += letters[rndm.Next(letters.Length)];
                }
            }
        }
        return psw;
    }
    
    public static string ToHesh(string s)
    {
        const string letters = "KLMNOPQRSATUVWXYZ";
        string str = s;
        StringBuilder sb = new StringBuilder();
 
        for (int i = str.Length - 1; i >=0 ; i--) {
            sb.Append(str[i]);
        }
        
        string start = "";
        string finish = "";
        int size = 7;
            
        for (int i = 0; i < size; i++)
        {
            if (rndm.Next(3) == 0)
            {
                start += rndm.Next(10);
                finish += rndm.Next(10);
            }
            else
            {
                if (rndm.Next(2) == 0)
                {
                    start += letters[rndm.Next(letters.Length)].ToString().ToLower();
                    finish += letters[rndm.Next(letters.Length)].ToString().ToLower();
                }
                else
                {
                    start += letters[rndm.Next(letters.Length)];
                    finish += letters[rndm.Next(letters.Length)];
                }
            }
        }
        string result = $"{start}{sb}{finish}";
        return result;
    }

    public static string OffHesh(string s)
    {
        string str = s;

        str = str.Substring(7).Trim((char)7);
        str = str.Substring(0, str.Length - 7).Trim((char)7);
            
        StringBuilder sb = new StringBuilder();
 
        for (int i = str.Length - 1; i >=0 ; i--) {
            sb.Append(str[i]);
        }
            
        return sb.ToString();
    }
}