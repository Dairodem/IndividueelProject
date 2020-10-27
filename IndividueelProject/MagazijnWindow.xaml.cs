using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
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

     * 
     * 
     * 
     * 
     * bij bewerken :
     *               -extra knop bij 'product' om document in te lezen
     *               
     *               
     *
     * overzichtTab uitwerken
     * -Sorteer opties
     * -Filter opties
     * -zoeken op ...
     * 
     * 
     * bestellingTab uitwerken
     *-list van leveranciers/klanten
     *-totale prijs
     *
     * 
     */


    public partial class MagazijnWindow : Window
    {
        private int myId = 0;
        private string[] overzichtArr = new string[] { "Stock", "Producten", "Klanten", "Leveranciers", "Personeel" };
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

        Klant klant = new Klant();
        Leverancier dealer = new Leverancier();
        Product product = new Product();
        Personeelslid person = new Personeelslid();
        Subcategorie cat = new Subcategorie();
        Order order = new Order();

        string selection = "";
        string errorText = "";
        bool isError = false;
        bool toChange = false;
        public int quantity = 0;
        public int Linecount { get; set; }

        public MagazijnWindow()
        {
            InitializeComponent();

            rbNew.IsChecked = true;
            rbCust.IsChecked = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxOverzicht.ItemsSource = overzichtArr;
            LvOverzichtAankoop2.ItemsSource = order.LineList;
            CbxOverzicht.SelectedIndex = 0;
            RefreshDealerList(cbAankoopBij);
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
        private void SetDataCustomer()
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                klant = ctx.Klants.Where(k => k.Id == myId).FirstOrDefault();

                TxtName.Text = klant.Bedrijf;
                TxtStreet.Text = klant.Straatnaam;
                TxtNumber.Text = klant.Huisnummer.ToString();
                TxtBus.Text = klant.Bus;
                TxtPostal.Text = klant.Postcode;
                TxtCity.Text = klant.Gemeente;
                TxtEmail.Text = klant.Emailadres;
                TxtTel.Text = klant.Telefoonnummer;
                TxtRemark.Text = klant.Opmerking;
                DpDate.SelectedDate = klant.AangemaaktOp;

            }
        }
        private void SetDataDealer()
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                dealer = ctx.Leveranciers.Where(d => d.Id == myId).FirstOrDefault();
                TxtName.Text = dealer.Bedrijf;
                TxtStreet.Text = dealer.Straatnaam;
                TxtNumber.Text = dealer.Huisnummer.ToString();
                TxtBus.Text = dealer.Bus;
                TxtPostal.Text = dealer.Postcode;
                TxtCity.Text = dealer.Gemeente;
                TxtEmail.Text = dealer.Emailadres;
                TxtTel.Text = dealer.Telefoonnummer;

            }
        }
        private void SetDataProduct()
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                product = ctx.Products.Where(p => p.Id == myId).FirstOrDefault();
                dealer = ctx.Leveranciers.Where(d => d.Id == product.IdLeverancier).FirstOrDefault();
                cat = ctx.Subcategories.Where(c => c.Id == product.IdSubcategorie).FirstOrDefault();

                TxtName.Text = product.Naam;
                TxtStreet.Text = product.Inkoopprijs.ToString();
                TxtNumber.Text = product.Marge.ToString();
                TxtBus.Text = product.BTW.ToString();
                TxtPostal.Text = product.Eenheid;
                CbxDealer.SelectedItem = dealer.Bedrijf;
                CbxCat.SelectedItem = cat.Naam;

            }
        }
        private void SetDataEmployee()
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                person = ctx.Personeelslids.Where(p => p.Id == myId).FirstOrDefault();

                TxtName.Text = person.Voornaam;
                TxtStreet.Text = person.Achternaam;
                TxtNumber.Text = person.Login;
                TxtBus.Text = person.Wachtwoord;
                TxtPostal.Text = person.Wachtwoord;
                TxtCity.Text = person.Afdeling;
                CbxFunction.SelectedItem = person.Afdeling;

            }
        }
        private void SetDataCategory()
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                cat = ctx.Subcategories.Where(p => p.Id == myId).FirstOrDefault();

                TxtCity.Text = cat.Naam;

            }
        }
        private void RefreshDealerList(ComboBox toComboBox)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                toComboBox.ItemsSource = ctx.Leveranciers.Select(l => l.Bedrijf).ToList();
                toComboBox.SelectedIndex = 0;
            }
        }
        private void ClearAllText()
        {
            TxtName.Text = "";
            TxtStreet.Text = "";
            TxtNumber.Text = "";
            TxtBus.Text = "";
            TxtPostal.Text = "";
            TxtCity.Text = "";
            TxtEmail.Text = "";
            TxtTel.Text = "";
            TxtRemark.Text = "";
            DpDate.SelectedDate = DateTime.Now;
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
        private void rbCust_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllText();

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

            selection = "Klant";
        }
        private void rbDealer_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllText();

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

            selection = "Leverancier";
        }
        private void rbProd_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllText();

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

            selection = "Product";
        }
        private void rbEmp_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllText();

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

            selection = "Personeel";
        }
        private void rbCat_Checked(object sender, RoutedEventArgs e)
        {
            ClearAllText();

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

            selection = "Categorie";
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                switch (selection)
                {
                    case "Klant":
                        //Bewerken of nieuwe klant
                        if (toChange)
                        {
                            klant = ctx.Klants.Where(k => k.Id == myId).FirstOrDefault();

                            klant.Bedrijf = TxtName.Text;
                            klant.Straatnaam = TxtStreet.Text;
                            klant.Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect Huisnummer");
                            klant.Bus = TxtBus.Text;
                            klant.Postcode = TxtPostal.Text;
                            klant.Gemeente = TxtCity.Text;
                            klant.Emailadres = TxtEmail.Text;
                            klant.Telefoonnummer = TxtTel.Text;
                            klant.Opmerking = TxtRemark.Text;
                            klant.AangemaaktOp = DpDate.SelectedDate;

                        }
                        else
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
                        break;

                    case "Leverancier":
                        //Bewerken of nieuwe leverancier
                        if (toChange)
                        {
                            dealer = ctx.Leveranciers.Where(k => k.Id == myId).FirstOrDefault();

                            dealer.Bedrijf = TxtName.Text;
                            dealer.Straatnaam = TxtStreet.Text;
                            dealer.Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect Huisnummer");
                            dealer.Bus = TxtBus.Text;
                            dealer.Postcode = TxtPostal.Text;
                            dealer.Gemeente = TxtCity.Text;
                            dealer.Emailadres = TxtEmail.Text;
                            dealer.Telefoonnummer = TxtTel.Text;

                        }
                        else
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
                        break;

                    case "Product":
                        //Bewerken of nieuwe product
                        if (toChange)
                        {
                            product = ctx.Products.Where(k => k.Id == myId).FirstOrDefault();

                            product.Naam = TxtName.Text;
                            product.Inkoopprijs = ConvertToDecimal(TxtStreet.Text, "Inkoopprijs niet correct ingegeven");
                            product.Marge = ConvertToInt(TxtNumber.Text, "Incorrecte marge ingegeven");
                            product.BTW = ConvertToInt(TxtBus.Text, "BTW niet juist ingegeven");
                            product.Eenheid = TxtPostal.Text;
                            product.IdLeverancier = dealer.Id;
                            product.IdSubcategorie = (int)CbxCat.SelectedValue;

                        }
                        else
                        {
                            ctx.Products.Add(new Product()
                            {
                                Naam = TxtName.Text,
                                Inkoopprijs = ConvertToDecimal(TxtStreet.Text, "Inkoopprijs ongeldig!"),
                                Marge = ConvertToDecimal(TxtNumber.Text, "Marge ongeldig!"),
                                BTW = ConvertToDecimal(TxtBus.Text, "BTW ongeldig!"),
                                Eenheid = TxtPostal.Text,
                                IdLeverancier = (int)CbxDealer.SelectedValue,
                                IdSubcategorie = (int)CbxCat.SelectedValue
                            });
                        }
                        break;

                    case "Personeel":
                        //Bewerken of nieuwe personeelslid
                        if (toChange)
                        {
                            person = ctx.Personeelslids.Where(k => k.Id == myId).FirstOrDefault();
                            person.Voornaam = TxtName.Text;
                            person.Achternaam = TxtStreet.Text;
                            person.Login = TxtNumber.Text;
                            person.Wachtwoord = TxtBus.Text;
                            person.Afdeling = CbxFunction.SelectedItem.ToString();

                        }
                        else
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
                        break;

                    case "Categorie":
                        //Bewerken of nieuwe categorie
                        if (toChange)
                        {
                            cat = ctx.Subcategories.Where(k => k.Id == myId).FirstOrDefault();
                            cat.Naam = TxtCity.Text;
                        }
                        else
                        {
                            ctx.Subcategories.Add(new Subcategorie()
                            {
                                Naam = TxtCity.Text
                            });
                        }
                        break;

                    default:
                        MessageBox.Show("Geen Selector gevonden");
                        break;
                }

                if (!isError)
                {
                    ctx.SaveChanges();
                    MessageBox.Show("// Data opgeslagen //");
                }
                else
                {
                    MessageBox.Show(errorText);
                }

                isError = false;
            }

        }
        private void rbChange_Checked(object sender, RoutedEventArgs e)
        {
            BtnAdd.Content = "Bewerken";
            toChange = true;
        }
        private void rbNew_Checked(object sender, RoutedEventArgs e)
        {
            BtnAdd.Content = "Toevoegen";
            toChange = false;

        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            rbChange.IsChecked = true;

            WindowChange windowChange = new WindowChange
            {
                Selector = selection,
                toChange = true,
                Owner = this
            };
            windowChange.ShowDialog();

            myId = windowChange.thisId;

            switch (selection)
            {
                case "Klant":
                    SetDataCustomer();
                    break;
                case "Leverancier":
                    SetDataDealer();
                    break;
                case "Product":
                    SetDataProduct();
                    break;
                case "Personeel":
                    SetDataEmployee();
                    break;
                case "Categorie":
                    SetDataCategory();
                    break;

                default:
                    MessageBox.Show($"Geen Selector gevonden / {selection}");
                    break;
            }
        }

        private void cbAankoopBij_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            order.LineList.Clear();

            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                Leverancier myDealer = ctx.Leveranciers.Where(d => d.Bedrijf == (string)cbAankoopBij.SelectedItem).FirstOrDefault();

                LvOverzichtAankoop1.ItemsSource = ctx.Products.Where(l => l.IdLeverancier == myDealer.Id).ToList();
                    /*
                    Join(ctx.Subcategories,
                    p => p.IdSubcategorie,
                    c => c.Id,
                    (p, c) => new { p, c }
                    ).ToList();*/
            }
        }
        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            WindowQuantity windowQuantity = new WindowQuantity
            {
                Owner = this
            };
            windowQuantity.ShowDialog();

            Button btn = sender as Button;
            Product product = (Product)btn.DataContext;
            order.LineList.Add(new Line(product, windowQuantity.Quantity));

            LvOverzichtAankoop2.ItemsSource = null;
            LvOverzichtAankoop2.ItemsSource = order.LineList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WindowChange windowDelete = new WindowChange();
            windowDelete.Selector = selection;
            windowDelete.toChange = false;
            windowDelete.Owner = this;
            windowDelete.ShowDialog();

            myId = windowDelete.thisId;


            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                switch (selection)
                {
                    case "Klant":
                        klant = ctx.Klants.Where(x => x.Id == myId).FirstOrDefault();
                        ctx.Klants.Remove(klant);
                        break;

                    case "Leverancier":
                        dealer = ctx.Leveranciers.Where(x => x.Id == myId).FirstOrDefault();
                        ctx.Leveranciers.Remove(dealer);
                        break;

                    case "Product":
                        product = ctx.Products.Where(x => x.Id == myId).FirstOrDefault();
                        ctx.Products.Remove(product);
                        break;

                    case "Personeel":
                        person = ctx.Personeelslids.Where(x => x.Id == myId).FirstOrDefault();
                        ctx.Personeelslids.Remove(person);
                        break;

                    case "Categorie":
                        cat = ctx.Subcategories.Where(x => x.Id == myId).FirstOrDefault();
                        ctx.Subcategories.Remove(cat);
                        break;

                    default:
                        MessageBox.Show($"Geen Selector gevonden met naam {selection}");
                        break;
                }

                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit element wilt verwijderen?", "Bevestiging",MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    ctx.SaveChanges();
                }
            }

        }
        private void btnDeleteLine_Click(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            Line line = (Line)btn.DataContext;

            order.LineList.Remove(line);

            LvOverzichtAankoop2.ItemsSource = null;
            LvOverzichtAankoop2.ItemsSource = order.LineList;
        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
