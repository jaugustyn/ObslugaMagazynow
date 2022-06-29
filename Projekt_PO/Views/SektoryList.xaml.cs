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
using Projekt_PO.Pages;
using Projekt_PO.ViewModels;

namespace Projekt_PO.Views
{
    /// <summary>
    /// Interaction logic for SektoryList.xaml
    /// </summary>
    public partial class SektoryList : UserControl
    {
        public SektoryList()
        {
            InitializeComponent();
        }
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        private List<SektoryModel> _list = new List<SektoryModel>();
        private List<Sektory> _sektoryList = new List<Sektory>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbMagazyn.ItemsSource = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;
            
            FillGrid();
        }

        private void FillGrid()
        {
            _sektoryList = db.Sektories.ToList();
            listSektory.ItemsSource = _sektoryList;
            cmbSektor.ItemsSource = _sektoryList;
            cmbSektor.DisplayMemberPath = "Oznaczenie";
            cmbSektor.SelectedValuePath = "Oznaczenie";
            cmbSektor.SelectedIndex = -1;

            using (var db = new Obsluga_magazynow_DBContext())
            {
                var list = db.SektoryMagazynows.Include(x => x.Sektor).Include(x => x.Magazyn).Select(x => new SektoryModel()
                {
                    MagazynId = x.MagazynId,
                    SektorId = x.SektorId,
                    Oznaczenie = x.Sektor.Oznaczenie,
                    OpisSektoru = x.Sektor.Opis,
                    OpisMagazynu = x.Magazyn.Opis,
                    Limit = x.Sektor.Limit,
                    Adres = x.Magazyn.Adres,
                }).ToList();

                gridSektory.ItemsSource = list;
                _list = list;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SektoryPage page = new SektoryPage();
            page.ShowDialog();
            FillGrid();
        }
        
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listSektory.SelectedItem is Sektory model && model.IdSektoru != 0)
            {
                SektoryPage page = new SektoryPage();
                page.model = model;
                page.ShowDialog();
                FillGrid();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Sektory model = (Sektory)listSektory.SelectedItem;
            if (model != null && model.IdSektoru != 0)
            {
                var s = db.SektoryMagazynows.Any(x => x.SektorId == model.IdSektoru);
                if (s)
                {
                    MessageBox.Show("Żeby usunąć sektor, nie może być on przypisany do jakiegokolwiek magazynu.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show($"Czy jesteś pewien że chcesz usunąć sektor {model.Oznaczenie.Trim()}?{Environment.NewLine}Sektor nie może być przypisany do jakiegokolwiek magazynu, nie mogą się również w nim znajdować pakiety.", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    db.Sektories.Remove(model);
                    db.SaveChanges();
                    MessageBox.Show("Sektor został usunięty");
                    FillGrid();
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<SektoryModel> searchList = _list;
            
            if (cmbMagazyn.SelectedIndex != -1)
                searchList = searchList.Where(x => x.MagazynId == Convert.ToInt32(cmbMagazyn.SelectedValue)).ToList();
            if (cmbSektor.SelectedIndex != -1)
                searchList = searchList.Where(x => x.Oznaczenie == cmbSektor.SelectedValue.ToString()).ToList();

            gridSektory.ItemsSource = searchList;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbMagazyn.SelectedIndex = -1;
            cmbSektor.ItemsSource = _sektoryList;
            cmbSektor.DisplayMemberPath = "Oznaczenie";
            cmbSektor.SelectedValuePath = "Oznaczenie";
            cmbSektor.SelectedIndex = -1;
            FillGrid();
        }

        private void cmbMagazyn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSektor.ItemsSource = db.SektoryMagazynows.Include(x => x.Sektor).Where(x => x.MagazynId == Convert.ToInt32(cmbMagazyn.SelectedValue)).ToList();
            cmbSektor.DisplayMemberPath = "Sektor.Oznaczenie";
            cmbSektor.SelectedValuePath = "Sektor.Oznaczenie";
            cmbSektor.SelectedIndex = -1;
        }
    }
}
