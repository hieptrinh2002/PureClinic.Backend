﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Enums.PermissionEnums;
using static PureLifeClinic.API.Attributes.PermissionAuthorizeAttribute;

namespace PureLifeClinic.API.Helpers.Authz.PolicyProvider
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {

            // original .net code
            if(!policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                return await base.GetPolicyAsync(policyName);
            }

            // extract the operator AND/OR enum from the policy name    
            PermissionOperator permissionOperator = GetOperatorFromPolicy(policyName);

            // extract the permissions from the policy name 
            int[] permissions = GetPermissionsFromPolicy(policyName);

            string resource = GetResourceFromPolicy(policyName);    

            // create the instance of our custom requirement
            PermissionRequirement requirement = new(permissions, resource, permissionOperator);   

            // use the builder to create a policy, adding our requirement
            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}
