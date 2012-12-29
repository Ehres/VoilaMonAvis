using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataModel
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Category_Slug { get; set; }
        public string Category_Title { get; set; }
        public string Category_Description { get; set; }
        public int Category_Parent_Id { get; set; }
        public int Category_Post_Count { get; set; }
        public List<Tags> Category_Tags { get; set; }

        public Category()
        {
        }

        public Category(string json)
        {
            JObject jObject = JObject.Parse(json);
            Category_Id = (int)jObject["id"];
            Category_Slug = (string)jObject["slug"];
            Category_Title = (string)jObject["title"];
            Category_Description = (string)jObject["description"];
            Category_Parent_Id = (int)jObject["parent"];
            Category_Post_Count = (int)jObject["post_count"];

            Category_Tags = new List<Tags>();
            
            if (jObject["tags"] != null)
            {
                JToken jTokencategories = jObject["tags"];
                foreach (var tagJson in jTokencategories)
                {
                    Tags tag = new Tags(tagJson.ToString());
                    Category_Tags.Add(tag);
                }
            }            
        }
    }
}
