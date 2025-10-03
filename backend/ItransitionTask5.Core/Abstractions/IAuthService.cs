using ItransitionTask5.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItransitionTask5.Core.Abstractions
{
    public interface IAuthService
    {
        Task<(Guid userId, List<string> errors)> RegisterAsync(string name, string email, string position, string password );
        Task<(string? token, User? user, string? error)> LoginAsync(string email, string password);
        Task<bool> VerifyEmailAsync(string token);
    }
}
