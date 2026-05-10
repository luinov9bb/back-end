using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Enums;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Core.Auth
{
    public class AuthAction
    {
        private static UserDto ToPublicUserDto(UserData u) =>
            new()
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role.ToString(),
                RegisteredOn = u.RegisteredOn,
                IsActive = u.IsActive
            };

        protected ResponceMsg ExecuteRegisterAction(UserCreateDto dto)
        {
            var username = dto.Username?.Trim() ?? string.Empty;
            var email = dto.Email?.Trim() ?? string.Empty;
            var password = dto.Password ?? string.Empty;

            if (username.Length is < 3 or > 20)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Логин: от 3 до 20 символов." };
            }

            if (email.Length is < 3 or > 30)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Email: от 3 до 30 символов." };
            }

            if (password.Length < 8)
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пароль не короче 8 символов." };
            }

            using var db = new UserContext();
            if (db.Users.Any(u => u.Username == username))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь с таким логином уже есть." };
            }

            if (db.Users.Any(u => u.Email == email))
            {
                return new ResponceMsg { IsSuccess = false, Message = "Пользователь с таким email уже есть." };
            }

            var user = new UserData
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RegisteredOn = DateTime.UtcNow,
                Role = UserRole.User,
                IsActive = true
            };

            db.Users.Add(user);
            db.SaveChanges();

            return new ResponceMsg { IsSuccess = true, Message = "Регистрация успешна." };
        }

        protected LoginResultDto ExecuteLoginAction(ULoginData dto)
        {
            var credential = dto.Credential?.Trim() ?? string.Empty;
            var password = dto.Password ?? string.Empty;

            if (credential.Length == 0 || password.Length == 0)
            {
                return new LoginResultDto { IsSuccess = false, Message = "Укажите логин/email и пароль." };
            }

            using var db = new UserContext();
            var user = db.Users.FirstOrDefault(u =>
                u.Username == credential || u.Email == credential);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new LoginResultDto { IsSuccess = false, Message = "Неверный логин или пароль." };
            }

            if (!user.IsActive)
            {
                return new LoginResultDto { IsSuccess = false, Message = "Учётная запись отключена." };
            }

            return new LoginResultDto
            {
                IsSuccess = true,
                Message = "OK",
                User = ToPublicUserDto(user)
            };
        }
    }
}
