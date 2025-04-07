using System.Transactions;
using APIs;
using APIs.Repository.Interface;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace APIs.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        public UserRepository(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<User> CreateUser(User user)
        {
            if(await _context.Users.AnyAsync(u => u.Email == user.Email))
                throw new InvalidOperationException($"A user already exists.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync(DateTime dateTime)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                //Remove users with no active carts
                var users = await context.Users.Where(u => u.LastRequest == dateTime).ToListAsync();
                for (var i = 0; i < users.Count; i++)
                {
                    if ((await context.Carts.Where(c => c.UserId == users[i].UserId && c.Paid == 0).CountAsync()) == 0)
                        users.Slice(i, 1);
                }

                return users;
            }
            
        }

        public async Task<Role> GetRole(string email)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            return user.Role;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{email}' not found.");
            }

            return user;
        }

        public async Task LogUserActivity(string email)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    var date = DateTime.UtcNow;
                    user.LastRequest = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                    context.Update(user);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SaveCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new KeyNotFoundException($"User with email '{email}' not found.");

            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
