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
using Projekt_PO.ViewModels;

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
        public PracownicyModel model = null!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // var stanowiskoList = db.Pracownicies.Select(x => x.Stanowisko).Distinct().ToList(); // Zależy od pracowników, przydałaby się tabela osobna dla listy stanowisk
            // cmbStanowisko.ItemsSource = stanowiskoList;
            
            var magazynList = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            
            cmbPlec.ItemsSource = new string[] {"Mężczyzna", "Kobieta"};
            cmbPlec.SelectedIndex = -1;

            cmbStanowisko.ItemsSource = new string[] { "Logistyk", "Magazynier", "Praktykant", "Informatyk", "Kierownik ds. Logistyki i Dystrybucji" };
            cmbStanowisko.SelectedIndex = -1;

            cmbMagazyn.ItemsSource = magazynList;
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;

            if (model != null && model.Id != 0)
            {
                txtMiejscowosc.Text = model.Miejscowosc;
                txtKodPocztowy.Text = model.KodPocztowy;
                txtUlica.Text = model.Ulica;
                txtNrDomMiesz.Text = model.NrLokalu;
                txtEmail.Text = model.Email;
                txtImie.Text = model.Imie;
                txtNazwisko.Text = model.Nazwisko;
                txtNrTel.Text = model.NrTel;
                txtPesel.Text = model.Pesel;
                dpDataUrodzenia.SelectedDate = model.DataUrodzenia;
                dpDataZatrudnienia.SelectedDate = model.DataZatrudnienia;
                
                cmbStanowisko.SelectedValue = model.Stanowisko;
                cmbMagazyn.SelectedValue = model.MagazynId;
                cmbPlec.SelectedValue = model.Plec == "m" ? "Mężczyzna" : "Kobieta";
                radioMieszkanie.IsChecked = model.NrLokalu.Contains("/");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlec.SelectedIndex == -1 || cmbStanowisko.SelectedIndex == -1 || cmbMagazyn.SelectedIndex == -1 || txtEmail.Text.Trim() == "" ||
                txtImie.Text.Trim() == "" || txtNazwisko.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtNrTel.Text.Trim() == "" || txtPesel.Text.Trim() == ""
                || txtMiejscowosc.Text.Trim() == "" || txtUlica.Text.Trim() == "" || txtKodPocztowy.Text.Trim() == "" || txtNrDomMiesz.Text.Trim() == "" || dpDataUrodzenia.SelectedDate == null)
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }
            else
            {
                if (model != null && model.Id != 0)
                {
                    var update = new Pracownicy();
                    update.IdPracownika = model.Id;
                    update.Imie = txtImie.Text.Trim();
                    update.Nazwisko = txtNazwisko.Text.Trim();
                    update.DataUrodzenia = dpDataUrodzenia.SelectedDate.Value;
                    update.DataZatrudnienia = dpDataZatrudnienia.SelectedDate ?? DateTime.Now;
                    update.Email = txtEmail.Text.Trim();
                    update.Plec = cmbPlec.SelectedValue.ToString()?[0].ToString().ToLower();
                    update.Stanowisko = cmbStanowisko.SelectedValue.ToString();
                    update.NrTel = txtNrTel.Text.Length < 10 ? txtNrTel.Text[0..3] + "-" + txtNrTel.Text[3..6] + "-" + txtNrTel.Text[6..9] : txtNrTel.Text.Trim();
                    update.Pesel = txtPesel.Text;
                    update.MagazynId = Convert.ToInt32(cmbMagazyn.SelectedValue);

                    if (txtMiejscowosc.Text.Trim() != model.Miejscowosc || txtUlica.Text.Trim() != model.Ulica || txtKodPocztowy.Text.Trim() != model.KodPocztowy || txtNrDomMiesz.Text.Trim() != model.NrLokalu)
                    {
                        Adresy a = new Adresy();
                        a.Miejscowosc = txtMiejscowosc.Text.Trim();
                        a.Ulica = txtUlica.Text.Trim();

                        if (radioDom.IsChecked == true)
                            a.NrDomu = txtNrDomMiesz.Text.Trim();
                        else
                            a.NrMieszkania = txtNrDomMiesz.Text.Trim();

                        a.KodPocztowy = txtKodPocztowy.Text.Trim();
                        db.Adresies.Add(a);
                        db.SaveChanges();
                        
                        update.AdresId = db.Adresies.OrderBy(x => x.IdAdresu).Last().IdAdresu;
                    }

                    db.Pracownicies.Update(update);
                    db.SaveChanges();
                    MessageBox.Show("Pracownik został zaaktualizowany.");
                    this.Close();
                }
                else
                {
                    Adresy a = new Adresy();
                    a.Miejscowosc = txtMiejscowosc.Text;
                    a.Ulica = txtUlica.Text;

                    if (radioDom.IsChecked == true)
                        a.NrDomu = txtNrDomMiesz.Text;
                    else
                        a.NrMieszkania = txtNrDomMiesz.Text;

                    a.KodPocztowy = txtKodPocztowy.Text;
                    db.Adresies.Add(a);
                    db.SaveChanges();

                    Pracownicy p = new Pracownicy();
                    p.Imie = txtImie.Text;
                    p.Nazwisko = txtNazwisko.Text;
                    p.DataUrodzenia = dpDataUrodzenia.SelectedDate.Value;
                    p.DataZatrudnienia = dpDataZatrudnienia.SelectedDate ?? DateTime.Now;
                    p.Email = txtEmail.Text;
                    p.Plec = cmbPlec.SelectedValue.ToString()?[0].ToString().ToLower();
                    p.Stanowisko = cmbStanowisko.SelectedValue.ToString();
                    p.NrTel = txtNrTel.Text.Length < 10 ? txtNrTel.Text[0..3] + "-" + txtNrTel.Text[3..6] + "-" + txtNrTel.Text[6..9] : txtNrTel.Text.Trim();
                    p.Pesel = txtPesel.Text;
                    p.AdresId = db.Adresies.OrderBy(x => x.IdAdresu).Last().IdAdresu;
                    p.MagazynId = Convert.ToInt32(cmbMagazyn.SelectedValue);

                    db.Pracownicies.Add(p);
                    db.SaveChanges();
                    cmbStanowisko.SelectedIndex = -1;
                    cmbPlec.SelectedIndex = -1;
                    cmbMagazyn.SelectedIndex = -1;
                    MessageBox.Show("Pracownik został dodany.");
                    this.Close();
                }
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
