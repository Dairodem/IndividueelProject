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

namespace IndividueelProject
{
    /// <summary>
    /// Interaction logic for WindowChange.xaml
    /// </summary>
    public partial class WindowChange : Window
    {
        public int thisId = 0;
        public string Selector;
        public bool toChange;
        public WindowChange()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string str = toChange ? "bewerken" : "verwijderen";

            switch (Selector)
            {
                case "Klant":
                    txtChange.Text = $"Welke klant wilt u {str}?";
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        cbChange.ItemsSource = ctx.Klants.Select(x => x.Bedrijf).ToList();
                    }
                    break;
                case "Leverancier":
                    txtChange.Text = $"Welke leverancier wilt u {str}?";
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        cbChange.ItemsSource = ctx.Leveranciers.Select(x => x.Bedrijf).ToList();
                    }
                    break;
                case "Product":
                    txtChange.Text = $"Welk product wilt u {str}?";
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        cbChange.ItemsSource = ctx.Products.Select(x => x.Naam).ToList();
                    }
                    break;
                case "Personeel":
                    txtChange.Text = $"Welk personeelslid wilt u {str}?";
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        var leden = ctx.Personeelslids.Select(x => new {x.Id, Naam = x.Voornaam + " " + x.Achternaam }).ToList();
                        foreach (var persoon in leden)
                        {
                            cbChange.Items.Add(persoon.Naam);
                        }
                    }
                    break;
                case "Categorie":
                    txtChange.Text = $"Welke categorie wilt u {str}?";
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        cbChange.ItemsSource = ctx.Subcategories.Select(x => x.Naam).ToList();
                    }
                    break;

                default:
                    MessageBox.Show($"Geen Selector gevonden / {Selector}");
                    break;
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {

            switch (Selector)
            {
                case "Klant":
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        thisId = ctx.Klants.Where(k => k.Bedrijf == (string)cbChange.SelectedValue).Select(k => k.Id).FirstOrDefault();
                    }
                    break;
                case "Leverancier":
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        thisId = ctx.Leveranciers.Where(k => k.Bedrijf == (string)cbChange.SelectedValue).Select(k => k.Id).FirstOrDefault();
                    }
                    break;
                case "Product":
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        thisId = ctx.Products.Where(p => p.Naam == (string)cbChange.SelectedValue).Select(p => p.Id).FirstOrDefault();
                    }
                    break;
                case "Personeel":
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        thisId = ctx.Personeelslids.Where(p => p.Voornaam + " " + p.Achternaam == (string)cbChange.SelectedValue).Select(p => p.Id).FirstOrDefault();
                    }
                    break;
                case "Categorie":
                    using (MagazijnEntities ctx = new MagazijnEntities())
                    {
                        thisId = ctx.Subcategories.Where(c => c.Naam == (string)cbChange.SelectedValue).Select(c => c.Id).FirstOrDefault();
                    }
                    break;

                default:
                    MessageBox.Show($"Geen Selector gevonden met naam {Selector}");
                    break;
            }
            DialogResult = true;

        }
    }
}
