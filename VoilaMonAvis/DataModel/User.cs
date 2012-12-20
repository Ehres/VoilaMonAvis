using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.Data
{
    public class Users
    {
        public int User_Id { get; set; }
        public string User_Login { get; set; }
        public string User_Pass { get; set; }
        public string User_Nickname { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_Email { get; set; }
        public string User_Url { get; set; }
        public DateTime User_Registered { get; set; }
        public string User_Activation_Key { get; set; }
        public int User_Status { get; set; }
        public string Display_Name { get; set; }
        public string User_Description { get; set; }

        public Users(string json)
        {
            JObject jObject = JObject.Parse(json);
            User_Id = (int)jObject["id"];
            User_Nickname = (string)jObject["nickname"];
            User_FirstName = (string)jObject["first_name"];
            User_LastName = (string)jObject["last_name"];
            User_Url = (string)jObject["url"];
            User_Description = (string)jObject["description"];
        }
    }
}
