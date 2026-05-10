using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IAuthActions
    {
        ResponceMsg RegisterAction(UserCreateDto dto);
        LoginResultDto LoginAction(ULoginData dto);
    }
}
