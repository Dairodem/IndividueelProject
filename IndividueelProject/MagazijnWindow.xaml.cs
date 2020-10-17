using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        string[] overzichtArr = new string[] { "Stock","Producten", "Klanten","Leveranciers" };
        Dictionary<string, string> sortStockDict = new Dictionary<string, string>()
        {
            {"Id","Id" },
            {"Naam A>Z","naamUp" },
            {"Naam Z>A","naamDown" },
            {"Categorie A>Z","catUp" },
            {"Categorie Z>A","catDown" },
            {"Aantal Laag>Hoog","aantalUp" },
            {"Aantal Hoog>Laag","aantalDown" },
            {"Leverancier A>Z","levUp" },
            {"Leverancier Z>A","levDown" },
        };
        Dictionary<string, string> sortProductDict = new Dictionary<string, string>()
        {
            {"Id","Id" },
            {"Naam A>Z","naamUp" },
            {"Naam Z>A","naamDown" },
            {"Categorie A>Z","catUp" },
            {"Categorie Z>A","catDown" },
            {"Prijs Laag>Hoog","aantalUp" },
            {"Prijs Hoog>Laag","aantalDown" },
            {"Leverancier A>Z","levUp" },
            {"Leverancier Z>A","levDown" },
        };
        Dictionary<string, string> sortLeverKlantDict = new Dictionary<string, string>()
        {
            {"Id","Id" },
            {"Bedrijf A>Z","bedrUp" },
            {"Bedrijf Z>A","bedrDown" },
        };

        string errorText = "";
        bool isError = false;

        public MagazijnWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxOverzicht.ItemsSource = overzichtArr;
            CbxOverzicht.SelectedIndex = 0;
            ChkNew.IsChecked = true;
            ChkCust.IsChecked = true;
            ChangeWidth();
        }
        private void ChangeColumns(string view)
        {
            switch (view)
            {
                case "Stock":
                    colID.DisplayMemberBinding = new Binding("ps2.ps.s.Id");
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("ps2.Naam");
                    col1.Header = "Cat.";
                    col1.Width = 150;
                    col2.DisplayMemberBinding = new Binding("ps2.ps.p.Naam");
                    col2.Header = "Naam";
                    col2.Width = 250;
                    col3.DisplayMemberBinding = new Binding("ps2.ps.s.Aantal");
                    col3.Header = "Aantal";
                    col3.Width = 50;
                    col4.DisplayMemberBinding = new Binding("ps2.ps.p.Eenheid");
                    col4.Header = "Eenheid";
                    col4.Width = 60;
                    col5.DisplayMemberBinding = new Binding("l.Bedrijf");
                    col5.Header = "Leverancier";
                    col5.Width = 100;
                    col6.DisplayMemberBinding = new Binding("x");
                    col6.Header = "";
                    col6.Width = 2;
                    break;
                case "Producten":
                    colID.DisplayMemberBinding = new Binding("pc.p.Id");
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("pc.Naam");
                    col1.Header = "Cat.";
                    col1.Width = 150;
                    col2.DisplayMemberBinding = new Binding("pc.p.Naam");
                    col2.Header = "Naam";
                    col2.Width = 250;
                    col3.DisplayMemberBinding = new Binding("pc.p.Inkoopprijs");
                    col3.Header = "Inkoopprijs";
                    col3.Width = 100;
                    col4.DisplayMemberBinding = new Binding("pc.p.Eenheid");
                    col4.Header = "Eenheid";
                    col4.Width = 60;
                    col5.DisplayMemberBinding = new Binding("pc.p.Marge");
                    col5.Header = "Marge";
                    col6.Width = 50;
                    col6.DisplayMemberBinding = new Binding("pc.p.BTW");
                    col6.Header = "BTW";
                    col6.Width = 50;
                    break;
                case "Klanten":
                    colID.DisplayMemberBinding = new Binding("Id");
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("Bedrijf");
                    col1.Header = "Bedrijf";
                    col1.Width = 200;
                    col2.DisplayMemberBinding = new Binding("Telefoonnummer");
                    col2.Header = "Telnr.";
                    col2.Width = 100;
                    col3.DisplayMemberBinding = new Binding("AangemaaktOp");
                    col3.Header = "Klant sinds";
                    col3.Width = 170;
                    col4.DisplayMemberBinding = new Binding("x");
                    col4.Header = "";
                    col4.Width = 2;
                    col5.DisplayMemberBinding = new Binding("x");
                    col5.Header = "";
                    col5.Width = 2;
                    col6.DisplayMemberBinding = new Binding("x");
                    col6.Header = "";
                    col6.Width = 2;
                    break;
                case "Leveranciers":
                    colID.DisplayMemberBinding = new Binding("Id");
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("Bedrijf");
                    col1.Header = "Bedrijf";
                    col1.Width = 100;
                    col2.DisplayMemberBinding = new Binding("Telefoonnummer");
                    col2.Header = "Telnr.";
                    col2.Width = 100;
                    col3.DisplayMemberBinding = new Binding("Emailadres");
                    col3.Header = "Emailadres";
                    col3.Width = 170;
                    col4.DisplayMemberBinding = new Binding("Straatnaam");
                    col4.Header = "Straat";
                    col4.Width = 150;
                    col5.DisplayMemberBinding = new Binding("Huisnummer");
                    col5.Header = "Huisnr.";
                    col5.Width = 70;
                    col6.DisplayMemberBinding = new Binding("Bus");
                    col6.Header = "Bus";
                    col6.Width = 50;
                    break;
                default:
                    break;
            }
        }
        private void ChangeWidth()
        {
            tabOverzicht.Width = Width / 3.5;
            tabData.Width = Width / 3.5;
            tabBestelling.Width = Width / 3.5;
        }
        private int ConvertToInt(string str, string messageText)
        {
            int number = 0;

            try
            {
                number = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                errorText += $"{messageText}\n";
                isError = true;
            }

            return number;
        }
        private decimal ConvertToDecimal(string str, string messageText)
        {
            decimal number = 0;

            try
            {
                number = Convert.ToDecimal(str);
            }
            catch (Exception)
            {
                errorText += $"{messageText}\n";
                isError = true;
            }

            return number;
        }

        private void CbxOverzicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {

                switch (CbxOverzicht.SelectedItem.ToString())
                {
                    case "Stock":
                        ChangeColumns("Stock");

                        var stockList = ctx.Products.Join(
                            ctx.Stocks,
                            p => p.Id,
                            s => s.IdProduct,
                            (p, s) => new { p, s }).Join(
                            ctx.Subcategories,
                            ps => ps.p.IdSubcategorie,
                            c => c.Id,
                            (ps, c) => new { ps, c.Naam }).Join(
                            ctx.Leveranciers,
                            ps2 => ps2.ps.p.IdLeverancier,
                            l => l.Id,
                            (ps2, l) => new { ps2, l }).ToList();

                        CbxSort.ItemsSource = sortStockDict.Keys;
                        LvOverzicht.ItemsSource = stockList;
                        break;

                    case "Producten":
                        ChangeColumns("Producten");

                        var productList = ctx.Products.Join(
                            ctx.Subcategories,
                            p => p.IdSubcategorie,
                            c => c.Id,
                            (p, c) => new { p, c.Naam }).Join(
                            ctx.Leveranciers,
                            pc => pc.p.IdLeverancier,
                            l => l.Id,
                            (pc, l) => new { pc, l }).ToList();

                        CbxSort.ItemsSource = sortProductDict.Keys;
                        LvOverzicht.ItemsSource = productList;
                        break;

                    case "Klanten":
                        var klantList = ctx.Klants.Select(k => k).ToList();
                        CbxSort.ItemsSource = sortLeverKlantDict.Keys;
                        LvOverzicht.ItemsSource = klantList;
                        ChangeColumns("Klanten");
                        break;

                    case "Leveranciers":
                        var leverList = ctx.Leveranciers.Select(k => k).ToList();
                        CbxSort.ItemsSource = sortLeverKlantDict.Keys;
                        LvOverzicht.ItemsSource = leverList;
                        ChangeColumns("Leveranciers");
                        break;

                    default:
                        break;
                }

            }
        }
        private void CbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(LvOverzicht.ItemsSource);

            switch (CbxSort.SelectedValue)
            {
                case "Id":
                    view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    break;
                case "naamUp":
                    view.SortDescriptions.Add(new SortDescription("Naam", ListSortDirection.Ascending));
                    break;
                case "naamDown":
                    view.SortDescriptions.Add(new SortDescription("Naam", ListSortDirection.Descending));
                    break;
                case "catUp":
                    break;
                case "catDown":
                    break;
                case "levUp":
                    break;
                case "levDown":
                    break;
                case "aantalUp":
                    break;
                case "aantalDown":
                    break;
                case "prijsUp":
                    break;
                case "prijsDown":
                    break;
                case "bedrUp":
                    break;
                case "bedrDown":
                    break;
                default:
                    break;
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ChangeWidth();
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            ChangeWidth();
        }
        private void ChkNew_Checked(object sender, RoutedEventArgs e)
        {
                ChkChange.IsChecked = false;
        }
        private void ChkChange_Checked(object sender, RoutedEventArgs e)
        {
            ChkNew.IsChecked = false;
        }
        private void ChkCust_Checked(object sender, RoutedEventArgs e)
        {
            ChkDealer.IsChecked = false;
            ChkProd.IsChecked = false;
            ChkEmp.IsChecked = false;
            ChkCat.IsChecked = false;

            LblName.Visibility = Visibility.Visible;
            TxtName.Visibility = Visibility.Visible;
            LblStreet.Visibility = Visibility.Visible;
            TxtStreet.Visibility = Visibility.Visible;
            LblNumber.Visibility = Visibility.Visible;
            TxtNumber.Visibility = Visibility.Visible;
            LblBus.Visibility = Visibility.Visible;
            TxtBus.Visibility = Visibility.Visible;
            LblPostal.Visibility = Visibility.Visible;
            TxtPostal.Visibility = Visibility.Visible;
            LblCity.Visibility = Visibility.Visible;
            TxtCity.Visibility = Visibility.Visible;

            LblRemark.Visibility = Visibility.Visible;
            TxtRemark.Visibility = Visibility.Visible;
            LblDate.Visibility = Visibility.Visible;
            DpDate.Visibility = Visibility.Visible;
            LblEmail.Visibility = Visibility.Visible;
            TxtEmail.Visibility = Visibility.Visible;
            LblTel.Visibility = Visibility.Visible;
            TxtTel.Visibility = Visibility.Visible;
            CbxDealer.Visibility = Visibility.Collapsed;
            CbxCat.Visibility = Visibility.Collapsed;
            CbxFunction.Visibility = Visibility.Collapsed;

            LblName.Text = "Naam van bedrijf:";
            LblStreet.Text = "Straat:";
            LblNumber.Text = "Huisnummer:";
            LblBus.Text = "Bus:";
            LblPostal.Text = "Postcode:";
            LblCity.Text = "Gemeente:";
            LblEmail.Text = "Emailadres:";
            LblTel.Text = "Telefoonnummer:";
        }
        private void ChkDealer_Checked(object sender, RoutedEventArgs e)
        {
            ChkCust.IsChecked = false;
            ChkProd.IsChecked = false;
            ChkEmp.IsChecked = false;
            ChkCat.IsChecked = false;

            LblName.Visibility = Visibility.Visible;
            TxtName.Visibility = Visibility.Visible;
            LblStreet.Visibility = Visibility.Visible;
            TxtStreet.Visibility = Visibility.Visible;
            LblNumber.Visibility = Visibility.Visible;
            TxtNumber.Visibility = Visibility.Visible;
            LblBus.Visibility = Visibility.Visible;
            TxtBus.Visibility = Visibility.Visible;
            LblPostal.Visibility = Visibility.Visible;
            TxtPostal.Visibility = Visibility.Visible;
            LblCity.Visibility = Visibility.Visible;
            TxtCity.Visibility = Visibility.Visible;

            LblRemark.Visibility = Visibility.Hidden;
            TxtRemark.Visibility = Visibility.Hidden;
            LblDate.Visibility = Visibility.Hidden;
            DpDate.Visibility = Visibility.Hidden;
            LblEmail.Visibility = Visibility.Visible;
            TxtEmail.Visibility = Visibility.Visible;
            LblTel.Visibility = Visibility.Visible;
            TxtTel.Visibility = Visibility.Visible;
            CbxDealer.Visibility = Visibility.Collapsed;
            CbxCat.Visibility = Visibility.Collapsed;
            CbxFunction.Visibility = Visibility.Collapsed;

            LblName.Text = "Naam van bedrijf:";
            LblStreet.Text = "Straat:";
            LblNumber.Text = "Huisnummer:";
            LblBus.Text = "Bus:";
            LblPostal.Text = "Postcode:";
            LblCity.Text = "Gemeente:";
            LblEmail.Text = "Emailadres:";
            LblTel.Text = "Telefoonnummer:";

        }
        private void ChkProd_Checked(object sender, RoutedEventArgs e)
        {
            ChkCust.IsChecked = false;
            ChkDealer.IsChecked = false;
            ChkEmp.IsChecked = false;
            ChkCat.IsChecked = false;

            LblName.Visibility = Visibility.Visible;
            TxtName.Visibility = Visibility.Visible;
            LblStreet.Visibility = Visibility.Visible;
            TxtStreet.Visibility = Visibility.Visible;
            LblNumber.Visibility = Visibility.Visible;
            TxtNumber.Visibility = Visibility.Visible;
            LblBus.Visibility = Visibility.Visible;
            TxtBus.Visibility = Visibility.Visible;
            LblPostal.Visibility = Visibility.Visible;
            TxtPostal.Visibility = Visibility.Visible;
            LblEmail.Visibility = Visibility.Visible;
            LblTel.Visibility = Visibility.Visible;

            LblRemark.Visibility = Visibility.Hidden;
            TxtRemark.Visibility = Visibility.Hidden;
            LblDate.Visibility = Visibility.Hidden;
            DpDate.Visibility = Visibility.Hidden;
            LblCity.Visibility = Visibility.Hidden;
            TxtCity.Visibility = Visibility.Hidden;
            TxtEmail.Visibility = Visibility.Collapsed;
            TxtTel.Visibility = Visibility.Collapsed;
            CbxFunction.Visibility = Visibility.Collapsed;
            CbxDealer.Visibility = Visibility.Visible;
            CbxCat.Visibility = Visibility.Visible;

            LblName.Text = "Naam van product:";
            LblStreet.Text = "Inkoopprijs:";
            LblNumber.Text = "Marge:";
            LblBus.Text = "BTW:";
            LblPostal.Text = "Eenheid:";
            LblEmail.Text = "Leverancier:";
            LblTel.Text = "Categorie:";
        }
        private void ChkEmp_Checked(object sender, RoutedEventArgs e)
        {
            ChkCust.IsChecked = false;
            ChkDealer.IsChecked = false;
            ChkProd.IsChecked = false;
            ChkCat.IsChecked = false;

            LblName.Visibility = Visibility.Visible;
            TxtName.Visibility = Visibility.Visible;
            LblStreet.Visibility = Visibility.Visible;
            TxtStreet.Visibility = Visibility.Visible;
            LblNumber.Visibility = Visibility.Visible;
            TxtNumber.Visibility = Visibility.Visible;
            LblBus.Visibility = Visibility.Visible;
            TxtBus.Visibility = Visibility.Visible;
            LblPostal.Visibility = Visibility.Visible;
            TxtPostal.Visibility = Visibility.Visible;

            LblRemark.Visibility = Visibility.Hidden;
            TxtRemark.Visibility = Visibility.Hidden;
            LblDate.Visibility = Visibility.Hidden;
            DpDate.Visibility = Visibility.Hidden;
            LblEmail.Visibility = Visibility.Hidden;
            TxtEmail.Visibility = Visibility.Hidden;
            LblTel.Visibility = Visibility.Hidden;
            TxtTel.Visibility = Visibility.Hidden;
            LblCity.Visibility = Visibility.Visible;
            CbxFunction.Visibility = Visibility.Visible;
            TxtCity.Visibility = Visibility.Collapsed;
            CbxDealer.Visibility = Visibility.Collapsed;
            CbxCat.Visibility = Visibility.Collapsed;

            LblName.Text = "Voornaam:";
            LblStreet.Text = "Achternaam:";
            LblNumber.Text = "Login:";
            LblBus.Text = "Wachtwoord:";
            LblPostal.Text = "Wachtwoord bevestigen:";
            LblCity.Text = "Functie:";
        }
        private void ChkCat_Checked(object sender, RoutedEventArgs e)
        {
            ChkCust.IsChecked = false;
            ChkDealer.IsChecked = false;
            ChkProd.IsChecked = false;
            ChkEmp.IsChecked = false;

            LblName.Visibility = Visibility.Hidden;
            TxtName.Visibility = Visibility.Hidden;
            LblStreet.Visibility = Visibility.Hidden;
            TxtStreet.Visibility = Visibility.Hidden;
            LblNumber.Visibility = Visibility.Hidden;
            TxtNumber.Visibility = Visibility.Hidden;
            LblBus.Visibility = Visibility.Hidden;
            TxtBus.Visibility = Visibility.Hidden;
            LblPostal.Visibility = Visibility.Hidden;
            TxtPostal.Visibility = Visibility.Hidden;
            LblEmail.Visibility = Visibility.Hidden;
            LblTel.Visibility = Visibility.Hidden;
            LblRemark.Visibility = Visibility.Hidden;
            TxtRemark.Visibility = Visibility.Hidden;
            LblDate.Visibility = Visibility.Hidden;
            DpDate.Visibility = Visibility.Hidden;
            TxtEmail.Visibility = Visibility.Hidden;
            TxtTel.Visibility = Visibility.Hidden;
            LblCity.Visibility = Visibility.Visible;
            TxtCity.Visibility = Visibility.Visible;
            CbxDealer.Visibility = Visibility.Collapsed;
            CbxCat.Visibility = Visibility.Collapsed;
            CbxFunction.Visibility = Visibility.Collapsed;

            LblCity.Text = "Categorienaam:";
        }
        private void ChkNew_Unchecked(object sender, RoutedEventArgs e)
        {
            ChkChange.IsChecked = true;
        }
        private void ChkChange_Unchecked(object sender, RoutedEventArgs e)
        {
            ChkNew.IsChecked = true;
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                if (ChkCust.IsChecked == true)
                {
                    ctx.Klants.Add(new Klant()
                    {
                        Bedrijf = TxtName.Text,
                        Straatnaam = TxtStreet.Text,
                        Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect huisnummer!"),
                        Bus = TxtBus.Text, 
                        Postcode = TxtPostal.Text,
                        Gemeente = TxtCity.Text,
                        Emailadres = TxtEmail.Text,
                        Telefoonnummer = TxtTel.Text,
                        Opmerking = TxtRemark.Text,
                        AangemaaktOp = DpDate.SelectedDate
                    });
                }
                if (ChkDealer.IsChecked == true)
                {
                    ctx.Leveranciers.Add(new Leverancier()
                    {
                        Bedrijf = TxtName.Text,
                        Straatnaam = TxtStreet.Text,
                        Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect huisnummer!"),
                        Bus = TxtBus.Text,
                        Postcode = TxtPostal.Text,
                        Gemeente = TxtCity.Text,
                        Emailadres = TxtEmail.Text,
                        Telefoonnummer = TxtTel.Text
                    });
                }
                if (ChkProd.IsChecked == true)
                {
                    ctx.Products.Add(new Product()
                    {
                        Naam = TxtName.Text,
                        Inkoopprijs = ConvertToDecimal(TxtStreet.Text,"Inkoopprijs ongeldig!"),
                        Marge = ConvertToDecimal(TxtNumber.Text,"Marge ongeldig!"),
                        BTW = ConvertToDecimal(TxtBus.Text, "BTW ongeldig!"),
                        Eenheid = TxtPostal.Text,
                        IdLeverancier = (int)CbxDealer.SelectedValue,
                        IdSubcategorie = (int)CbxCat.SelectedValue
                    });
                }
                if (ChkEmp.IsChecked == true)
                {
                    ctx.Personeelslids.Add(new Personeelslid()
                    {
                        Voornaam = TxtName.Text,
                        Achternaam = TxtStreet.Text,
                        Login = TxtNumber.Text,
                        Wachtwoord = TxtBus.Text,
                        Afdeling = CbxFunction.SelectedItem.ToString()
                    });
                }
                if (ChkCat.IsChecked == true)
                {
                    ctx.Subcategories.Add(new Subcategorie()
                    {
                        Naam = TxtCity.Text
                    });
                }

                if (!isError)
                {
                    ctx.SaveChanges();
                }
                else
                {
                    MessageBox.Show(errorText);
                }
                isError = false;
            }

        }
    }
}
