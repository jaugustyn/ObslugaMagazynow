using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Projekt_PO.DB;
using Projekt_PO.ViewModels;

namespace Projekt_PO.Views
{
    /// <summary>
    /// Interaction logic for PracownicyList.xaml
    /// </summary>
    public partial class PracownicyList : UserControl
    {
        public PracownicyList()
        {
            InitializeComponent();
        }

        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillGrid();
        }

        void FillGrid()
        {
            var list = db.Pracownicies.Include(x => x.Adres).Include(x => x.Magazyn).Select(a => new
            {
                a.IdPracownika,
                a.AdresId,
                Miejscowosc = a.Adres.Miejscowosc,
                a.MagazynId,
                a.Stanowisko,
                a.Imie,
                a.Nazwisko,
                a.NrTel,
                a.Email,
                a.Pesel,
                a.DataUrodzenia,
                a.Plec,
                a.DataZatrudnienia
            }).OrderBy(x => x.Nazwisko).ToList();

            List<PracownicyModel> modelList = new List<PracownicyModel>();
            foreach (var item in list)
            {
                PracownicyModel model = new PracownicyModel();
                model.Id = item.IdPracownika;
                model.AdresId = item.AdresId;
                model.Imie = item.Imie;
                model.Nazwisko = item.Nazwisko;
                model.MagazynId = item.MagazynId;
                model.DataUrodzenia = item.DataUrodzenia;
                model.DataZatrudnienia = item.DataZatrudnienia;
                model.Plec = item.Plec;
                model.NrTel = item.NrTel;
                model.Email = item.Email;
                model.Pesel = item.Pesel;
                model.Stanowisko = item.Stanowisko;
                model.Miejscowosc = item.Miejscowosc;
                modelList.Add(model);
            }
            gridPracownicy.ItemsSource = modelList;

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PracownicyPage page = new PracownicyPage();
            page.ShowDialog();
            FillGrid();
        }
    }
}
