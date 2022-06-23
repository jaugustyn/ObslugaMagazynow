using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Pakiety
    {
        public int IdPakietu { get; set; }
        public int MagazynId { get; set; }
        public int SektorId { get; set; }
        public string Kod { get; set; }

        public virtual SektoryMagazynow SektoryMagazynow { get; set; }
    }
}
