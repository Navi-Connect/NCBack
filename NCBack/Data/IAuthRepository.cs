using NCBack.Dtos.User;
using NCBack.Models;

namespace NCBack.Data;

public interface IAuthRepository
{
    Task<User> VerificationCode(int code);
    Task<User> Register( string city, string phone, string email,
        string username, string fullname, DateTime dateOfBirth,
        IFormFile file, string password);
    Task<User> Login(string username, string password);
    Task<bool> UserExists(string username);
    Task<User> ChangePassword(UserChangePasswordDto request);
    Task<User> SMSNotReceived(string phone, int? id);
    /*Task<User> ForgotPassword(string email);*/
}