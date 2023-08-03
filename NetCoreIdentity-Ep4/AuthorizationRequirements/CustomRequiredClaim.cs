using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreIdentity_Ep4
{
    // This is an Authorization Middleware
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        // Constructor that takes a claim type as a parameter.
        public CustomRequireClaim(string claimType)
        {
            // Set the 'ClaimType' property to the value passed through the constructor.
            ClaimType = claimType;
        }

        // A read-only property to store the claim type.
        public string ClaimType { get; }
    }


    // Handler to process the request
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        // This method is an override of the HandleRequirementAsync method from the base class. It is used for handling authorization requirements.
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            // Retrieve the claims associated with the user from the context.
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            // Check if the user has the required claim (specified by requirement.ClaimType).
            if (hasClaim)
            {
                // If the user has the required claim, succeed the authorization requirement.
                context.Succeed(requirement);
            }

            // Return a completed Task since the authorization handling is complete.
            return Task.CompletedTask;
        }

    }

    // For Extension Method -- OPTIONAL
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            return builder.AddRequirements(new CustomRequireClaim(claimType));
        }
    }
}
