using Boilerplate.Web.Mvc.OpenGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Infrastructure.OpenGraph.Extensions
{
    public static class TypeExtensions
    {
        public static string ToLowercaseString(this OpenGraphType type)
        {
            switch (type)
            {
                case OpenGraphType.Article: return "article";
                case OpenGraphType.Book: return "book";
                case OpenGraphType.BooksAuthor: return "books.author";
                case OpenGraphType.BooksBook: return "books.book";
                case OpenGraphType.BooksGenre: return "books.genre";
                case OpenGraphType.Business: return "business.business";
                case OpenGraphType.FitnessCourse: return "fitness.course";
                case OpenGraphType.GameAchievement: return "game.achievement";
                case OpenGraphType.MusicAlbum: return "music.album";
                case OpenGraphType.MusicPlaylist: return "music.playlist";
                case OpenGraphType.MusicRadioStation: return "music.radio_station";
                case OpenGraphType.MusicSong: return "music.song";
                case OpenGraphType.Place: return "place";
                case OpenGraphType.Product: return "product";
                case OpenGraphType.ProductGroup: return "product.group";
                case OpenGraphType.ProductItem: return "product.item";
                case OpenGraphType.Profile: return "profile";
                case OpenGraphType.RestaurantMenu: return "restaurant.menu";
                case OpenGraphType.RestaurantMenuItem: return "restaurant.menu_item";
                case OpenGraphType.RestaurantMenuSection: return "restaurant.menu_section";
                case OpenGraphType.Restaurant: return "restaurant.restaurant";
                case OpenGraphType.VideoEpisode: return "video.episode";
                case OpenGraphType.VideoMovie: return "video.movie";
                case OpenGraphType.VideoOther: return "video.other";
                case OpenGraphType.VideoTvShow: return "video.tv_show";
                default: return "website";
            }
        }
    }
}