using ItransitionTask5.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using ItransitionTask5.Core.Abstractions;
using ItransitionTask5.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItransitionTask5.Persistence.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> Get()
        {
            var entities = await _dbContext.Users
                            .AsNoTracking()
                            .OrderBy(u => u.LastLogin)
                            .ToListAsync();
            return entities
                .Select(e => User.Load(
                    e.Id,
                    e.Name,
                    e.Email,
                    e.Position,
                    e.PasswordHash,
                    e.RegistrationTime,
                    e.LastLogin,
                    e.Status))
                .ToList();
        }

        public async Task<List<User>> GetByPage(int page, int pageSize)
        {
            var entities = await _dbContext.Users
                .AsNoTracking()
                .OrderBy(u => u.LastLogin)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entities
                .Select(e => User.Load(
                    e.Id,
                    e.Name,
                    e.Email,
                    e.Position,
                    e.PasswordHash,
                    e.RegistrationTime,
                    e.LastLogin,
                    e.Status))
                .ToList();
        }

        public async Task<User?> GetByEmail(string email)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (entity == null)
                return null;

            return User.Load(
                entity.Id,
                entity.Name,
                entity.Email,
                entity.Position,
                entity.PasswordHash,
                entity.RegistrationTime,
                entity.LastLogin,
                entity.Status);
        }

        public async Task<User?> GetById(Guid id)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
                return null;

            return User.Load(
                entity.Id,
                entity.Name,
                entity.Email,
                entity.Position,
                entity.PasswordHash,
                entity.RegistrationTime,
                entity.LastLogin,
                entity.Status);
        }

        public async Task Update(User user)
        {
            var entity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Position = user.Position,
                PasswordHash = user.PasswordHash,
                RegistrationTime = user.RegistrationTime,
                LastLogin = user.LastLogin,
                Status = user.Status
            };

            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Guid> Create(Guid id, string name, string email, string position, string passwordHash)
        {
            var userEntity = new UserEntity
            {
                Id = id,
                Name = name,
                Email = email,
                Position = position,
                PasswordHash = passwordHash,
                RegistrationTime = DateTime.UtcNow,
                Status = "Unverified"
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();

            return id;
        }

        // START of Two status change functions for case by user selection (single,multiple)
        public async Task<bool> SetStatus(string email, string status)
        {
            var rows = await _dbContext.Users
                .Where(u => u.Email == email)
                .ExecuteUpdateAsync(u => u.SetProperty(user => user.Status, status));

            return rows > 0;
        }

        public async Task<bool> SetStatusMultiple(List<Guid> userIds, string status)
        {
            var rows = await _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ExecuteUpdateAsync(u => u.SetProperty(user => user.Status, status));

            return rows > 0;
        }
        // END of Two functions for case by user selection (single,multiple)

        public async Task<bool> UpdateLastLogin(Guid userId)
        {
            var rows = await _dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(
                    user => user.LastLogin,
                    DateTime.UtcNow));

            return rows > 0;
        }

        // START of Two delete functions for case by user selection (single,multiple)
        public async Task<bool> Delete(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMultiple(List<Guid> userIds)
        {
            var users = await _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            if (!users.Any()) return false;

            _dbContext.Users.RemoveRange(users);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        // END of Two delete functions for case by user selection (single,multiple)

        public async Task<int> DeleteUnverified()
        {
            var unverifiedUsers = await _dbContext.Users
                .Where(u => u.Status == "Unverified")
                .ToListAsync();

            if (!unverifiedUsers.Any()) return 0;

            _dbContext.Users.RemoveRange(unverifiedUsers);
            await _dbContext.SaveChangesAsync();

            return unverifiedUsers.Count;
        }
    }
}
