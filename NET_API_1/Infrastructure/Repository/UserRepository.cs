using Microsoft.EntityFrameworkCore;
using NET_API_1.Infrastructure.Data;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Models.Entity;
using NET_API_1.Utils;
using System.Collections.Generic;

namespace NET_API_1.Infrastructure.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(User entity)
        {
            entity.Status = 2;
            Update(entity);
        }

        public void Insert(User entity)
        {
            _dbContext.User.Add(entity);
        }
        public void Update(User entity)
        {
            _dbContext.User.Update(entity);
        }

        public async Task<PaginatedList<User>> GetListAsync(int pageNumber, int pageSize)
        {
            var user = await _dbContext.User.ToListAsync();
            return new PaginatedList<User>(user, pageNumber, pageSize);
        }
        public async Task<User?> GetUserByIdAsync(int UserId)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == UserId);
        }
        public async Task<User?> GetUserAsync(string Email = "")
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.Email == Email);
        }

    }
}