using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoilaMonAvis.Data;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class TagsDal
    {
        private static List<Tags> GetListTagsByJson(string json)
        {
            JObject o = JObject.Parse(json.ToString());
            List<Tags> listTags = new List<Tags>();
            foreach (var tagJson in o["tags"])
            {
                Tags tag = new Tags(tagJson.ToString());
                listTags.Add(tag);
            }

            return listTags;
        }


        public async static Task<List<Tags>> GetAllTags()
        {
            string json = await JsonDal.GetJson(Config.Url_GetAllTags);

            return GetListTagsByJson(json);
        }
    }
}
