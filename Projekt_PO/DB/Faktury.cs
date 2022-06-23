using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Faktury
    {
        public int IdFaktury { get; set; }
        public int WystawiajacyId { get; set; }
        public int HurtowniaId { get; set; }
        public string NumerFaktury { get; set; }
        public DateTime DataWystawienia { get; set; }
        public float Wartosc { get; set; }
        public string Opis { get; set; }

        public virtual Hurtownie Hurtownia { get; set; }
        public virtual Pracownicy Wystawiajacy { get; set; }
    }
}
