using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Pracownicy
    {
        public Pracownicy()
        {
            Fakturies = new HashSet<Faktury>();
        }

        public int IdPracownika { get; set; }
        public int AdresId { get; set; }
        public int MagazynId { get; set; }
        public string Stanowisko { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string NrTel { get; set; }
        public string Email { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string Pesel { get; set; }
        public string Plec { get; set; }
        public DateTime DataZatrudnienia { get; set; }

        public virtual Adresy Adres { get; set; }
        public virtual Magazyny Magazyn { get; set; }
        public virtual ICollection<Faktury> Fakturies { get; set; }
    }
}
