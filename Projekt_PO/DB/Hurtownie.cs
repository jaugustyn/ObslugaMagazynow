using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Hurtownie
    {
        public Hurtownie()
        {
            Fakturies = new HashSet<Faktury>();
        }

        public int IdHurtowni { get; set; }
        public string Nazwa { get; set; }
        public string Nip { get; set; }

        public virtual ICollection<Faktury> Fakturies { get; set; }
    }
}
