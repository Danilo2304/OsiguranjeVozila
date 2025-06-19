using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;

namespace OsiguranjeVozila.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();

            
            var adminUser = await authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "jankovic.danilo23@gmail.com");

            if (adminUser != null)
            {
                users.Remove(adminUser);
            }

            return users;
        }

        public async Task<IdentityUser?> GetByIdAsync(Guid id)
        {
            return await authDbContext.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<IdentityUser> EditAsync(IdentityUser user)
        {
            var existingUser = await authDbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (existingUser!=null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                await authDbContext.SaveChangesAsync();
                return existingUser;
            }

            return null;
        }


    }
}





