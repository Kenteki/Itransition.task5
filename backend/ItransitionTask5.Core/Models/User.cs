using System;
using System.Collections.Generic;
using System.Linq;

namespace ItransitionTask5.Core.Models
{
    public class User
    {
        public const int MIN_PASSWORD_LENGTH = 1;
        public const int MAX_NAME_LENGTH = 100;

        private User(Guid id, string name, string email, string position, string passwordHash)
        {
            Id = id;
            Name = name;
            Email = email;
            Position = position ?? "";
            PasswordHash = passwordHash;
            RegistrationTime = DateTime.UtcNow;
            Status = "Unverified";
        }

        private User(Guid id, string name, string email, string position, string passwordHash,
                    DateTime registrationTime, DateTime? lastLogin, string status)
        {
            Id = id;
            Name = name;
            Email = email;
            Position = position;
            PasswordHash = passwordHash;
            RegistrationTime = registrationTime;
            LastLogin = lastLogin;
            Status = status;
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string Position { get; } = string.Empty;
        public string PasswordHash { get; } = string.Empty;
        public DateTime RegistrationTime { get; }
        public DateTime? LastLogin { get; private set; }
        public string Status { get; set; } = string.Empty;

        public static (User? user, List<string> errors) Create(Guid id, string name, string email, string position, string passwordHash)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
                errors.Add("Name cannot be empty or longer than " + MAX_NAME_LENGTH + " characters.");

            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email cannot be empty.");
            else if (!IsValidEmail(email))
                errors.Add("Email format is invalid.");

            if (string.IsNullOrWhiteSpace(passwordHash) || passwordHash.Length < MIN_PASSWORD_LENGTH)
                errors.Add("Password must be at least " + MIN_PASSWORD_LENGTH + " characters.");

            if (errors.Any())
                return (null, errors);

            var user = new User(id, name, email, position, passwordHash);
            return (user, errors);
        }
        public static User Load(Guid id, string name, string email, string position, string passwordHash,
                               DateTime registrationTime, DateTime? lastLogin, string status)
        {
            return new User(id, name, email, position, passwordHash, registrationTime, lastLogin, status);
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string GetUniqIdValue()
        {
            return Id.ToString();
        }
    }
}