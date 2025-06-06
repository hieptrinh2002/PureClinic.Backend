﻿using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    class UserClaimRepository : BaseRepository<IdentityUserClaim<int>>, IUserClaimRepository
    {
        public UserClaimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
