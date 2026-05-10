using bookStore.BusinessLogic.Core.Auth;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.User;

namespace bookStore.BusinessLogic.Functions.Auth
{
    public class AuthFlow : AuthAction, IAuthActions
    {
        public ResponceMsg RegisterAction(UserCreateDto dto) => ExecuteRegisterAction(dto);

        public LoginResultDto LoginAction(ULoginData dto) => ExecuteLoginAction(dto);
    }
}
