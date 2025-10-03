using ItransitionTask5.Core.Abstractions;
using ItransitionTask5.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ItransitionTask5.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }


        // Registration of user with hashed password also register validation for duplicates, generating verificationToken, send verification email (async).
        public async Task<(Guid userId, List<string> errors)> RegisterAsync(string name, string email, string position, string password )
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var (user, errors) = User.Create(Guid.NewGuid(), name, email, position, passwordHash);

            if (errors.Any())
                return (Guid.Empty, errors);

            try
            {
                var userId = await _userRepository.Create(
                    user!.Id,
                    user.Name,
                    user.Email,
                    user.Position,
                    user.PasswordHash
                );

                var verificationToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.Id.ToString()));

                _ = Task.Run(async () =>
                {
                    await _emailService.SendVerificationEmailAsync(user.Email, user.Name, verificationToken);
                });

                return (userId, new List<string>());
            }
            catch (Exception)
            {
                errors.Add("Email already exists. Please use a different email or login.");
                return (Guid.Empty, errors);
            }
        }

        // Insite this function we find user by email, check status, verify password, and put last login timestamp
        public async Task<(string? token, User? user, string? error)> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                return (null, null, "Invalid email or password.");

            if (user.Status == "Blocked")
                return (null, null, "Your account has been blocked. Please contact support.");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (null, null, "Invalid email or password.");

            await _userRepository.UpdateLastLogin(user.Id);
            user.UpdateLastLogin();

            var token = _jwtService.GenerateToken(user.Id, user.Email);
            return (token, user, null);
        }
        public async Task<bool> VerifyEmailAsync(string token)
        {
            try
            {
                var userIdBytes = Convert.FromBase64String(token);
                var userIdString = System.Text.Encoding.UTF8.GetString(userIdBytes);
                var userId = Guid.Parse(userIdString);

                var user = await _userRepository.GetById(userId);

                if (user == null)
                    return false;

                // Verification if user has status "Unverified"
                if (user.Status == "Unverified")
                {
                    await _userRepository.SetStatus(user.Email, "Active");
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
