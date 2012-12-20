using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoilaMonAvis.Data;

namespace VoilaMonAvis.DataAccessLayer
{
    public static class UsersDal
    {

        private static List<Users> GetListUsersByJson(string json)
        {
            JObject o = JObject.Parse(json.ToString());
            List<Users> listUsers = new List<Users>();
            foreach (var postJson in o["authors"])
            {
                Users post = new Users(postJson.ToString());
                listUsers.Add(post);
            }

            return listUsers;
        }

        public async static Task<List<Users>> GetAllAuthors()
        {
            string json = await JsonDal.GetJson(Config.Url_GetAllAuthors);

            return GetListUsersByJson(json);
        }
    }
}
