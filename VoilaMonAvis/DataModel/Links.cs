using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataModel
{
    public class Links
    {
        public int Link_ID { get; set; }
        public string Link_Url { get; set; }
        public string Link_Name { get; set; }
        public string Link_Image { get; set; }
        public string Link_Target { get; set; }
        public string Link_Description { get; set; }
        public string Link_Visible { get; set; }
        public int Link_Owner { get; set; }
        public int Link_Rating { get; set; }
        public DateTime Link_Uptated { get; set; }
        public string Link_Rel { get; set; }
        public string Link_Notes { get; set; }
        public string Link_RSS { get; set; }
    }
}
