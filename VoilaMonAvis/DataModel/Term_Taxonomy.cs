using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoilaMonAvis.Data
{
    public class Term_Taxonomy
    {
        public int Term_Taxonomy_Id { get; set; }
        public Terms Term { get; set; }
        public string Taxonomy { get; set; }
        public string Description { get; set; }
        public int Parent { get; set; }
        public int Count { get; set; }
    }
}
