namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Auth { get; }
        IProductRepository Products { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    } 
}
