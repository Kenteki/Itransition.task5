using ItransitionTask5.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItransitionTask5.Core.Abstractions
{
    public interface IUserRepository
    {
        Task<List<User>> Get();
        Task<Guid> Create(Guid id, string name, string email, string position, string passwordHash);
        Task<User?> GetByEmail(string email);
        Task<User?> GetById(Guid id);
        Task Update(User user);
        Task<bool> Delete(string email);
        Task<bool> DeleteMultiple(List<Guid> userIds);
        Task<List<User>> GetByPage(int page, int pageSize);
        Task<bool> SetStatus(string email, string status);
        Task<bool> SetStatusMultiple(List<Guid> userIds, string status);
        Task<bool> UpdateLastLogin(Guid userId);
        Task<int> DeleteUnverified();
    }
}
