using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoilaMonAvis.DataModel;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class PostsDal
    {
        private static List<Posts> GetListPostByJson(string json)
        {
            JObject o = JObject.Parse(json.ToString());
            List<Posts> listPosts = new List<Posts>();
            foreach (var postJson in o["posts"])
            {
                Posts post = new Posts(postJson.ToString());
                listPosts.Add(post);
            }

            return listPosts;
        }

        private static Posts GetPostByJson(string json)
        {
            JObject o = JObject.Parse(json.ToString());

            return new Posts(o["posts"].First().ToString());

        }


        public async static Task<List<Posts>> GetRecentPost()
        {
            string json = await JsonDal.GetJson(Config.Url_GetRecentPosts);

            return GetListPostByJson(json);
        }
        public async static Task<List<Posts>> GetRecentPost(int page)
        {
            string json = await JsonDal.GetJson(Config.Url_GetRecentPosts + Config.Page + page);

            return GetListPostByJson(json);
        }       

        public async static Task<Posts> GetPost(int id)
        {
            string json = await JsonDal.GetJson(Config.Url_GetPost + id);

            return GetPostByJson(json);
        }

        public async static Task<List<Posts>> GetPostsByDate(DateTime date)
        {
            string json = await JsonDal.GetJson(Config.Url_GetDatePosts + date);

            return GetListPostByJson(json);
        }
        public async static Task<List<Posts>> GetPostsByDate(DateTime date, int page)
        {
            string json = await JsonDal.GetJson(Config.Url_GetDatePosts + date + Config.Page + page);

            return GetListPostByJson(json);
        }

        public async static Task<List<Posts>> GetPostsByCategory(string category)
        {
            string json = await JsonDal.GetJson(Config.Url_GetCategoryPosts + category);

            return GetListPostByJson(json);
        }
        public async static Task<List<Posts>> GetPostsByCategory(string category, int page)
        {
            string json = await JsonDal.GetJson(Config.Url_GetCategoryPosts + category + Config.Page + page);

            return GetListPostByJson(json);
        }

        public async static Task<List<Posts>> GetPostsByTag(string tag)
        {
            string json = await JsonDal.GetJson(Config.Url_GetTagPosts + tag);

            return GetListPostByJson(json);
        }
        public async static Task<List<Posts>> GetPostsByTag(string tag, int page)
        {
            string json = await JsonDal.GetJson(Config.Url_GetTagPosts + tag + Config.Page + page);

            return GetListPostByJson(json);
        }

        public async static Task<List<Posts>> GetPostsByAuthor(int authorsId)
        {
            string json = await JsonDal.GetJson(Config.Url_GetAuthorPosts + authorsId);

            return GetListPostByJson(json);
        }
        public async static Task<List<Posts>> GetPostsByAuthor(int authorsId, int page)
        {
            string json = await JsonDal.GetJson(Config.Url_GetAuthorPosts + authorsId + Config.Page + page);

            return GetListPostByJson(json);
        }

        public async static Task<List<Posts>> Find(string value)
        {
            string json = await JsonDal.GetJson(Config.Url_Search + value);

            return GetListPostByJson(json);
        }

        private static int PageCount(string json)
        {
            JObject o = JObject.Parse(json);

            int pageCount = int.Parse((string)o["pages"]);

            return 0;
        }
    }
}
