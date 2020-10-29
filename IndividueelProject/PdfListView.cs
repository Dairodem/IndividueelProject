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
                }
            }

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
