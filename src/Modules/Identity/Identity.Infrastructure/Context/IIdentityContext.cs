using Identity.Domain.Const;
using Identity.Domain.Entities;
using Identity.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Context;

public interface IIdentityContext
{
    public DbSet<UserAvatar> UserAvatars { get; set; }
}

public class IdentityContext(DbContextOptions<IdentityContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim,
        UserToken>(options), IIdentityContext
{
    public DbSet<UserAvatar> UserAvatars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(IdentitySchema.Identity);
        base.OnModelCreating(modelBuilder);

        #region Config

        modelBuilder.ApplyConfiguration(new RoleClaimConfig());
        modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfiguration(new UserAvatarConfig());
        modelBuilder.ApplyConfiguration(new UserClaimConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new UserRoleConfig());
        modelBuilder.ApplyConfiguration(new UserLoginConfig());
        modelBuilder.ApplyConfiguration(new UserTokenConfig());

        #endregion
    }
}