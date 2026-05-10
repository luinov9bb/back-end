using bookStore.BusinessLogic.Core.Favorites;
using bookStore.BusinessLogic.Interfaces;
using bookStore.Domain.Models.Base;
using bookStore.Domain.Models.Favorite;

namespace bookStore.BusinessLogic.Functions.Favorite
{
    public class FavoriteFlow : FavoriteAction, IFavoriteActions
    {
        public List<FavoriteDto> GetAllFavoritesAction() => ExecuteGetAllFavoritesAction();

        public List<FavoriteDto> GetFavoritesByUserAction(int userId) => ExecuteGetFavoritesByUserAction(userId);

        public FavoriteDto? GetFavoriteByIdAction(int id) => ExecuteGetFavoriteByIdAction(id);

        public ResponceMsg ResponseFavoriteCreateAction(AddFavoriteDto dto) => ExecuteFavoriteCreateAction(dto);

        public ResponceMsg ResponseFavoriteUpdateAction(FavoriteDto dto) => ExecuteFavoriteUpdateAction(dto);

        public ResponceMsg ResponseFavoriteDeleteAction(int id) => ExecuteFavoriteDeleteAction(id);
    }
}
