using NCBack.Dtos.User;
using NCBack.Models;

namespace NCBack.Data;

public interface IAuthRepository
{
    Task<User> VerificationCode(int? id, int code);

    Task<IntermediateUser> Register(int cityId, string email,
        string username, string fullname, DateTime dateOfBirth, int genderId,
        IFormFile file, string password);

    Task<User> Login(string username, string password, string? deviceId);
    Task<bool> UserExists(string username);
    Task<User> ChangePassword(UserChangePasswordDto request);

    Task<IntermediateUser> SMSNotReceived(int? id, string phone);
    /*Task<User> ForgotPassword(string email);*/
}