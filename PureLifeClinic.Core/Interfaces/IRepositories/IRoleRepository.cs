﻿using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Role GetPatientRole();
    }
}
