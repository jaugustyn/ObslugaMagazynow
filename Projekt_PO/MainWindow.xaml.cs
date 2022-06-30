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
using Projekt_PO.ViewModels;

namespace Projekt_PO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFaktury_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista faktur";
            DataContext = new FakturyViewModel();
        }

        private void btnPracownicy_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista pracowników";
            DataContext = new PracownicyViewModel();
        }

        private void btnHurtownie_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista hurtowni";
            DataContext = new HurtownieViewModel();
        }

        private void btnMagazyny_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista hurtowni";
            DataContext = new MagazynyViewModel();
        }

        private void btnPakiety_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista pakietów";
            DataContext = new PakietyViewModel();
        }

        private void btnSektory_Click(object sender, RoutedEventArgs e)
        {
            lblwindowname.Content = "Lista sektorów w magazynach";
            DataContext = new SektoryViewModel();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
