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
     * -listview van stock/klanten/leveranciers
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
        }

        private void CbxOverzicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                
                switch (CbxOverzicht.SelectedItem.ToString())
                {
                    case "Stock":
                        //kolom namen veranderen
                        colID.DisplayMemberBinding = new Binding("ps.p.Id");
                        col1.DisplayMemberBinding = new Binding("Naam");
                        col1.Header = "Cat.";
                        col2.DisplayMemberBinding =  new Binding("ps.p.Naam");
                        col2.Header = "Naam";
                        col3.DisplayMemberBinding = new Binding("ps.s.Aantal");
                        col3.Header = "Aantal";
                        col4.DisplayMemberBinding = new Binding("ps.p.Inkoopprijs");
                        col4.Header = "Inkoopprijs";
                        col5.DisplayMemberBinding = new Binding("x");
                        col6.DisplayMemberBinding = new Binding("x");

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
                        //kolom namen veranderen
                        colID.DisplayMemberBinding = new Binding("Id");
                        col1.DisplayMemberBinding = new Binding("Bedrijf");
                        col1.Header = "Bedrijf";
                        col2.DisplayMemberBinding = new Binding("Telefoonnummer");
                        col2.Header = "Telefoonnummer";
                        col3.DisplayMemberBinding = new Binding("AangemaaktOp");
                        col3.Header = "Aangemaakt op";
                        LvOverzicht.ItemsSource = ctx.Klants.Select(k => k).ToList();
                        break;
                    case "Leveranciers":
                        break;

                    default:
                        break;
                }
            }
        }

        private void CbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
