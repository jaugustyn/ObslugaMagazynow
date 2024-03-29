﻿using System;
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
using Projekt_PO.Pages;
using Projekt_PO.ViewModels;

namespace Projekt_PO.Views
{
    /// <summary>
    /// Interaction logic for PakietyList.xaml
    /// </summary>
    public partial class PakietyList : UserControl
    {
        public PakietyList()
        {
            InitializeComponent();
        }
        private Obsluga_magazynow_DBContext db = new Obsluga_magazynow_DBContext();
        private List<PakietyModel> _list = new List<PakietyModel>();
        private List<Sektory> _sekList = new List<Sektory>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbMagazyn.ItemsSource = db.Magazynies.Where(x => x.CzyAktywny == true).ToList();
            cmbMagazyn.DisplayMemberPath = "Adres";
            cmbMagazyn.SelectedValuePath = "IdMagazynu";
            cmbMagazyn.SelectedIndex = -1;

            _sekList = db.Sektories.ToList();
            cmbSektor.ItemsSource = _sekList;
            cmbSektor.DisplayMemberPath = "Oznaczenie";
            cmbSektor.SelectedValuePath = "Oznaczenie";
            cmbSektor.SelectedIndex = -1;
            
            FillGrid();
        }
        private void FillGrid()
        {
            using (var db = new Obsluga_magazynow_DBContext())
            {
                var list = db.Pakieties.Include(x => x.SektoryMagazynow).Select(x => new PakietyModel()
                {
                    IdPakietu = x.IdPakietu,
                    Kod = x.Kod,
                    SektorId = x.SektorId,
                    MagazynId = x.MagazynId,
                    OznaczenieSektoru = x.SektoryMagazynow.Sektor.Oznaczenie,
                    AdresMagazynu = x.SektoryMagazynow.Magazyn.Adres,
                    LimitSektoru = x.SektoryMagazynow.Sektor.Limit,
                }).ToList();

                gridPakiety.ItemsSource = list;
                _list = list;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PakietyPage page = new PakietyPage();
            page.ShowDialog();
            FillGrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (gridPakiety.SelectedItem is PakietyModel model && model.IdPakietu != 0)
            {
                PakietyPage page = new PakietyPage();
                page.model = model;
                page.ShowDialog();
                FillGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridPakiety.SelectedItem is PakietyModel model && model.IdPakietu != 0)
            {
                if (MessageBox.Show($"Czy jesteś pewien że chcesz usunąć ten pakiet o kodzie {model.Kod}?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Pakiety p = db.Pakieties.Find(model.IdPakietu);
                    
                    db.Pakieties.Remove(p);
                    db.SaveChanges();
                    MessageBox.Show("Pakiet został usunięty.");
                    FillGrid();
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<PakietyModel> searchList = _list;
            if (txtKod.Text.Trim() != "")
                searchList = searchList.Where(x => x.Kod.ToUpper().Contains(txtKod.Text.Trim().ToUpper())).ToList();
            if (cmbMagazyn.SelectedIndex != -1)
                searchList = searchList.Where(x => x.MagazynId == Convert.ToInt32(cmbMagazyn.SelectedValue)).ToList();
            if (cmbSektor.SelectedIndex != -1)
                searchList = searchList.Where(x => x.OznaczenieSektoru == cmbSektor.SelectedValue.ToString()).ToList();

            gridPakiety.ItemsSource = searchList;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtKod.Clear();
            cmbMagazyn.SelectedIndex = -1;
            cmbSektor.ItemsSource = _sekList;
            cmbSektor.DisplayMemberPath = "Oznaczenie";
            cmbSektor.SelectedValuePath = "Oznaczenie";
            cmbSektor.SelectedIndex = -1;
            FillGrid();
        }

        private void ListSektory_Selection(object sender, SelectionChangedEventArgs e) // cmbMagazyny on change
        {
            cmbSektor.ItemsSource = db.SektoryMagazynows.Include(x => x.Sektor).Where(x => x.MagazynId == Convert.ToInt32(cmbMagazyn.SelectedValue)).ToList();
            cmbSektor.DisplayMemberPath = "Sektor.Oznaczenie";
            cmbSektor.SelectedValuePath = "Sektor.Oznaczenie";
            cmbSektor.SelectedIndex = -1;
        }
    }
}
