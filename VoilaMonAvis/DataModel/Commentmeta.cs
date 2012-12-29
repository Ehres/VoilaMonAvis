using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataModel
{
    public class Commentmeta
    {
        public int MetaID { get; set; }
        public Comments Comment { get; set; }
        public string Meta_Key { get; set; }
        public string Meta_Value { get; set; }
    }
}
