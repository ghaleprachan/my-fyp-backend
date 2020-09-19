using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Models.Favorites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class FavoritesServices
    {
        private readonly RoofCareEntities _dbContext;
        public FavoritesServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal ResponseModel AddToFavorites(FavoritesModel favorites)
        {
            try
            {
                using (_dbContext)
                {
                    int favoriteBy = EncodeDecode.GetUserId(favorites.favoriteBy);
                    int favoritedTo = EncodeDecode.GetUserId(favorites.favoriteTo);

                    var old_fav = (from favorite in _dbContext.Favorites
                                   select new
                                   {
                                       favorite.FavoriteId,
                                       favorite.CustomerId,
                                       favorite.SPId,
                                   }).Where(f => f.CustomerId == favoriteBy && f.SPId == favoritedTo).ToList().FirstOrDefault();
                    if (old_fav != null)
                    {
                        Favorite remove_old = _dbContext.Favorites.Find(old_fav.FavoriteId);
                        _dbContext.Favorites.Remove(remove_old);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._removedSuccessfully, "Removed Successfully");
                    }
                    else
                    {
                        Favorite new_fav = new Favorite
                        {
                            CustomerId = favoriteBy,
                            SPId = favoritedTo,
                            AddedDate = DateTime.Now
                        };
                        _dbContext.Favorites.Add(new_fav);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Added to favorites");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object GetUserFavorites(UserIdModel tokenNumber)
        {
            try
            {
                using (_dbContext)
                {
                    int id = EncodeDecode.GetUserId(tokenNumber.tokenNumber);
                    var user_favs = (from fav in _dbContext.Favorites
                                     select new
                                     {
                                         fav.FavoriteId,
                                         fav.CustomerId,
                                         fav.SPId,
                                         fav.User1.UserProfileImage,
                                         fav.User1.UserRating,
                                         fav.User1.FullName,
                                         fav.AddedDate,
                                         fav.User1.UserId,
                                         fav.User1.Username
                                     }).Where(v => v.CustomerId == id).ToList();

                    return HelperClass.Response(true, GlobalDecleration._successAction, user_favs);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel GetFavUserId(FavoritesModel favorites)
        {
            try
            {
                using (_dbContext)
                {
                    int favoriteBy = EncodeDecode.GetUserId(favorites.favoriteBy);
                    int favoritedTo = EncodeDecode.GetUserId(favorites.favoriteTo);

                    var user_favs = (from fav in _dbContext.Favorites
                                     select new
                                     {
                                         fav.FavoriteId,
                                         fav.CustomerId,
                                         fav.SPId,
                                         fav.User1.Username
                                     }).Where(v => v.CustomerId == favoriteBy && v.SPId == favoritedTo).ToList().FirstOrDefault();
                    if (user_favs != null)
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, "yes");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "no");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
    }
}