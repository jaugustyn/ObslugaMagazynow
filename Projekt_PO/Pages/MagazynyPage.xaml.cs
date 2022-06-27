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
using System.Windows.Shapes;
using Projekt_PO.DB;
using Projekt_PO.ViewModels;

namespace Projekt_PO.Pages
{
    /// <summary>
    /// Interaction logic for MagazynyPage.xaml
    /// </summary>
    public partial class MagazynyPage : Window
    {
        public MagazynyPage()
        {
            InitializeComponent();
        }
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        public Magazyny model = null!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Sektory> sektory = new List<Sektory>();

            db.Sektories.ToList().ForEach(x => sektory.Add(x));

            listSektory.ItemsSource = sektory;
        }
		private void lbTodoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listSektory.SelectedItem != null)
                this.Title = (listSektory.SelectedItem as Sektory).Oznaczenie;
        }

        private void btnShowSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in listSektory.SelectedItems)
                MessageBox.Show((o as Sektory).Oznaczenie);
        }

        private void btnSelectLast_Click(object sender, RoutedEventArgs e)
        {
            listSektory.SelectedIndex = listSektory.Items.Count - 1;
        }

        private void btnSelectNext_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = 0;
            if ((listSektory.SelectedIndex >= 0) && (listSektory.SelectedIndex < (listSektory.Items.Count - 1)))
                nextIndex = listSektory.SelectedIndex + 1;
            listSektory.SelectedIndex = nextIndex;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in listSektory.Items)
                listSektory.SelectedItems.Add(o);
        }
    }
}
