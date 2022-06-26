using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Projekt_PO.DB;

namespace Projekt_PO
{
    /// <summary>
    /// Interaction logic for PracownicyPage.xaml
    /// </summary>
    public partial class PracownicyPage : Window
    {
        public PracownicyPage()
        {
            InitializeComponent();
        }

        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = db.Pracownicies.Include(x => x.Adres).ToList().OrderBy(x => x.Nazwisko);
            var magazynList = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            var adresList = db.Adresies.ToList();
            
            cmbPlec.ItemsSource = new string[] {"Mężczyzna", "Kobieta"};
            cmbPlec.SelectedValue = new string[] { "m", "k" };
            cmbPlec.SelectedIndex = -1;

            cmbStanowisko.ItemsSource = list;
            cmbStanowisko.DisplayMemberPath = "Stanowisko";
            cmbStanowisko.SelectedValuePath = "Stanowisko";
            cmbStanowisko.SelectedIndex = -1;

            cmbAdres.ItemsSource = adresList;
            cmbAdres.SelectedValuePath = "IdAdresu";
            cmbAdres.SelectedIndex = -1;

            cmbMagazyn.ItemsSource = magazynList;
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlec.SelectedIndex == -1 || cmbStanowisko.SelectedIndex == -1 || cmbAdres.SelectedIndex == -1 || cmbMagazyn.SelectedIndex == -1 || txtEmail.Text.Trim() == "" ||
                txtImie.Text.Trim() == "" || txtNazwisko.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtNrTel.Text.Trim() == "" || txtPesel.Text.Trim() == "" || dpDataUrodzenia.SelectedDate == null || dpDataZatrudnienia.SelectedDate == null)
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }
            else
            {
                Pracownicy p = new Pracownicy();
                p.Imie = txtImie.Text;
                p.Nazwisko = txtNazwisko.Text;
                p.DataUrodzenia = dpDataUrodzenia.SelectedDate.Value;
                p.DataZatrudnienia = dpDataZatrudnienia.SelectedDate.Value;
                p.Email = txtEmail.Text;
                p.Plec = cmbPlec.SelectedValue.ToString();
                p.Stanowisko = cmbStanowisko.SelectedValue.ToString();
                p.NrTel = txtNrTel.Text;
                p.Pesel = txtPesel.Text;
                p.AdresId = Convert.ToInt32(cmbAdres.SelectedValue);
                p.MagazynId = Convert.ToInt32(cmbMagazyn.SelectedValue);

                db.Pracownicies.Add(p);
                db.SaveChanges();
                cmbStanowisko.SelectedIndex = -1;
                cmbPlec.SelectedIndex = -1;
                cmbAdres.SelectedIndex = -1;
                cmbMagazyn.SelectedIndex = -1;
                MessageBox.Show("Pracownik został dodany.");
            }
                

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
