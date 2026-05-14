using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Enums;
using bookStore.Domain.Models.Base;
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

        private static int ActiveAdminCount(UserContext db) =>
            db.Users.Count(u => u.IsActive && u.Role == UserRole.Admin);

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

        protected ResponceMsg ExecuteAdminUpdateUserAction(AdminUserUpdateDto dto, int actingAdminId)
        {
            if (dto.Id <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор пользователя." };
            }

            var username = dto.Username?.Trim() ?? string.Empty;
            var email = dto.Email?.Trim() ?? string.Empty;

            if (username.Length is < 3 or > 20)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Логин: от 3 до 20 символов." };
            }

            if (email.Length is < 3 or > 30)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Email: от 3 до 30 символов." };
            }

            if (!Enum.TryParse<UserRole>(dto.Role?.Trim(), ignoreCase: true, out var newRole) ||
                !Enum.IsDefined(typeof(UserRole), newRole))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Роль должна быть User или Admin." };
            }

            var pwd = dto.NewPassword?.Trim();
            if (!string.IsNullOrEmpty(pwd) && pwd.Length < 8)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Новый пароль не короче 8 символов." };
            }

            using var db = new UserContext();
            var user = db.Users.FirstOrDefault(u => u.Id == dto.Id);
            if (user == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь не найден." };
            }

            if (db.Users.Any(u => u.Id != dto.Id && u.Username == username))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Логин уже занят другим пользователем." };
            }

            if (db.Users.Any(u => u.Id != dto.Id && u.Email == email))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Email уже занят другим пользователем." };
            }

            if (actingAdminId == user.Id && (!dto.IsActive || newRole != UserRole.Admin))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Нельзя отключить или понизить свою учётную запись." };
            }

            var lastAdminErr = ValidateNotLastAdminRemoval(db, user, newRole, dto.IsActive);
            if (lastAdminErr != null)
            {
                return lastAdminErr;
            }

            user.Username = username;
            user.Email = email;
            user.Role = newRole;
            user.IsActive = dto.IsActive;

            if (!string.IsNullOrEmpty(pwd))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(pwd);
            }

            db.SaveChanges();
            return new ResponceMsg { IsSuccess = true, Message = "Пользователь обновлён." };
        }

        protected ResponceMsg ExecuteAdminSoftDeleteUserAction(int targetUserId, int actingAdminId)
        {
            if (targetUserId <= 0)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Некорректный идентификатор пользователя." };
            }

            if (targetUserId == actingAdminId)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Нельзя отключить свою учётную запись." };
            }

            using var db = new UserContext();
            var user = db.Users.FirstOrDefault(u => u.Id == targetUserId);
            if (user == null)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь не найден." };
            }

            if (!user.IsActive)
            {
                return new ResponceMsg { IsSuccess = true, Message = "Учётная запись уже отключена." };
            }

            var lastAdminErr = ValidateNotLastAdminRemoval(db, user, user.Role, newIsActive: false);
            if (lastAdminErr != null)
            {
                return lastAdminErr;
            }

            user.IsActive = false;
            db.SaveChanges();
            return new ResponceMsg { IsSuccess = true, Message = "Пользователь отключён." };
        }

        private static ResponceMsg? ValidateNotLastAdminRemoval(
            UserContext db,
            UserData target,
            UserRole newRole,
            bool newIsActive)
        {
            var wasActiveAdmin = target.IsActive && target.Role == UserRole.Admin;
            var removesAdminStatus = newRole != UserRole.Admin || !newIsActive;
            if (!wasActiveAdmin || !removesAdminStatus)
            {
                return null;
            }

            if (ActiveAdminCount(db) <= 1)
            {
                return new ResponceMsg
                {
                    IsSuccess = false,
                    Message = "Нельзя отключить или понизить единственного администратора.",
                };
            }

            return null;
        }
    }
}
