using ElevenNote.Models.User;
using ElevenNote.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ElevenNote.Data;

namespace ElevenNote.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(UserRegister model)
        {
            if (await GetUserByEmailAsync(model.Email) != null || await GetUserByUsernameAsync(model.Username) != null)
                return false;

            UserEntity entity = new UserEntity
            {
                Email = model.Email,
                Username = model.Username,
                Password = model.Password, // Be aware of storing passwords as plain text!
                DateCreated = DateTime.Now
            };

            _context.Users.Add(entity);
            int numberOfChanges = await _context.SaveChangesAsync();

            return numberOfChanges == 1;
        }

        private async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(User => User.Email.ToLower() == email.ToLower());
        }

        private async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
        }
    }
}
