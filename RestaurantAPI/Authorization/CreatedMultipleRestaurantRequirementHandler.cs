using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public CreatedMultipleRestaurantRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
        {
            var user = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (user is null) 
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userId = int.Parse(user.Value);
            var restaurantCount = _dbContext
                .Restaurants
                .Count(r => r.CreatedById == userId);

            if (restaurantCount >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
