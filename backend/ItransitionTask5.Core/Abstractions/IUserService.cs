using ItransitionTask5.Core.Models;

namespace ItransitionTask5.Core.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUser(Guid id, string name, string email, string position, string passwordHash);
        Task<bool> Delete(string email);
        Task<bool> DeleteMultiple(List<Guid> userIds);
        Task<int> DeleteUnverified();
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetByPage(int page, int pageSize);
        Task<bool> SetStatus(string email, string newStatus);
        Task<bool> BlockUsers(List<Guid> userIds);
        Task<bool> UnblockUsers(List<Guid> userIds);
        Task<User?> GetById(Guid userId);
    }
}