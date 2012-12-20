using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.Data
{
    public class Term_Relationships
    {
        public int Object_Id { get; set; }
        public Term_Taxonomy Term_Taxonomy { get; set; }
        public int Term_Order { get; set; }

    }
}
