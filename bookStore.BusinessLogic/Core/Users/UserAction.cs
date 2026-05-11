using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace bookStore.BusinessLogic.Core.Users
{
    public class UserAction
    {
        private static UserDto ToDto(UserData u) =>
            new()
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role.ToString(),
                RegisteredOn = u.RegisteredOn,
                IsActive = u.IsActive
            };

        protected List<UserDto> ExecuteGetAllUsersAction()
        {
            using var db = new UserContext();
            return db.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToList()
                .Select(ToDto)
                .ToList();
        }

        protected UserDto? ExecuteGetUserByIdAction(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            using var db = new UserContext();
            var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            return user == null ? null : ToDto(user);
        }
    }
}
