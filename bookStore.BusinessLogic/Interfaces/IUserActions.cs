using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IUserActions
    {
        List<UserDto> GetAllUsersAction();
        UserDto? GetUserByIdAction(int id);
        ResponceMsg AdminUpdateUserAction(AdminUserUpdateDto dto, int actingAdminId);
        ResponceMsg AdminSoftDeleteUserAction(int targetUserId, int actingAdminId);
    }
}
