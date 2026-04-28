using bookStore.BusinessLogic.Core;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Entities;

namespace bookStore.BusinessLogic
{
    // нследуемся от UserApi и реализуем интерфейс ISession
    public class SessionBL : UserApi, ISession
    {
        public UserMinimal UserLogin(ULoginData data)
        {
            //  проверка (потом заменим на запрос к БД)
            if (data.Credential == "admin" && data.Password == "123")
            {
                return new UserMinimal { Status = true, Username = "Admin" };
            }

            return new UserMinimal { Status = false, StatusMsg = "Неверный логин или пароль" };
        }
    }
}