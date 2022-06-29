using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_PO.DB;

namespace Projekt_PO.ViewModels
{
    public class SektoryModel
    {
        public SektoryModel()
        {
            Pakieties = new HashSet<Pakiety>();
        }

        public int MagazynId { get; set; }
        public int SektorId { get; set; }

        public virtual Magazyny Magazyn { get; set; }
        public virtual Sektory Sektor { get; set; }
        public virtual ICollection<Pakiety> Pakieties { get; set; }

        public string Oznaczenie { get; set; }
        public string OpisSektoru { get; set; }
        public string OpisMagazynu { get; set; }
        public int Limit { get; set; }
        public string Adres { get; set; }
    }
}
