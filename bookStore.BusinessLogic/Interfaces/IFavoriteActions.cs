using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Favorite;

namespace bookStore.BusinessLogic.Interfaces
{
    public interface IFavoriteActions
    {
        List<FavoriteDto> GetAllFavoritesAction();
        List<FavoriteDto> GetFavoritesByUserAction(int userId);
        FavoriteDto? GetFavoriteByIdAction(int id);
        ResponceMsg ResponseFavoriteCreateAction(AddFavoriteDto dto);
        ResponceMsg ResponseFavoriteUpdateAction(FavoriteDto dto);
        ResponceMsg ResponseFavoriteDeleteAction(int id);
    }
}
