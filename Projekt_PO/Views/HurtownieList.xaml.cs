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
    /// Interaction logic for HurtownieList.xaml
    /// </summary>
    public partial class HurtownieList : UserControl
    {
        public HurtownieList()
        {
            InitializeComponent();
        }
        private List<Hurtownie> list = new List<Hurtownie>();
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillGrid();
        }

        void FillGrid()
        {
            using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext())
            {
                list = db.Hurtownies.OrderBy(x => x.Nazwa).ToList();
                gridHurtownie.ItemsSource = list;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            HurtowniePage page = new HurtowniePage();
            page.ShowDialog();
            FillGrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Hurtownie model = (Hurtownie)gridHurtownie.SelectedItem;
            if (model != null && model.IdHurtowni != 0)
            {
                HurtowniePage page = new HurtowniePage();
                page.model = model;
                page.ShowDialog();
                FillGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Hurtownie model = (Hurtownie)gridHurtownie.SelectedItem;
            if (model != null && model.IdHurtowni != 0)
            {
                if (MessageBox.Show($"Czy napewno chcesz usunąć hurtownię {model.Nazwa}?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Hurtownie h = db.Hurtownies.Find(model.IdHurtowni);
                    db.Hurtownies.Remove(h);
                    db.SaveChanges();
                    MessageBox.Show("Hurtownia została usunięta");
                    FillGrid();
                }
            }
        }
    }
}
