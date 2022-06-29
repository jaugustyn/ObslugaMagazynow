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

            if (model != null && model.IdMagazynu != 0)
            {
                txtOpis.Text = model.Opis;
                txtAdres.Text = model.Adres;
                chkCzyAktwny.IsChecked = model.CzyAktywny;

                var wybraneSektory = db.SektoryMagazynows.Where(x => x.MagazynId == model.IdMagazynu).ToList();
                foreach (SektoryMagazynow obj in wybraneSektory)
                {
                    listSektory.SelectedItems.Add(obj.Sektor);
                }
            }
            
        }
		private void listSektory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listSektory.SelectedItem != null)
                this.Title = (listSektory.SelectedItem as Sektory)?.Oznaczenie;
        }

        private void btnShowSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            var result = new StringBuilder();
            foreach (object o in listSektory.Items)
                result.Append((o as Sektory)?.Oznaczenie + "-  " + (o as Sektory)?.Opis + Environment.NewLine);
            
            MessageBox.Show(result.ToString());
        }

        private void btnSelectLast_Click(object sender, RoutedEventArgs e)
        {
            listSektory.SelectedIndex = listSektory.Items.Count - 1;
        }

        private void btnSelectNext_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = 0;
            if (listSektory.SelectedIndex >= 0 && listSektory.SelectedIndex < listSektory.Items.Count - 1)
                nextIndex = listSektory.SelectedIndex + 1;
            listSektory.SelectedIndex = nextIndex;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            listSektory.SelectAll();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtOpis.Text.Trim() == "" || txtAdres.Text.Trim() == "")
            {
                MessageBox.Show("Wszystkie pola są wymagane.");
            }
            else if (listSektory.SelectedItems.Count == 0)
            {
                MessageBox.Show("Magazyn musi mieć co najmniej jeden sektor. Zaznacz z listy.");
            }
            else
            {
                if (model != null && model.IdMagazynu != 0) //update
                {
                    var update = new Magazyny();
                    update.IdMagazynu = model.IdMagazynu;
                    update.Adres = txtAdres.Text.Trim();
                    update.Opis = txtOpis.Text.Trim();
                    if (chkCzyAktwny.IsChecked != null) update.CzyAktywny = chkCzyAktwny.IsChecked.Value;
                    update.IloscSektorow = (byte)listSektory.SelectedItems.Count;
                    update.IloscPracownikow = model.IloscPracownikow;
                    
                    var sek = db.SektoryMagazynows.Where(x => x.MagazynId == model.IdMagazynu).Include(x => x.Pakieties).ToList();
                    foreach (SektoryMagazynow sektor in sek) // Wszystkie sektory przed zmianą
                    {
                        if (!listSektory.SelectedItems.Contains(sektor.Sektor)) // jeśli nie ma sektoru w wybranych to trzeba go usunąć
                        {
                            if (sektor.Pakieties.Count > 0)
                            {
                                MessageBox.Show($"Sektor {sektor.Sektor.Oznaczenie} w magazynie {model.Adres} nie jest pusty. Znajduje się w nim {sektor.Pakieties.Count} pakietów.");
                                this.Close();
                                return;
                            }
                            db.SektoryMagazynows.Remove(sektor);
                        }
                    } 
                    db.SaveChanges();
                    
                    db.Magazynies.Update(update);
                    
                    foreach (Sektory obj in listSektory.SelectedItems)
                    {
                        if (db.SektoryMagazynows.Find(model.IdMagazynu, obj.IdSektoru) == null)
                        {
                            var wybranySektor = new SektoryMagazynow() { MagazynId = model.IdMagazynu, SektorId = obj.IdSektoru };
                            db.SektoryMagazynows.Add(wybranySektor);
                        }
                    }
                    db.SaveChanges();

                    MessageBox.Show("Magazyn został zaaktualizowany.");
                    this.Close();
                }
                else //add
                {
                    var m = new Magazyny();
                    if (db.Magazynies.Select(x => x.Adres).Contains(txtAdres.Text.Trim()))
                    {
                        MessageBox.Show("Adres magazynu musi być unikatowy!");
                        return;
                    }
                    m.Adres = txtAdres.Text.Trim();
                    m.Opis = txtOpis.Text.Trim();
                    if (chkCzyAktwny.IsChecked != null) m.CzyAktywny = chkCzyAktwny.IsChecked.Value;
                    m.SektoryMagazynows = listSektory.SelectedItems.OfType<SektoryMagazynow>().ToList();
                    m.IloscSektorow = (byte)listSektory.SelectedItems.Count;
                    m.IloscPracownikow = 0;

                    db.Magazynies.Add(m);
                    db.SaveChanges();
                    
                    foreach (Sektory obj in listSektory.SelectedItems)
                    {
                        var wybranySektor = new SektoryMagazynow() { MagazynId = db.Magazynies.OrderBy(x => x.IdMagazynu).Last().IdMagazynu, SektorId = obj.IdSektoru};
                        db.SektoryMagazynows.Add(wybranySektor);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Magazyn został dodany.");
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
