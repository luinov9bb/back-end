using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IUserActions
    {
        List<UserDto> GetAllUsersAction();
        UserDto? GetUserByIdAction(int id);
    }
}
