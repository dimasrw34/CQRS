using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace InTouch.UserService.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
 private readonly ILogger<PermissionAuthorizationPolicyProvider> _logger;

    public PermissionAuthorizationPolicyProvider(IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options, ILogger<PermissionAuthorizationPolicyProvider> logger) : base(options)
    {     
        _logger = logger;
    }

   public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
   {
        
        try
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка в GetPolicyAsync");
            throw;
        }

        return new AuthorizationPolicyBuilder()
                    .AddRequirements(GetPermissionRequirement(policyName))
                    .Build();
   }

    private PermissionRequirement GetPermissionRequirement(string policyName)
    {
        string[] groups = policyName.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        Permission[] enams = new Permission[groups.Length];

        for (int i = 0; i < groups.Length; i++)
        {
            enams[i] = (Permission)Enum.Parse(typeof(Permission), groups[i].ToString());
        }

        return new PermissionRequirement(enams);
    }
}
