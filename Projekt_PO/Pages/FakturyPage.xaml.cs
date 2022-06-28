using System;
using System.Collections.Generic;
using System.Globalization;
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
using Projekt_PO.DB;

namespace Projekt_PO
{
    /// <summary>
    /// Interaction logic for FakturyPage.xaml
    /// </summary>
    public partial class FakturyPage : Window
    {
        public FakturyPage()
        {
            InitializeComponent();
        }
        Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        public Faktury model = null!;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbHurtownia.ItemsSource = db.Hurtownies.ToList();
            cmbHurtownia.DisplayMemberPath = "Nazwa";
            cmbHurtownia.SelectedValuePath = "IdHurtowni";
            cmbHurtownia.SelectedIndex = -1;

            cmbWystawiajacy.ItemsSource = db.Pracownicies.ToList();
            cmbWystawiajacy.DisplayMemberPath = "Nazwisko";
            cmbWystawiajacy.SelectedValuePath = "IdPracownika";
            cmbWystawiajacy.SelectedIndex = -1;

            if (model != null && model.IdFaktury != 0)
            {
                cmbHurtownia.SelectedValue = model.HurtowniaId;
                cmbWystawiajacy.SelectedValue = model.WystawiajacyId;
                txtNumerFaktury.Text = model.NumerFaktury;
                txtWartosc.Text = model.Wartosc.ToString(CultureInfo.InvariantCulture);
                txtOpis.Text = model.Opis;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumerFaktury.Text.Trim() == "" || cmbHurtownia.SelectedValue == null || cmbWystawiajacy.SelectedValue == null){
                MessageBox.Show("Pola oznaczone gwiazdką są wymagane!");
            }else if (txtWartosc.Text.Trim() == "" || !int.TryParse(txtWartosc.Text.Trim(), out _) ||
                      int.Parse(txtWartosc.Text.Trim()) <= 0 || int.Parse(txtWartosc.Text.Trim()) > 1000000000)
            {
                MessageBox.Show("Wartość musi być liczbą dodatnią, nie większą niż 1 miliard");
            }
            else if (model != null && model.IdFaktury != 0)
            {
                Faktury update = new Faktury();
                update.IdFaktury = model.IdFaktury;
                update.NumerFaktury = txtNumerFaktury.Text.Trim();
                update.HurtowniaId = Convert.ToInt32(cmbHurtownia.SelectedValue);
                update.WystawiajacyId = Convert.ToInt32(cmbWystawiajacy.SelectedValue);
                update.Wartosc = int.Parse(txtWartosc.Text.Trim());
                update.Opis = txtOpis.Text.Trim();
                update.DataWystawienia = DateTime.Now;
                db.Fakturies.Update(update);
                db.SaveChanges();
                MessageBox.Show($"Zaaktualizowano fakturę o numerze: {model.NumerFaktury}");
                this.Close();

            }
            else //ADD
            {
                Faktury newfaktura = new Faktury();
                newfaktura.NumerFaktury = txtNumerFaktury.Text.Trim();
                if (db.Fakturies.Select(x => x.NumerFaktury).Contains(newfaktura.NumerFaktury))
                {
                    MessageBox.Show("Numer faktury musi być unikatowy!");
                    return;
                }
                newfaktura.HurtowniaId = Convert.ToInt32(cmbHurtownia.SelectedValue); // if checks if its empty
                newfaktura.WystawiajacyId = Convert.ToInt32(cmbWystawiajacy.SelectedValue);
                newfaktura.Wartosc = int.Parse(txtWartosc.Text.Trim());
                newfaktura.Opis = txtOpis.Text.Trim();
                newfaktura.DataWystawienia = DateTime.Now;

                db.Fakturies.Add(newfaktura);
                db.SaveChanges();
                MessageBox.Show("Faktura została dodana.");
                this.Close();
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
