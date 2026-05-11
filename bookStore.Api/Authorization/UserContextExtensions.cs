using System.Security.Claims;
using bookStore.Domain.Enums;

namespace bookStore.Api.Authorization
{
    public static class UserContextExtensions
    {
        public static int? GetCurrentUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }

        public static bool IsAdmin(this ClaimsPrincipal user) =>
            user.IsInRole(UserRole.Admin.ToString());

        public static bool CanAccessUser(this ClaimsPrincipal user, int userId) =>
            user.IsAdmin() || user.GetCurrentUserId() == userId;
    }
}
