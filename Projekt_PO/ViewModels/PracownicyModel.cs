using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_PO.ViewModels
{
    public class PracownicyModel
    {
        public int Id { get; set; }
        public int AdresId { get; set; }
        public string Miejscowosc { get; set; }
        public string KodPocztowy { get; set; }
        public string Ulica { get; set; }
        public string NrLokalu { get; set; }
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
    }
}
