using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_PO.DB;

namespace Projekt_PO.ViewModels
{
    public class PakietyModel
    {
        public int IdPakietu { get; set; }
        public int MagazynId { get; set; }
        public int SektorId { get; set; }
        public string Kod { get; set; }
        public virtual SektoryMagazynow SektoryMagazynow { get; set; }
        
        public string AdresMagazynu { get; set; }
        public string OznaczenieSektoru { get; set; }
        public int LimitSektoru { get; set; }

    }
}
