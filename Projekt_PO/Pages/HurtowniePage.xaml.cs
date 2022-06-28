using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Projekt_PO.DB;

namespace Projekt_PO
{
    /// <summary>
    /// Interaction logic for HurtowniePage.xaml
    /// </summary>
    public partial class HurtowniePage : Window
    {
        public HurtowniePage()
        {
            InitializeComponent();
        }
        private readonly Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        public Hurtownie model = null!;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (model != null && model.IdHurtowni != 0)
            {
                txtNazwa.Text = model.Nazwa.Trim();
                txtNip.Text = model.Nip.Trim();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNazwa.Text.Trim() == "" || txtNip.Text.Trim() == "")
            {
                MessageBox.Show("Wszystkie pola są wymagane!");
            }
            else if (txtNip.Text.Trim().Length < 10)
            {
                MessageBox.Show("Numer NIP powinien mieć 10 cyfr!");
            }
            else
            {
                if (model != null && model.IdHurtowni != 0)
                {
                    var update = new Hurtownie();
                    update.IdHurtowni = model.IdHurtowni;
                    update.Nazwa = txtNazwa.Text.Trim();
                    update.Nip = txtNip.Text.Trim();
                    db.Hurtownies.Update(update);
                    db.SaveChanges();
                    MessageBox.Show("Hurtownia została zaaktualizowana.");
                    this.Close();
                }
                else
                {
                    var h = new Hurtownie();

                    h.Nazwa = txtNazwa.Text.Trim();
                    h.Nip = txtNip.Text.Trim();

                    db.Hurtownies.Add(h);
                    MessageBox.Show(h.IdHurtowni.ToString());
                    db.SaveChanges();
                    MessageBox.Show("Hurtownia została dodana.");
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
