using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoilaMonAvis.Data;

namespace VoilaMonAvis_FromScratch_.DataModel
{
    public class Posts
    {
        public int ID { get; set; }
        public Users Post_Author { get; set; }
        public DateTime Post_Date { get; set; }
        public DateTime Post_Date_GMT { get; set; }
        public string Post_Content { get; set; }
        public string Post_Content_Next { get; set; }
        public string Post_Title { get; set; }
        public string Post_Excerpt { get; set; }
        public string Post_Status { get; set; }
        public string Comment_Status { get; set; }
        public string Ping_Status { get; set; }
        public string Post_Password { get; set; }
        public DateTime Post_Modified { get; set; }
        public string Post_Type { get; set; }
        public string Post_Mime_Type { get; set; }
        public int Comment_Count { get; set; }
        public string Post_Url { get; set; }
        public List<Category> Post_Categories { get; set; }
        public string Post_Image_Full_Url { get; set; }
        public Uri Post_Video_Url { get; set; }

        public Posts(string json)
        {
            JObject jObject = JObject.Parse(json);
            ID = (int)jObject["id"];
            Post_Type = (string)jObject["type"];
            Post_Url = (string)jObject["url"];
            Post_Status = (string)jObject["status"];
            Post_Title = (string)jObject["title"];
            Post_Content = (string)jObject["content"];
            Post_Content_Next = "";

            Post_Author = new Users(jObject["author"].ToString());
            Post_Url = (string)jObject["url"];
            Post_Status = (string)jObject["status"];
            Post_Excerpt = (string)jObject["excerpt"];
            Post_Date = (DateTime)jObject["date"];
            Post_Modified = (DateTime)jObject["modified"];


            Post_Categories = new List<Category>();
            JToken jTokencategories = jObject["categories"];

            foreach (var categoryJson in jTokencategories)
            {
                Category category = new Category(categoryJson.ToString());
                Post_Categories.Add(category);
            }

            JToken jTokenAttachments = jObject["attachments"];
            foreach (var attachmentJson in jTokenAttachments)
            {
                JObject jObjectAttachmentJson = (JObject)(attachmentJson);
                JObject jObjectImages = (JObject)jObjectAttachmentJson["images"];
                JToken jObjectImageFull = jObjectImages["full"];
                Post_Image_Full_Url = jObjectImageFull["url"].ToString();
            }

            int indexOfOpenIframe = 0;
            int indexOfCloseIframe = 0;
            bool drapForSearchVideo = true;
            string content = Post_Content;


            while (drapForSearchVideo)
            {
                //j'enlève la balise de fermetue iframe
                if (content.Substring(0, 9) == "</iframe>")
                    content = content.Substring(9);

                indexOfOpenIframe = content.IndexOf("<iframe");
                indexOfCloseIframe = content.IndexOf("</iframe>");
                int lenghtOfIframe = indexOfCloseIframe - indexOfOpenIframe;

                if (content.Length > lenghtOfIframe && lenghtOfIframe > 0)
                {
                    string iframe = content.Substring(indexOfOpenIframe, lenghtOfIframe);
                    iframe = iframe.Replace("\\", "");
                    if (!iframe.Contains("facebook"))
                    {
                        int indexSrc = iframe.IndexOf(" src=\"");
                        content = iframe.Substring(indexSrc + 6);
                        int indexOfEndLink = content.IndexOf("\"");
                        Post_Video_Url = new Uri(content.Substring(0, indexOfEndLink));
                        Post_Content_Next = Post_Content.Substring(Post_Content.IndexOf(content) + indexOfEndLink);
                        int endOfIframeMovie = Post_Content_Next.IndexOf("</iframe>") + 9;
                        Post_Content_Next = Post_Content_Next.Substring(endOfIframeMovie);

                        Post_Content = Post_Content.Substring(0, Post_Content.Length - 13);

                        drapForSearchVideo = false;
                    }
                    else
                        content = content.Substring(indexOfCloseIframe);
                }
                else
                    drapForSearchVideo = false;
            }
        }

        public Posts() { }
    }
}
