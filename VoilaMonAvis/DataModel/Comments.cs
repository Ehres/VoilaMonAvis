using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.Data
{
    public class Comments
    {
        public int Comment_ID { get; set; }
        public Posts Comment_Post { get; set; }
        public string Comment_Author { get; set; }
        public string Comment_Author_Email { get; set; }
        public string Comment_Author_Url { get; set; }
        public string Comment_Author_IP { get; set; }
        public DateTime Comment_Date { get; set; }
        public DateTime Comment_Date_GMT { get; set; }
        public string Comment_Content { get; set; }
        public int Comment_Karma { get; set; }
        public string Comment_Approved { get; set; }
        public string Comment_Agent { get; set; }
        public string Comment_Type { get; set; }
        public Comments Comment_Parent { get; set; }
        public Users User { get; set; }
    }
}
