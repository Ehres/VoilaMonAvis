using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class Config
    {
        private static string Url_Site = "http://www.voilamonavis.fr/";

        public static string Url_GetRecentPosts = Url_Site + "?json=get_recent_posts";
        public static string Url_GetPost = Url_Site + "?json=get_post&id=";
        public static string Url_GetDatePosts = Url_Site + "?json=get_date_posts&date=";
        public static string Url_GetCategoryPosts = Url_Site + "?json=get_category_posts&id=";
        public static string Url_GetTagPosts = Url_Site + "?json=get_tag_posts&id=";
        public static string Url_GetAuthorPosts = Url_Site + "?json=get_author_posts&id=";
        public static string Url_Search = Url_Site + "?json=get_search_results&search=";
        public static string Url_GetAllCategories = Url_Site + "?json=get_category_index";
        public static string Url_GetAllTags = Url_Site + "?json=get_tag_index";
        public static string Url_GetAllAuthors = Url_Site + "?json=get_author_index";
        public static string Url_GetAllPages = Url_Site + "?json=get_page_index";
    }
}
