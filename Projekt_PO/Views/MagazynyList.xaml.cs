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
using Projekt_PO.DB;
using Projekt_PO.Pages;

namespace Projekt_PO.Views
{
    /// <summary>
    /// Interaction logic for MagazynyList.xaml
    /// </summary>
    public partial class MagazynyList : UserControl
    {
        public MagazynyList()
        {
            InitializeComponent();
        }

        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        private List<Magazyny> list = new List<Magazyny>();

        private void FillGrid()
        {
            list = db.Magazynies.ToList();
            gridMagazyny.ItemsSource = list;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillGrid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MagazynyPage page = new MagazynyPage();
            page.ShowDialog();
            FillGrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (gridMagazyny.SelectedItem is Magazyny model && model.IdMagazynu != 0)
            {
                MagazynyPage page = new MagazynyPage();
                page.model = model;
                page.ShowDialog();
                FillGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridMagazyny.SelectedItem is Magazyny model && model.IdMagazynu != 0)
            {
                if (MessageBox.Show($"Czy jesteś pewien że chcesz usunąć magazyn {model.Adres}?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Magazyny m = db.Magazynies.Find(model.IdMagazynu);
                    db.Magazynies.Remove(m);
                    db.SaveChanges();
                    MessageBox.Show("Magazyn został usunięty.");
                    FillGrid();
                }
            }
        }
    }
}
