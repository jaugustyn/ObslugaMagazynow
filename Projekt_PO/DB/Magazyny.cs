using System;
using System.Collections.Generic;

#nullable disable

namespace Projekt_PO.DB
{
    public partial class Magazyny
    {
        public Magazyny()
        {
            Pracownicies = new HashSet<Pracownicy>();
            SektoryMagazynows = new HashSet<SektoryMagazynow>();
        }

        public int IdMagazynu { get; set; }
        public string Adres { get; set; }
        public byte? IloscSektorow { get; set; }
        public byte? IloscPracownikow { get; set; }
        public bool CzyAktywny { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Pracownicy> Pracownicies { get; set; }
        public virtual ICollection<SektoryMagazynow> SektoryMagazynows { get; set; }
    }
}
