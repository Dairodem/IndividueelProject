using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using Syncfusion.XPS;

namespace IndividueelProject
{
    /// <summary>
    /// Interaction logic for MagazijnWindow.xaml
    /// </summary>
    /// 

    /*-------------------------------TO DO---------------------------------

     * 
     * 
     * usergegevens tonen
     * 
     * bij bewerken :- kijken of alle data up to date is
     *               
     *  
     * bestellingTab uitwerken
     *  -Orders tonen in lijst in & out apart (met keuze om meerdere te printen als dit nog niet is gebeurd)    
     *  
     *  
     * CreatePDF verder afwerken
     *
     * overzichtTab uitwerken
     * -Sorteer opties
     * -Filter opties
     * -zoeken op ...
     * 
     * gehele opmaak
     *
     * aan- en af-melden
     * menu aanvullen
     * 
     */


    public partial class MagazijnWindow : Window
    {
        public Personeelslid User = new Personeelslid();
        public int quantity;

        private int myId = 0;
        private int prevIndex = 0;
        private string[] overzichtArr = new string[] { "Stock", "Producten", "Klanten", "Leveranciers", "Personeel" };
        private string[] functieArr = new string[] { "Admin", "Magazijn", "Verkoop"};
        private GridViewColumnHeader listViewSortCol = null;
        private GridViewColumnHeader listViewSortCol1 = null;
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
        Order orderOUT = new Order();
        Order orderIN = new Order();
        List<Order> orderList = new List<Order>();
        Random rand = new Random();
        ListViewItem selectedOrder = new ListViewItem();
        OrdersView ordersView = new OrdersView();
        StockView stockView = new StockView();

        string selection = "";
        string errorText = "";
        bool isError = false;
        bool toChange = false;

        public MagazijnWindow()
        {
            InitializeComponent();

            lvOverzichtOrders.ItemsSource = ordersView.AllOrders;
            LvOverzicht.ItemsSource = stockView.AllStock;

            rbNew.IsChecked = true;
            rbCust.IsChecked = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetUserAccessFor(User);

            CbxOverzicht.ItemsSource = overzichtArr;
            CbxOverzicht.SelectedIndex = 0;
            CbxFunction.ItemsSource = functieArr;
            CbxFunction.SelectedIndex = 0;
            LvOverzichtAankoop2.ItemsSource = orderOUT.LineList;

            orderList.Clear();
            orderList.Add(new Order() { LineList = RandomLines(true), OrderedBy = "Car-Repair" });
            orderList.Add(new Order() { LineList = RandomLines(), OrderedBy = "Car-Repair" });
            orderList.Add(new Order() { LineList = RandomLines(), OrderedBy = "Onderdelen Vanhoutten" });
            orderList.Add(new Order() { LineList = RandomLines(), OrderedBy = "Garage Van Mechelen" });
            orderList.Add(new Order() { LineList = RandomLines(), OrderedBy = "Garage Opsomer" });

            lvOrders.ItemsSource = orderList;
            LvOverzichtVerkoop.ItemsSource = orderList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvOverzichtOrders.ItemsSource);
            CollectionView viewStock = (CollectionView)CollectionViewSource.GetDefaultView(LvOverzicht.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("InOut", ListSortDirection.Ascending));
            viewStock.SortDescriptions.Add(new SortDescription("Naam", ListSortDirection.Ascending));

