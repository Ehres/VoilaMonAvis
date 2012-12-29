using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataModel
{
    public class Tags
    {
        public int Tag_Id { get; set; }
        public string Tag_Slug { get; set; }
        public string Tag_Title { get; set; }
        public string Tag_Description { get; set; }
        public int Tag_Post_Count { get; set; }

        public Tags() { }

        public Tags(string json)
        {
            JObject jObject = JObject.Parse(json);
            Tag_Id = (int)jObject["id"];
            Tag_Slug = (string)jObject["slug"];
            Tag_Title = (string)jObject["title"];
            Tag_Description = (string)jObject["description"];
            Tag_Post_Count = (int)jObject["post_count"];
        }
    }
}
