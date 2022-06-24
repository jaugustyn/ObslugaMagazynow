using System;
using System.Collections.Generic;
using System.Globalization;
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

        private void btnClose_Click(object sender, RoutedEventArgs e) => this.Close();

        public Faktury Faktura;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumerFaktury.Text.Trim() == "" || cmbHurtownia.SelectedValue == null || cmbWystawiajacy.SelectedValue == null){
                MessageBox.Show("Pola oznaczone gwiazdką są wymagane!");
            }else if (txtWartosc.Text.Trim() == "" || !int.TryParse(txtWartosc.Text.Trim(), out _) ||
                      int.Parse(txtWartosc.Text.Trim()) <= 0 || int.Parse(txtWartosc.Text.Trim()) > 1000000000)
            {
                MessageBox.Show("Wartość musi być liczbą dodatnią, nie większą niż 1 miliard");
            }
            else
                using (Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext())
                {
                    if (Faktura != null && Faktura.IdFaktury != 0) //Update // https://www.csharp-console-examples.com/wpf/wpf-entity-framework-select-insert-update-delete/ trza sprawdzić
                    {
                        Faktury update = new Faktury();
                        update.IdFaktury = Faktura.IdFaktury;
                        update.NumerFaktury = Faktura.NumerFaktury;
                        update.HurtowniaId = Faktura.HurtowniaId;
                        update.WystawiajacyId = Faktura.WystawiajacyId;
                        update.Wartosc = Faktura.Wartosc;
                        update.Opis = Faktura.Opis;
                        update.DataWystawienia = DateTime.Now;
                        db.Fakturies.Update(update);
                        db.SaveChanges();
                        MessageBox.Show($"Zaaktualizowano fakturę o numerze: {Faktura.NumerFaktury}");

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
                    }
                }
        }

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

            if (Faktura != null && Faktura.IdFaktury != 0)
            {
                cmbHurtownia.SelectedValue = Faktura.HurtowniaId;
                cmbWystawiajacy.SelectedValue = Faktura.WystawiajacyId;
                txtNumerFaktury.Text = Faktura.NumerFaktury;
                txtWartosc.Text = Faktura.Wartosc.ToString(CultureInfo.InvariantCulture);
                txtOpis.Text = Faktura.Opis;
            }
        }
    }
}
