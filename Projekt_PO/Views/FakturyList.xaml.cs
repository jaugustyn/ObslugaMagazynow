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

namespace Projekt_PO.Views
{
    /// <summary>
    /// Interaction logic for FakturyList.xaml
    /// </summary>
    public partial class FakturyList : UserControl
    {
        public FakturyList()
        {
            InitializeComponent();
            {
                FillGrid();
            }
        }
        Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();

        private void FillGrid()
        {
            using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext()) // bez tego gridFaktury nie chce się aktualizować po zamknięciu okna hmm
            {
                var list = db.Fakturies.Include(x => x.Wystawiajacy).Include(x => x.Hurtownia).OrderBy(x => x.DataWystawienia).ToList();
                gridFaktury.ItemsSource = list;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FakturyPage page = new FakturyPage();
            page.ShowDialog();
            FillGrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Faktury faktura = (Faktury) gridFaktury.SelectedItem;
            if (faktura != null && faktura.IdFaktury != 0)
            {
                FakturyPage page = new FakturyPage();
                page.model = faktura;
                page.ShowDialog();
                FillGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Faktury model = (Faktury)gridFaktury.SelectedItem;
            if (model != null && model.IdFaktury != 0)
            {
                if (MessageBox.Show($"Czy jesteś pewien że chcesz usunąć fakturę o nr. {model.NumerFaktury}?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Faktury f = db.Fakturies.Find(model.IdFaktury);
                    db.Fakturies.Remove(f);
                    db.SaveChanges();
                    MessageBox.Show("Faktura została usunięta");
                    FillGrid();
                }
            }
        }
    }
}
