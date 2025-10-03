using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItransitionTask5.Core.Abstractions;
using ItransitionTask5.Core.Models;

namespace ItransitionTask5.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateUser(Guid id, string name, string email, string position, string passwordHash)
        {
            var (user, errors) = User.Create(id, name, email, position, passwordHash);

            if (errors.Any())
                throw new ArgumentException(string.Join("; ", errors));

            return await _userRepository.Create(
                user!.Id,
                user.Name,
                user.Email,
                user.Position,
                user.PasswordHash
            );
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var result = await _userRepository.Get();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User?> GetById(Guid userId)
        {
            return await _userRepository.GetById(userId);
        }

        public async Task<bool> SetStatus(string email, string newStatus)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user == null) return false;

            return await _userRepository.SetStatus(email, newStatus);
        }

        public async Task<bool> Delete(string email)
        {
            return await _userRepository.Delete(email);
        }
        public async Task<List<User>> GetByPage(int page, int pageSize)
        {
            return await _userRepository.GetByPage(page, pageSize);
        }

        public async Task<bool> DeleteMultiple(List<Guid> userIds)
        {
            if (!userIds.Any()) return false;
            return await _userRepository.DeleteMultiple(userIds);
        }

        public async Task<int> DeleteUnverified()
        {
            return await _userRepository.DeleteUnverified();
        }

        public async Task<bool> BlockUsers(List<Guid> userIds)
        {
            if (!userIds.Any()) return false;
            return await _userRepository.SetStatusMultiple(userIds, "Blocked");
        }

        public async Task<bool> UnblockUsers(List<Guid> userIds)
        {
            if (!userIds.Any()) return false;
            return await _userRepository.SetStatusMultiple(userIds, "Active");
        }


    }
}
