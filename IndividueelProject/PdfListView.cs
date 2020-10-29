using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace IndividueelProject
{
    public class PdfListView
    {
        public List<Entry> ViewList { get; }

        public decimal Price6 { get; set; }
        public decimal Price12 {get;set;}
        public decimal Price21 {get;set; }
        public decimal Tax6 { get; set; }
        public decimal Tax12 { get; set; }
        public decimal Tax21 { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }

        public PdfListView(Order order)
        {
            ViewList = new List<Entry>();

            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                foreach (Line line in order.LineList)
                {
                    string cat = ctx.Subcategories.Where(x => x.Id == line.Product.IdSubcategorie).Select(x => x.Naam).FirstOrDefault();
                    decimal price = (decimal)(line.Product.Inkoopprijs + line.Product.Marge);
                    ViewList.Add(new Entry(line.Product.Naam, cat,price,line.Quantity,(price*line.Quantity)));
                    SortByTax(price, line.Quantity, (decimal)line.Product.BTW);
                }
            }
            GetTaxes();
            TotalPrice = GetTotalPrice();
            TotalTax = GetTotalTax();


        }
        private void SortByTax(decimal price,int qty, decimal tax)
        {
            switch ((int)tax)
            {
                case 6:
                    Price6 += price*qty;
                    break;
                case 12:
                    Price12 += price*qty;
                    break;
                case 21:
                    Price21 += price*qty;
                    break;
                default:
                    break;
            }
        }
        private void GetTaxes()
        {
            Tax6 = Price6 * (decimal)0.06;
            Tax12 = Price12 * (decimal)0.12;
            Tax21 = Price21 * (decimal)0.21;
        }
        private decimal GetTotalPrice()
        {
            return Price6 + Price12 + Price21;
        }
        private decimal GetTotalTax()
        {
            return Tax6 + Tax12 + Tax21;
        }

    }
    public class Entry
    {
        public string Product { get; set; }
        public string Categorie { get; set; }
        public decimal Eenheidsprijs { get; set; }
        public int Aantal { get; set; }
        public decimal Totaal { get; set; }

        public Entry(string product, string categorie, decimal eenheidsprijs, int aantal, decimal totaal)
        {
            Product = product;
            Categorie = categorie;
            Eenheidsprijs = eenheidsprijs;
            Aantal = aantal;
            Totaal = totaal;
        }
    }
}
