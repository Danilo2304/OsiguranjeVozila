using Microsoft.AspNetCore.Identity;

namespace OsiguranjeVozila.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();

        public Task<IdentityUser?> GetByIdAsync(Guid id);

        public Task<IdentityUser> EditAsync(IdentityUser user);
    }
}
