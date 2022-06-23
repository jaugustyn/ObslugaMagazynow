using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Sektory
    {
        public Sektory()
        {
            SektoryMagazynows = new HashSet<SektoryMagazynow>();
        }

        public int IdSektoru { get; set; }
        public string Oznaczenie { get; set; }
        public string Opis { get; set; }
        public byte Limit { get; set; }

        public virtual ICollection<SektoryMagazynow> SektoryMagazynows { get; set; }
    }
}
