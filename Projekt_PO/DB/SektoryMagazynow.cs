using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class SektoryMagazynow
    {
        public SektoryMagazynow()
        {
            Pakieties = new HashSet<Pakiety>();
        }

        public int MagazynId { get; set; }
        public int SektorId { get; set; }

        public virtual Magazyny Magazyn { get; set; }
        public virtual Sektory Sektor { get; set; }
        public virtual ICollection<Pakiety> Pakieties { get; set; }
    }
}
