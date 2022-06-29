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
using Microsoft.EntityFrameworkCore;
using Projekt_PO.DB;
using Projekt_PO.ViewModels;

namespace Projekt_PO.Pages
{
    /// <summary>
    /// Interaction logic for PakietyPage.xaml
    /// </summary>
    public partial class PakietyPage : Window
    {
        public PakietyPage()
        {
            InitializeComponent();
        }
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        public PakietyModel model = null!;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbMagazyn.ItemsSource = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;

            if (model != null && model.IdPakietu != 0)
            {
                txtKod.Text = model.Kod;
                cmbMagazyn.SelectedValue = model.MagazynId;
                cmbSektor.SelectedValue = model.SektorId;
            }
        }
        private void ListSektory_Selection(object sender, SelectionChangedEventArgs e) // cmbMagazyny on change 
        {
            cmbSektor.ItemsSource = db.SektoryMagazynows.Include(x => x.Sektor).Where(x => x.MagazynId == Convert.ToInt32(cmbMagazyn.SelectedValue)).ToList();
            cmbSektor.DisplayMemberPath = "Sektor.Oznaczenie";
            cmbSektor.SelectedValuePath = "Sektor.IdSektoru";
            cmbSektor.SelectedIndex = -1;

            if (cmbMagazyn.SelectedIndex != -1)
            {
                cmbSektor.IsEnabled = true;
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtKod.Text.Trim() == "" || cmbMagazyn.SelectedIndex == -1 || cmbSektor.SelectedIndex == -1)
            {
                MessageBox.Show("Wszystkie pola są wymagane.");
            }
            else
            {
                if (model != null && model.IdPakietu != 0) //update
                {
                    var update = new Pakiety();
                    update.IdPakietu = model.IdPakietu;
                    update.Kod = txtKod.Text.Trim();
                    update.MagazynId = Convert.ToInt32(cmbMagazyn.SelectedValue);
                    update.SektorId = Convert.ToInt32(cmbSektor.SelectedValue);
                    db.Pakieties.Update(update);
                    db.SaveChanges();
                    this.Close();
                }
                else //add
                {
                    if (db.Pakieties.Select(x => x.Kod).Contains(txtKod.Text.Trim()))
                    {
                        MessageBox.Show("Pakiet o podanym kodzie już istnieje");
                        return;
                    }

                    // var p = new Pakiety();
                    // p.Kod = txtKod.Text.Trim();
                    // p.MagazynId = Convert.ToInt32(cmbMagazyn.SelectedValue);
                    // p.SektorId = Convert.ToInt32(cmbSektor.SelectedValue);
                    // var sekMag = db.SektoryMagazynows.Find(Convert.ToInt32(cmbMagazyn.SelectedValue), Convert.ToInt32(cmbSektor.SelectedValue));
                    // sekMag.Pakieties.Add(p);
                    // db.Pakieties.Add(p);
                    
                    // Jedyny działający sposób...
                    db.Database.ExecuteSqlRaw("INSERT INTO pakiety(kod, magazyn_id, sektor_id) VALUES({0}, {1}, {2} );", txtKod.Text.Trim(), Convert.ToInt32(cmbMagazyn.SelectedValue), Convert.ToInt32(cmbSektor.SelectedValue));
                    // db.Pakieties.Add(p);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
