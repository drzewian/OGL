using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class OgloszenieKategoria
    {
        public OgloszenieKategoria()
        { }

        public int ID { get; set; }
        public int KategoriaID { get; set; }
        public int OgloszenieID { get; set; }

        public virtual Kategoria Kategoria { get; set; }
        public virtual Ogloszenie Ogloszenie { get; set; }
    }
}