using bookStore.Domain.Entities;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface ISession
    {
        
        UserMinimal UserLogin(ULoginData data);
    }
}