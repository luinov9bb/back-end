using bookStore.BusinessLogic.Core.Users;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Functions.Users
{
    public class UserFlow : UserAction, IUserActions
    {
        public List<UserDto> GetAllUsersAction() => ExecuteGetAllUsersAction();

        public UserDto? GetUserByIdAction(int id) => ExecuteGetUserByIdAction(id);

        public ResponceMsg AdminUpdateUserAction(AdminUserUpdateDto dto, int actingAdminId) =>
            ExecuteAdminUpdateUserAction(dto, actingAdminId);

        public ResponceMsg AdminSoftDeleteUserAction(int targetUserId, int actingAdminId) =>
            ExecuteAdminSoftDeleteUserAction(targetUserId, actingAdminId);
    }
}
