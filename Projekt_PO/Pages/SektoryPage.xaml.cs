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
    /// Interaction logic for SektoryPage.xaml
    /// </summary>
    public partial class SektoryPage : Window
    {
        public SektoryPage()
        {
            InitializeComponent();
        }
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        public Sektory model = null!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (model != null && model.IdSektoru != 0)
            {
                txtOznaczenie.Text = model.Oznaczenie;
                txtOpis.Text = model.Opis;
                txtLimit.Text = model.Limit.ToString();
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtOznaczenie.Text.Trim() == "" || txtLimit.Text.Trim() == "" || txtLimit.Text.Trim() == "")
            {
                MessageBox.Show("Wszystkie pola są wymagane.");
            }
            else if (int.TryParse(txtLimit.Text.Trim(), out var limit) == false)
            {
                MessageBox.Show("Limit musi być liczbą.");
            }
            else
            {
                if (limit is <= 0 or > 250)
                {
                    MessageBox.Show("Limit miejsca w sektorze nie może wynosić mniej niż 1 ani więcej niż 250.");
                    return;
                }
                
                if (model != null && model.IdSektoru != 0) //update
                {
                    var update = new Sektory();
                    update.IdSektoru = model.IdSektoru;
                    update.Oznaczenie = txtOznaczenie.Text.Trim();
                    update.Opis = txtOpis.Text.Trim();
                    update.Limit = Convert.ToByte(txtLimit.Text.Trim());
                    db.Sektories.Update(update);
                    db.SaveChanges();
                    this.Close();
                }
                else //add
                {
                    if (db.Sektories.Select(x => x.Oznaczenie).Contains(txtOznaczenie.Text.Trim()))
                    {
                        MessageBox.Show("Sektor o podanym oznaczeniu już istnieje.");
                        return;
                    }

                    var s = new Sektory();
                    s.Oznaczenie = txtOznaczenie.Text.Trim();
                    s.Opis = txtOpis.Text.Trim();
                    s.Limit = Convert.ToByte(txtLimit.Text.Trim());
                    
                    db.Sektories.Add(s);
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
