namespace Carden.Api.Interfaces;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken> Create(RefreshToken refreshToken);

    public Task<RefreshToken?> FindById(Guid id);

    public Task<RefreshToken[]> FindByUser(Guid id);

    public Task<RefreshToken?> Revoke(Guid id);
    
    public Task<Guid?> Delete(Guid userId, Guid refreshId);
}