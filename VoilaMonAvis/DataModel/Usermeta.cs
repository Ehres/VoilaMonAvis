using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.DataModel
{
    public class Usermeta
    {
        public int Umeta_Id { get; set; }
        public Users User { get; set; }
        public string Meta_Key { get; set; }
        public string Meta_Value { get; set; }
    }
}
