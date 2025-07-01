namespace Carden.Api.Interfaces;

public interface IUserRepository
{
    public Task<User> Create(User user);

    public Task<User?> FindById(Guid userId);

    public Task<User?> FindByEmail(string email);
    
    public Task<User?> Update(User user);

    public Task<Guid?> Delete(Guid userId);
}