            RefreshDealerList(cbAankoopBij);
            ChangeWidth();
        }
        private void SetUserAccessFor(Personeelslid user)
        {
            switch (user.Afdeling)
            {

                case "Magazijn":
                    //databeheer
                    rbCust.Visibility = Visibility.Collapsed;
                    rbProd.Visibility = Visibility.Collapsed;
                    rbEmp.Visibility = Visibility.Collapsed;

                    //bestelllingen
                    tabVerkoop.Visibility = Visibility.Collapsed;

                    //overzicht 
                    overzichtArr = new string[] { "Stock", "Producten", "Leveranciers"};
                    break;

                case "Verkoop":
                    //databeheer
                    rbDealer.Visibility = Visibility.Collapsed;
                    rbEmp.Visibility = Visibility.Collapsed;

                    //bestellingen
                    tabAankoop.Visibility = Visibility.Collapsed;
                    tabVerkoop.IsSelected = true;

                    //overzicht
                    overzichtArr = new string[] { "Stock", "Producten", "Klanten"};
                    break;

                case "Admin":
                case "Gamer":
                default:
                    break;
            }
        }
        private void ChangeColumns(string view)
        {
            switch (view)
            {
                case "Stock":
                    colID.DisplayMemberBinding = new Binding("Id");
                    h0.Tag = "Id";
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("Categorie");
                    h1.Tag = "Categorie";
                    col1.Header = "Cat.";
                    col1.Width = 150;
                    col2.DisplayMemberBinding = new Binding("Naam");
                    h2.Tag = "Naam";
                    col2.Header = "Naam";
                    col2.Width = 250;
                    col3.DisplayMemberBinding = new Binding("Aantal");
                    h3.Tag = "Aantal";
                    col3.Header = "Aantal";
                    col3.Width = 50;
                    col4.DisplayMemberBinding = new Binding("Eenheid");
                    h4.Tag = "Eenheid";
                    col4.Header = "Eenheid";
                    col4.Width = 60;
                    col5.DisplayMemberBinding = new Binding("Leverancier");
                    h5.Tag = "Leverancier";
                    col5.Header = "Leverancier";
                    col5.Width = 100;
                    col6.DisplayMemberBinding = new Binding("Verkocht");
                    h6.Tag ="Verkocht";
                    col6.Header = "Verkocht";
                    col6.Width = 50;
                    break;
                case "Producten":
                    colID.DisplayMemberBinding = new Binding("pc.p.Id");
                    h6.Tag = "";
                    colID.Width = 30;
                    col1.DisplayMemberBinding = new Binding("pc.Naam");
                    h6.Tag = "";
                    col1.Header = "Cat.";
                    col1.Width = 150;
                    col2.DisplayMemberBinding = new Binding("pc.p.Naam");
                    h6.Tag = "";
                    col2.Header = "Naam";
                    col2.Width = 250;
                    col3.DisplayMemberBinding = new Binding("pc.p.Inkoopprijs");
                    h6.Tag = "";
                    col3.Header = "Inkoopprijs";
                    col3.Width = 100;
                    col4.DisplayMemberBinding = new Binding("pc.p.Eenheid");
                    h6.Tag = "";
                    col4.Header = "Eenheid";
                    col4.Width = 60;
                    col5.DisplayMemberBinding = new Binding("pc.p.Marge");
                    h6.Tag = "";
                    col5.Header = "Marge";
                    col6.Width = 50;
                    col6.DisplayMemberBinding = new Binding("pc.p.BTW");
                    h6.Tag = "";
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
        private string Try(string text)
        {
            if (text.Length == 0)
            {
                errorText = "Sommige gegevens zijn niet ingevuld!";
                isError = true;
            }

            return text;
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
                toComboBox.ItemsSource = null;
                toComboBox.ItemsSource = ctx.Leveranciers.Select(l => l.Bedrijf).ToList();
                toComboBox.SelectedIndex = 0;
            }
        }
        private void RefreshCategoryList(ComboBox toComboBox)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                toComboBox.ItemsSource = null;
                toComboBox.ItemsSource = ctx.Subcategories.Select(l => l.Naam).ToList();
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
        private List<Line> RandomLines(bool test = false)
        {
            List<Line> myList = new List<Line>();
            Product prod = new Product();

            int count = 1;
            int select = 0;

            if (!test)
            {
                count = rand.Next(1, 5);
            }

            for (int i = 0; i < count; i++)
            {
                int qty = rand.Next(1, 10);

                using (MagazijnEntities ctx = new MagazijnEntities())
                {
                    List<Product> allproducts = ctx.Products.Select(x => x).ToList();
                    if (test)
                    {
                        select = 9;
                        qty = 2;
                    }
                    else
                    {
                        select = rand.Next(1, allproducts.Count);

                    }
                    prod = allproducts[select];
                }

                myList.Add(new Line(prod, qty));

            }

            return myList;
        }
        private void CreatePDF(Bestelling bestelling)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;
                RectangleF bounds = new RectangleF(0, 0, graphics.ClientSize.Width, 100);
                PdfBrush headerBrush = new PdfSolidBrush(new PdfColor());
                PdfBitmap headerImg = new PdfBitmap("Res/Bestelbon_header.jpg");
                DateTime date = (DateTime)bestelling.DatumOpgemaakt;

                string currentDate = $"Datum: {date.ToString("dd-MM-yyy")}";
                string currentStr = "";
                string btwH = "6%\n12%\n21%";
                string prijs = "";
                string btw = "";
                string totalH = "Totaal excl. BTW\nTotaal BTW\nTe betalen";
                string total = "";
                int marge = 20;

                //fonts 
                PdfFont fontH = new PdfStandardFont(PdfFontFamily.Helvetica, 18,PdfFontStyle.Bold);
                PdfFont fonth = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
                PdfFont fontn = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Regular);
                PdfFont fontB = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfFont fontN = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Regular);

                //Draw header
                graphics.DrawImage(headerImg, bounds);

                //Draw 'Bestelbon'
                graphics.DrawString("Bestelbon", fontH, PdfBrushes.Black, new PointF(0, 120));

                //draw header voor bestelnr en datum
                bounds = new RectangleF(0, bounds.Bottom + 60, graphics.ClientSize.Width, 24);
                graphics.DrawRectangle(PdfBrushes.Linen, bounds);
                PdfTextElement element = new PdfTextElement($"Bestelnr: {bestelling.Id}", fonth);
                element.Brush = PdfBrushes.Black;
                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 5));

                SizeF textSize = fonth.MeasureString(currentDate);
                PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);

                //Draw datum in header
                graphics.DrawString(currentDate, fonth, element.Brush, textPosition);

                //draw klantgegevens
                using (MagazijnEntities ctx = new MagazijnEntities())
                {
                    Klant klant = ctx.Klants.Where(x => x.Id == bestelling.IdKlant).FirstOrDefault();
                    currentStr =$"{klant.Bedrijf}\n{klant.Straatnaam} {klant.Huisnummer}{klant.Bus}\n{klant.Postcode} {klant.Gemeente}";
                }

                element = new PdfTextElement(currentStr, fontn);
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + marge));

                //Draw lijn onder klant
                PdfPen pen = new PdfPen(PdfBrushes.Black);
                PointF start = new PointF(0, result.Bounds.Bottom + 3);
                PointF end = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
                graphics.DrawLine(pen, start, end);

                PdfGrid grid = new PdfGrid();
                using (MagazijnEntities ctx = new MagazijnEntities())
                {
                    List<BestellingProduct> bpList = ctx.BestellingProducts.Where(x => x.IdBestelling == bestelling.Id).ToList();
                    Order myOrder = new Order();

                    foreach (BestellingProduct item in bpList)
                    {
                        Product product = ctx.Products.Where(x => x.Id == item.IdProduct).FirstOrDefault();
                        myOrder.LineList.Add(new Line(product, (int)item.Aantal));
                    }

                    PdfListView orderView = new PdfListView(myOrder);
                    grid.DataSource = orderView.ViewList;

                    prijs = $"{orderView.Price6}\n{orderView.Price12}\n{orderView.Price21}";
                    btw = $"{orderView.Tax6}\n{orderView.Tax12}\n{orderView.Tax21}";
                    total = $"{orderView.TotalPrice}\n{orderView.TotalTax}\n{orderView.TotalPrice + orderView.TotalTax}\n";
                    
                }


                //grid cell styles
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.All = PdfPens.White;
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
                PdfGridRow header = grid.Headers[0];
                //Creates the header style
                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

                //Adds cell customizations
                for (int i = 0; i < header.Cells.Count; i++)
                {
                    if (i == 0 || i == 1)
                    {
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    }
                    else
                    {
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    }
                }

                //Applies the header style
                header.ApplyStyle(headerStyle);
                //Creates the layout format for grid
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                // Creates layout format settings to allow the table pagination
                layoutFormat.Layout = PdfLayoutType.Paginate;
                //Draws the grid to the PDF page.
                PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);
                bounds = gridResult.Bounds;

                bounds = new RectangleF(0, bounds.Bottom + 60, 100, 100);
                element = new PdfTextElement(btwH, fontB);
                result = element.Draw(page, new PointF(10, bounds.Bottom + marge));
                element = new PdfTextElement(prijs, fontn);
                result = element.Draw(page, new PointF(70, bounds.Bottom + marge));
                element = new PdfTextElement(btw, fontn);
                result = element.Draw(page, new PointF(130, bounds.Bottom + marge));

                element = new PdfTextElement(totalH, fontB);
                result = element.Draw(page, new PointF(250, bounds.Bottom + marge));
                element = new PdfTextElement(total, fontn);
                result = element.Draw(page, new PointF(400, bounds.Bottom + marge));



                document.Save($"Bestellingen/bestelbon_{bestelling.IdLeverancier}{bestelling.Id}.pdf");
            }
        }
        private void CbxOverzicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                LvOverzicht.ItemsSource = null;

                switch (CbxOverzicht.SelectedItem.ToString())
                {
                    case "Stock":

                        ChangeColumns("Stock");
                        CbxSort.ItemsSource = sortStockDict.Keys;
                        LvOverzicht.ItemsSource = stockView.AllStock;
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

                    case "Personeel":
                        var persList = ctx.Personeelslids.Select(k => k).ToList();
                        CbxSort.ItemsSource = sortLeverKlantDict.Keys;
                        LvOverzicht.ItemsSource = persList;
                        ChangeColumns("Personeel");
                        break;

                    default:
                        break;
                }

            }
        }
        private void CbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(LvOverzicht.ItemsSource);

            switch (CbxSort.SelectedValue)
            {
                case "Id":
                    view1.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    break;
                case "naamUp":
                    view1.SortDescriptions.Add(new SortDescription("Naam", ListSortDirection.Ascending));
                    break;
                case "naamDown":
                    view1.SortDescriptions.Add(new SortDescription("Naam", ListSortDirection.Descending));
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
            btnAddTemplate.Visibility = Visibility.Collapsed;

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
            btnAddTemplate.Visibility = Visibility.Collapsed;

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
            RefreshDealerList(CbxDealer);
            RefreshCategoryList(CbxCat);

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
            btnAddTemplate.Visibility = Visibility.Visible;

            LblRemark.Visibility = Visibility.Hidden;
            TxtRemark.Visibility = Visibility.Hidden;
            LblDate.Visibility = Visibility.Hidden;
            LblCity.Visibility = Visibility.Hidden;
            TxtCity.Visibility = Visibility.Hidden;
            TxtEmail.Visibility = Visibility.Collapsed;
            TxtTel.Visibility = Visibility.Collapsed;
            DpDate.Visibility = Visibility.Collapsed;
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
            btnAddTemplate.Visibility = Visibility.Collapsed;

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
            btnAddTemplate.Visibility = Visibility.Collapsed;

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

                            klant.Bedrijf = Try(TxtName.Text);
                            klant.Straatnaam = Try(TxtStreet.Text);
                            klant.Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect Huisnummer");
                            klant.Bus = TxtBus.Text;
                            klant.Postcode = Try(TxtPostal.Text);
                            klant.Gemeente = Try(TxtCity.Text);
                            klant.Emailadres = Try(TxtEmail.Text);
                            klant.Telefoonnummer = Try(TxtTel.Text);
                            klant.Opmerking = TxtRemark.Text;
                            klant.AangemaaktOp = DpDate.SelectedDate;

                        }
                        else
                        {
                            ctx.Klants.Add(new Klant()
                            {
                                Bedrijf = Try(TxtName.Text),
                                Straatnaam = Try(TxtStreet.Text),
                                Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect huisnummer!"),
                                Bus = TxtBus.Text,
                                Postcode = Try(TxtPostal.Text),
                                Gemeente = Try(TxtCity.Text),
                                Emailadres = Try(TxtEmail.Text),
                                Telefoonnummer = Try(TxtTel.Text),
                                Opmerking = TxtRemark.Text,
                                AangemaaktOp = DpDate.SelectedDate
                            });
                        }
                        LvOverzicht.ItemsSource = null;
                        stockView = new StockView();
                        LvOverzicht.ItemsSource = stockView.AllStock;
                        break;

                    case "Leverancier":
                        //Bewerken of nieuwe leverancier
                        if (toChange)
                        {
                            dealer = ctx.Leveranciers.Where(k => k.Id == myId).FirstOrDefault();

                            dealer.Bedrijf = Try(TxtName.Text);
                            dealer.Straatnaam = Try(TxtStreet.Text);
                            dealer.Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect Huisnummer");
                            dealer.Bus = TxtBus.Text;
                            dealer.Postcode = Try(TxtPostal.Text);
                            dealer.Gemeente = Try(TxtCity.Text);
                            dealer.Emailadres = Try(TxtEmail.Text);
                            dealer.Telefoonnummer = Try(TxtTel.Text);

                        }
                        else
                        {
                            ctx.Leveranciers.Add(new Leverancier()
                            {
                                Bedrijf = Try(TxtName.Text),
                                Straatnaam = Try(TxtStreet.Text),
                                Huisnummer = ConvertToInt(TxtNumber.Text, "Incorrect huisnummer!"),
                                Bus = TxtBus.Text,
                                Postcode = Try(TxtPostal.Text),
                                Gemeente = Try(TxtCity.Text),
                                Emailadres = Try(TxtEmail.Text),
                                Telefoonnummer = Try(TxtTel.Text)
                            });
                        }
                        break;

                    case "Product":
                        //Bewerken of nieuwe product
                        if (toChange)
                        {
                            product = ctx.Products.Where(k => k.Id == myId).FirstOrDefault();

                            product.Naam = Try(TxtName.Text);
                            product.Inkoopprijs = ConvertToDecimal(TxtStreet.Text, "Inkoopprijs niet correct ingegeven");
                            product.Marge = ConvertToInt(TxtNumber.Text, "Incorrecte marge ingegeven");
                            product.BTW = ConvertToInt(TxtBus.Text, "BTW niet juist ingegeven");
                            product.Eenheid = Try(TxtPostal.Text);
                            product.IdLeverancier = dealer.Id;
                            product.IdSubcategorie = (int)CbxCat.SelectedValue;

                        }
                        else
                        {
                            ctx.Products.Add(new Product()
                            {
                                Naam = Try(TxtName.Text),
                                Inkoopprijs = ConvertToDecimal(TxtStreet.Text, "Inkoopprijs ongeldig!"),
                                Marge = ConvertToDecimal(TxtNumber.Text, "Marge ongeldig!"),
                                BTW = ConvertToDecimal(TxtBus.Text, "BTW ongeldig!"),
                                Eenheid = Try(TxtPostal.Text),
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

                            person.Voornaam = Try(TxtName.Text);
                            person.Achternaam = Try(TxtStreet.Text);
                            person.Login = Try(TxtNumber.Text);
                            person.Wachtwoord = Try(TxtBus.Text);
                            person.Afdeling = CbxFunction.SelectedItem.ToString();

                        }
                        else
                        {
                            ctx.Personeelslids.Add(new Personeelslid()
                            {
                                Voornaam = Try(TxtName.Text),
                                Achternaam = Try(TxtStreet.Text),
                                Login = Try(TxtNumber.Text),
                                Wachtwoord = Try(TxtBus.Text),
                                Afdeling = CbxFunction.SelectedItem.ToString()
                            });
                        }
                        break;

                    case "Categorie":
                        //Bewerken of nieuwe categorie
                        if (toChange)
                        {
                            cat = ctx.Subcategories.Where(k => k.Id == myId).FirstOrDefault();
                            cat.Naam = Try(TxtCity.Text);
                        }
                        else
                        {
                            ctx.Subcategories.Add(new Subcategorie()
                            {
                                Naam = Try(TxtCity.Text)
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
                    errorText = "";
                }

                isError = false;
            }
            ClearAllText();
            CbxOverzicht.SelectedIndex = 0;

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
            if (orderOUT.LineList.Count != 0 && cbAankoopBij.SelectedIndex != prevIndex)
            {
                MessageBoxResult result = MessageBox.Show("Bestelling zal gewist worden!", "Confirmatie", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    orderOUT.LineList.Clear();
                    LvOverzichtAankoop2.ItemsSource = null;
                }
                else
                {
                    cbAankoopBij.SelectedIndex = prevIndex;
                }
            }

            txtTotal.Text = $"€ {orderOUT.GetTotal()}";

            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                Leverancier myDealer = ctx.Leveranciers.Where(d => d.Bedrijf == (string)cbAankoopBij.SelectedItem).FirstOrDefault();

                LvOverzichtAankoop1.ItemsSource = ctx.Products.Where(l => l.IdLeverancier == myDealer.Id).ToList();
            }

            prevIndex = cbAankoopBij.SelectedIndex;
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

            orderOUT.LineList.Add(new Line(product, windowQuantity.Quantity));
            txtTotal.Text = $"€ {orderOUT.GetTotal()}";

            LvOverzichtAankoop2.ItemsSource = null;
            LvOverzichtAankoop2.ItemsSource = orderOUT.LineList;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            WindowChange windowDelete = new WindowChange();
            windowDelete.Selector = selection;
            windowDelete.toChange = false;
            windowDelete.Owner = this;
            windowDelete.ShowDialog();

            myId = windowDelete.thisId;

            if (windowDelete.DialogResult == true)
            {
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
                    MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit element wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        ctx.SaveChanges();
                    }
                }
            }

        }
        private void btnDeleteLine_Click(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            Line line = (Line)btn.DataContext;

            orderOUT.LineList.Remove(line);
            txtTotal.Text = $"€ {orderOUT.GetTotal()}";

            LvOverzichtAankoop2.ItemsSource = null;
            LvOverzichtAankoop2.ItemsSource = orderOUT.LineList;
        }
        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {
            string name = cbAankoopBij.SelectedItem.ToString();

            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                Leverancier mydealer = ctx.Leveranciers.Where(x => x.Bedrijf == name).FirstOrDefault();

                // bestelling aanmaken in DB
                Bestelling myOrder = new Bestelling() { DatumOpgemaakt = DateTime.Now, IdLeverancier = mydealer.Id, IdPersoneelslid = User.Id };
                ctx.Bestellings.Add(myOrder);
                ctx.SaveChanges();

                // controleren of product al dan niet in stock is
                List<Stock> myStock = ctx.Stocks.Select(x => x).ToList();

                foreach (Line line in orderOUT.LineList)
                {
                    bool inStock = false;

                    ctx.BestellingProducts.Add(new BestellingProduct() { IdBestelling = myOrder.Id, IdProduct = line.Product.Id, Aantal = line.Quantity });

                    foreach (Stock item in myStock)
                    {
                        if (line.Product.Id == item.IdProduct)
                        {
                            //Match gevonden, aantal bij stock tellen
                            item.Aantal += line.Quantity;
                            inStock = true;

                            break;
                        }
                    }

                    // wanneer niet in stock: nieuwe entry
                    if (!inStock)
                    {
                        ctx.Stocks.Add(new Stock() { IdProduct = line.Product.Id, Aantal = line.Quantity, Verkocht = 0 });
                    }
                }

                // opslaan
                ctx.SaveChanges();
                MessageBox.Show("Bestelling geplaatst!");
                stockView = new StockView();
                LvOverzicht.ItemsSource = null;
                LvOverzicht.ItemsSource = stockView.AllStock;
                orderOUT.LineList.Clear();
                LvOverzichtAankoop2.ItemsSource = null;
                txtTotal.Text = $"€ {orderOUT.GetTotal()}";

            }
        }
        private void AcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Order geaccepteerd!");

            //
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedOrder = sender as ListViewItem;

            if (selectedOrder != null || selectedOrder.IsSelected)
            {
                LvOverzichtVerkoop.ItemsSource = (selectedOrder.Content as Order).LineList;
            }

        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                // Controle tegenover stock
                List<Stock> stock = ctx.Stocks.Select(x => x).ToList();
                bool canSell = false;
                string errorStock = "";

                foreach (Line line in (selectedOrder.Content as Order).LineList)
                {
                    foreach (Stock prod in stock)
                    {
                        if (prod.IdProduct == line.Product.Id)
                        {
                            if (prod.Aantal > line.Quantity)
                            {
                                canSell = true;
                            }
                            else
                            {
                                errorStock += $"- {line.Product.Naam}\n";
                                canSell = false;
                            }
                            break;
                        }
                    }

                    if (!canSell)
                    {
                        errorStock += $"- {line.Product.Naam}\n";
                    }
                }

                if (errorStock == "")
                {
                    // toevoegen aan Bestelling-table
                    string company = (lvOrders.SelectedItem as Order).OrderedBy;

                    int id = ctx.Klants.Where(x => x.Bedrijf == company).Select(x => x.Id).FirstOrDefault();
                    Bestelling myOrder = new Bestelling() { DatumOpgemaakt = DateTime.Now, IdPersoneelslid = User.Id, IdKlant = id };
                    ctx.Bestellings.Add(myOrder);
                    ctx.SaveChanges();

                    // toevoegen aan BestellingProduct en stock aanpassen
                    foreach (Line line in (selectedOrder.Content as Order).LineList)
                    {
                        ctx.BestellingProducts.Add(new BestellingProduct() { IdBestelling = myOrder.Id, IdProduct = line.Product.Id, Aantal = line.Quantity });
                        Stock myStock = ctx.Stocks.Where(x => x.IdProduct == line.Product.Id).FirstOrDefault();

                        myStock.Aantal -= line.Quantity;
                        myStock.Verkocht += line.Quantity;
                    }

                    // opslaan
                    ctx.SaveChanges();

                    CreatePDF(myOrder);
                    // verwijderen uit orderlijst en overzicht leegmaken
                    orderList.Remove((Order)lvOrders.SelectedItem);
                    LvOverzichtVerkoop.ItemsSource = null;

                    // orderoverzicht refreshen
                    lvOrders.ItemsSource = null;
                    lvOrders.ItemsSource = orderList;

                }
                else
                {
                    MessageBox.Show($"Niet op voorraad:\n{errorStock}");
                }

            }


            LvOverzicht.ItemsSource = null;
            StockView stockView2 = new StockView();
            LvOverzicht.ItemsSource = stockView2.AllStock;

        }

        private void btnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text files (*.txt)|*.txt";

            string name = "";
            decimal price = 0;
            if (openFile.ShowDialog() == true)
            {
                string[] data = File.ReadAllLines(openFile.FileName);

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Contains("Productnaam"))
                    {
                        int index = data[i].IndexOf(':') + 1;
                        name = data[i].Substring(index, data[i].Length - index).Trim(' ');
                        MessageBox.Show(name);
                    }
                    if (data[i].Contains("Nieuwe prijs"))
                    {
                        int index = data[i].IndexOf(':') + 1;
                        price =ConvertToDecimal(data[i].Substring(index, data[i].Length - index).Trim(' '),"Prijs is niet correct!");
                    }
                }

                using (MagazijnEntities ctx = new MagazijnEntities())
                {
                    Product prod = ctx.Products.Where(x => x.Naam == name).FirstOrDefault();
                    prod.Inkoopprijs = price;
                    ctx.SaveChanges();
                }
                MessageBox.Show("Product gewijzigd!");
            }
        }
        private void lvOverzichtOrdersHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                lvOverzichtOrders.Items.SortDescriptions.Clear();
            }
            
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column )
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            lvOverzichtOrders.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void Header_Click(object sender, RoutedEventArgs e)
        {

            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            MessageBox.Show("entered");
            if (listViewSortCol1 != null)
            {
                LvOverzicht.Items.SortDescriptions.Clear();
                MessageBox.Show("null");
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol1 == column)
            {
                newDir = ListSortDirection.Descending;
                MessageBox.Show("sorted");
            }

            listViewSortCol1 = column;
            LvOverzicht.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
