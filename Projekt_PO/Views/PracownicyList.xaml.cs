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
        private List<PracownicyModel> list = new List<PracownicyModel>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var magazynList = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            // var stanowiskoList = db.Pracownicies.Select(x => x.Stanowisko).Distinct().ToList(); // Zależy od pracowników, przydałaby się tabela osobna dla listy stanowisk

            cmbStanowisko.ItemsSource = new string[] { "Logistyk", "Magazynier", "Praktykant", "Informatyk", "Kierownik ds. Logistyki i Dystrybucji" };
            // cmbStanowisko.ItemsSource = stanowiskoList;
            cmbStanowisko.SelectedIndex = -1;

            cmbMagazyn.ItemsSource = magazynList;
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;

            list = db.Pracownicies.Include(x => x.Magazyn).Include(x => x.Adres).Select(x => new PracownicyModel()
            {
                Id = x.IdPracownika,
                AdresId = x.AdresId,
                Miejscowosc = x.Adres.Miejscowosc,
                KodPocztowy = x.Adres.KodPocztowy,
                NrLokalu = x.Adres.NrDomu != null ? x.Adres.NrDomu : x.Adres.NrMieszkania,
                Ulica = x.Adres.Ulica,
                MagazynId = x.MagazynId,
                Stanowisko = x.Stanowisko,
                Imie = x.Imie,
                Nazwisko = x.Nazwisko,
                NrTel = x.NrTel,
                Email = x.Email,
                Pesel = x.Pesel,
                DataUrodzenia = x.DataUrodzenia,
                Plec = x.Plec,
                DataZatrudnienia = x.DataZatrudnienia
            }).OrderBy(x => x.Nazwisko).ToList();
            
            FillGrid();
        }

        void FillGrid()
        {
            var list = db.Pracownicies.Include(x => x.Adres).Include(x => x.Magazyn).Select(x => new
            {
                x.IdPracownika,
                x.AdresId,
                Miejscowosc = x.Adres.Miejscowosc,
                KodPocztowy = x.Adres.KodPocztowy,
                NrLokalu = x.Adres.NrDomu != null ? x.Adres.NrDomu : x.Adres.NrMieszkania,
                Ulica = x.Adres.Ulica,
                x.MagazynId,
                x.Stanowisko,
                x.Imie,
                x.Nazwisko,
                x.NrTel,
                x.Email,
                x.Pesel,
                x.DataUrodzenia,
                x.Plec,
                x.DataZatrudnienia
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
                model.Ulica = item.Ulica;
                model.KodPocztowy = item.KodPocztowy;
                model.NrLokalu = item.NrLokalu;
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (gridPracownicy.SelectedItem is PracownicyModel model && model.Id != 0)
            {
                PracownicyPage page = new PracownicyPage();
                page.model = model;
                page.ShowDialog();
                FillGrid();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<PracownicyModel> searchList = list;
            if (txtImie.Text.Trim() != "")
                searchList = searchList.Where(x => x.Imie.Contains(txtImie.Text)).ToList();
            if (txtNazwisko.Text.Trim() != "")
                searchList = searchList.Where(x => x.Nazwisko.Contains(txtNazwisko.Text)).ToList();
            if (cmbMagazyn.SelectedIndex != -1)
                searchList = searchList.Where(x => x.MagazynId == (int)cmbMagazyn.SelectedValue).ToList();
            if (cmbStanowisko.SelectedIndex != -1)
                searchList = searchList.Where(x => x.Stanowisko ==cmbStanowisko.SelectedItem.ToString()).ToList();

            gridPracownicy.ItemsSource = searchList;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtImie.Clear();
            txtNazwisko.Clear();
            cmbMagazyn.SelectedIndex = -1;
            cmbStanowisko.SelectedIndex = -1;
            FillGrid();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            PracownicyModel model = (PracownicyModel)gridPracownicy.SelectedItem;
            if (model != null && model.Id != 0)
            {
                if (MessageBox.Show($"Czy jesteś pewien że chcesz usunąć pracownika {model.Imie} {model.Nazwisko}?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Pracownicy f = db.Pracownicies.Find(model.Id);
                    db.Pracownicies.Remove(f);
                    db.SaveChanges();
                    MessageBox.Show("Pracownik został usunięty");
                    FillGrid();
                }
            }
        }
    }
}
