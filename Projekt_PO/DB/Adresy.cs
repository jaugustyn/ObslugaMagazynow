using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Adresy
    {
        public int IdAdresu { get; set; }
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        public string NrDomu { get; set; }
        public string NrMieszkania { get; set; }
        public string KodPocztowy { get; set; }

        public virtual Pracownicy Pracownicy { get; set; }
    }
}
