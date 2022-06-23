using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class WartośćFakturMiesiące
    {
        public int? Rok { get; set; }
        public int? Miesiąc { get; set; }
        public double? Suma { get; set; }
    }
}
