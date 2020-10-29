using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividueelProject
{
    class OrdersView
    {
        public List<OrderLine> AllOrders { get; set; }

        public OrdersView()
        {
            AllOrders = new List<OrderLine>();
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                List<Bestelling> bestellingen = ctx.Bestellings.Select(x => x).ToList();
                string sign = "";

                foreach (Bestelling order in bestellingen)
                {
                    string direction = "";
                    string name = "";
                    decimal total = 0;
                    DateTime date = (DateTime)order.DatumOpgemaakt;
                    List<BestellingProduct> bp = ctx.BestellingProducts.Where(x => x.IdBestelling == order.Id).ToList();

                    // totale prijs berekenen
                    foreach (BestellingProduct item in bp)
                    {
                        Product prod = ctx.Products.Where(x => x.Id == item.IdProduct).FirstOrDefault();

                        if (order.IdLeverancier == null)
                        {
                            total += GetTotal((decimal)prod.Inkoopprijs, (decimal)prod.Marge, (decimal)prod.BTW, (int)item.Aantal);
                        }
                        else if (order.IdKlant == null)
                        {
                            total += (decimal)(prod.Inkoopprijs*item.Aantal);
                        }
                    }

                    // naam van bedrijf selecteren
                    if (order.IdLeverancier == null)
                    {
                        direction = "IN";
                        sign = "+";
                        name = ctx.Klants.Where(x => x.Id == order.IdKlant).Select(x => x.Bedrijf).FirstOrDefault();
                    }
                    else if (order.IdKlant == null)
                    {
                        direction = "OUT";
                        sign = "-";
                        name = ctx.Leveranciers.Where(x => x.Id == order.IdLeverancier).Select(x => x.Bedrijf).FirstOrDefault();
                    }

                    // toevoegen aan orderlijst
                    AllOrders.Add(new OrderLine() { Bedrijf = name, Datum = date.ToString("dd-MM-yyyy"),Totaal = total, InOut = direction });
                }
            }

        }
        private decimal GetTotal(decimal price, decimal profit, decimal tax, int qty)
        {
            return ((price + profit) + ((price + profit) * (tax / 100))) * qty;
        }
    }
    public class OrderLine
    {

        public string Bedrijf { get; set; }
        public string Datum { get; set; }
        public decimal Totaal { get; set; }
        public string InOut { get; set; }

        public OrderLine()
        {

        }
    }
}
