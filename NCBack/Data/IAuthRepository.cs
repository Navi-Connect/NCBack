using Microsoft.AspNetCore.Mvc;
using NCBack.Dtos.User;
using NCBack.Models;

namespace NCBack.Data;

public interface IAuthRepository
{
    Task<User> Register(string city, string region, 
        string username, string firstname, 
        string lastname, string surname, DateTime dateOfBirth,
        IFormFile file, string password);
    Task<User> Login(string username, string password);
    Task<bool> UserExists(string username);
    Task<User> ChangePassword(UserChangePasswordDto request);
}