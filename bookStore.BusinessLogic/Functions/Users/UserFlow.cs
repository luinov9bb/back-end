using bookStore.BusinessLogic.Core.Users;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Functions.Users
{
    public class UserFlow : UserAction, IUserActions
    {
        public List<UserDto> GetAllUsersAction() => ExecuteGetAllUsersAction();

        public UserDto? GetUserByIdAction(int id) => ExecuteGetUserByIdAction(id);
    }
}
