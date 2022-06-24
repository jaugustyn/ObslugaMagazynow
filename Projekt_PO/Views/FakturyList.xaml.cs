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

            using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext())
            {
                List<Faktury> list = db.Fakturies.Include(x => x.Wystawiajacy).Include(x => x.Hurtownia).OrderBy(x => x.DataWystawienia).ToList();
                gridFaktury.ItemsSource = list;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FakturyPage page = new FakturyPage();
            // this.Visibility = Visibility.Collapsed;
            page.ShowDialog();

            using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext())
            {
                List<Faktury> list = db.Fakturies.Include(x => x.Wystawiajacy).Include(x => x.Hurtownia).OrderBy(x => x.DataWystawienia).ToList();
                gridFaktury.ItemsSource = list;

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Faktury faktura = (Faktury) gridFaktury.SelectedItem;
            FakturyPage page = new FakturyPage();
            page.Faktura = faktura;
            page.ShowDialog();
            using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext())
            {
                gridFaktury.ItemsSource = db.Fakturies.Include(x => x.Wystawiajacy).Include(x => x.Hurtownia).OrderBy(x => x.DataWystawienia).ToList();
            }

        }
    }
}
