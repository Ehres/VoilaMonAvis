using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoilaMonAvis.DataModel;

namespace VoilaMonAvis.DataModel
{
    public class Postmeta
    {
        public int Meta_Id { get; set; }
        public Posts Post { get; set; }
        public string Meta_Key { get; set; }
        public string Meta_Value { get; set; }
    }
}
