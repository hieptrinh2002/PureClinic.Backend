using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Enums;
using static PureLifeClinic.API.Helpers.Auth.PolicyProvider.PermissionAuthorizeAttribute;

namespace PureLifeClinic.API.Helpers.Auth.PolicyProvider
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
            string[] permissions = GetPermissionsFromPolicy(policyName);

            // create the instance of our custom requirement

            PermissionRequirement requirement = new(permissions, permissionOperator);

            // use the builder to create a policy, adding our requirement
            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}
