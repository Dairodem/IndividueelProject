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
    /// Interaction logic for MagazijnWindow.xaml
    /// </summary>
    /// 

    /*-------------------------------TO DO---------------------------------

     * overzichtTab uitwerken
     * -opmaak verbeteren zodat alles zichtbaar is
     * -Sorteer opties
     * -Filter opties
     * -zoeken op ...
     * 
     * 
     * databeheerTab uitwerken (voor admin)
     * -personeel toevoegen/bewerken/verwijderen
     * -product  toevoegen/bewerken/verwijderen
     * -klant toevoegen/bewerken/verwijderen
     * -leverancier toevoegen/bewerken/verwijderen
     * -categorie toevoegen/bewerken/verwijderen
     * -subcategorie toevoegen/bewerken/verwijderen
     * 
     * bestellingTab uitwerken
     *-list van leveranciers/klanten
     *-list van producten uit stock / bij leverancier met aantal
     *-keuze voor quantiteit 
     *-totale prijs
     *
     * 
     */


    public partial class MagazijnWindow : Window
    {
        string[] overzichtArr = new string[] { "Stock","Klanten","Leveranciers" };
        Dictionary<string, string> sortDict = new Dictionary<string, string>()
        {
            {"Id","Id" },
            {"Alfabetisch A>Z","alfaUp" },
            {"Alfabetisch Z>A","alfaDown" },
        };

        public MagazijnWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxOverzicht.ItemsSource = overzichtArr;
            CbxOverzicht.SelectedIndex = 0;
            
            CbxSort.ItemsSource = sortDict.Keys;
            CbxSort.SelectedIndex = 0;

            tabOverzicht.Width = Width / 4;
            tabData.Width = Width / 4;
            tabBestelling.Width = Width / 4;
        }
        private void ChangeColumns(string view)
        {
            switch (view)
            {
                case "Stock":
                    colID.DisplayMemberBinding = new Binding("ps.s.Id");
                    col1.DisplayMemberBinding = new Binding("Naam");
                    col1.Header = "Cat.";
                    col2.DisplayMemberBinding = new Binding("ps.p.Naam");
                    col2.Header = "Naam";
                    col3.DisplayMemberBinding = new Binding("ps.s.Aantal");
                    col3.Header = "Aantal";
                    col4.DisplayMemberBinding = new Binding("ps.p.Eenheid");
                    col4.Header = "Eenheid";
                    col5.DisplayMemberBinding = new Binding("ps.p.Inkoopprijs");
                    col5.Header = "Inkoopprijs";
                    col6.DisplayMemberBinding = new Binding("ps.p.Marge");
                    col6.Header = "Marge";
                    break;
                case "Klanten":
                    colID.DisplayMemberBinding = new Binding("Id");
                    col1.DisplayMemberBinding = new Binding("Bedrijf");
                    col1.Header = "Bedrijf";
                    col1.Width = 200;
                    col2.DisplayMemberBinding = new Binding("Telefoonnummer");
                    col2.Header = "Telefoonnummer";
                    col2.Width = 100;
                    col3.DisplayMemberBinding = new Binding("AangemaaktOp");
                    col3.Header = "Aangemaakt op";
                    break;
                case "Leveranciers":
                    colID.DisplayMemberBinding = new Binding("Id");
                    col1.DisplayMemberBinding = new Binding("Bedrijf");
                    col1.Header = "Bedrijf";
                    col2.DisplayMemberBinding = new Binding("Telefoonnummer");
                    col2.Header = "Tel.";
                    col3.DisplayMemberBinding = new Binding("Emailadres");
                    col3.Header = "Emailadres";
                    col4.DisplayMemberBinding = new Binding("Straatnaam");
                    col4.Header = "Straat";
                    col5.DisplayMemberBinding = new Binding("Huisnummer");
                    col5.Header = "Huisnummer";
                    col6.DisplayMemberBinding = new Binding("Bus");
                    col6.Header = "Bus";
                    break;
                default:
                    break;
            }
        }
        private void CbxOverzicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                
                switch (CbxOverzicht.SelectedItem.ToString())
                {
                    case "Stock":
                        ChangeColumns("Stock");
                        var productList = ctx.Products.Join(
                            ctx.Stocks,
                            p => p.Id,
                            s => s.IdProduct,
                            (p, s) => new {p,s }).Join(
                            ctx.Subcategories,
                            ps => ps.p.IdSubcategorie,
                            c => c.Id,
                            (ps , c) => new {ps, c.Naam }).ToList();

                        LvOverzicht.ItemsSource = productList;
                        break;

                    case "Klanten":
                        LvOverzicht.ItemsSource = ctx.Klants.Select(k => k).ToList();
                        ChangeColumns("Klanten");
                        break;
                    case "Leveranciers":
                        LvOverzicht.ItemsSource = ctx.Leveranciers.Select(k => k).ToList();
                        ChangeColumns("Leveranciers");
                        break;

                    default:
                        break;
                }
            }
        }

        private void CbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ChangeWidth();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            ChangeWidth();
        }
        private void ChangeWidth()
        {
            tabOverzicht.Width = Width / 3.5;
            tabData.Width = Width / 3.5;
            tabBestelling.Width = Width / 3.5;
        }
    }
}
