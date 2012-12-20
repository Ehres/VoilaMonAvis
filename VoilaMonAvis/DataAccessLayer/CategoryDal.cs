using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoilaMonAvis.Data;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class CategoryDal
    {
        private static List<Category> GetListCategoriesByJson(string json)
        {
            JObject o = JObject.Parse(json.ToString());
            List<Category> listCategories = new List<Category>();
            foreach (var categoryJson in o["categories"])
            {
                Category category = new Category(categoryJson.ToString());
                listCategories.Add(category);
            }

            return listCategories;
        }

        public async static Task<List<Category>> GetAllCategories()
        {
            string json = await JsonDal.GetJson(Config.Url_GetAllCategories);

            return GetListCategoriesByJson(json);
        }
    }
}
