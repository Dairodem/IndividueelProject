using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividueelProject
{
    public class StockView
    {
        public List<StockLine> AllStock { get; set; }

        public StockView()
        {
            AllStock = new List<StockLine>();

            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                //List<Stock> stockList = ctx.Stocks.Select(x => x).ToList();

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

                foreach (var item in stockList)
                {
                    AllStock.Add(new StockLine() 
                    {
                        Id = item.ps2.ps.s.Id, 
                        Categorie = item.ps2.Naam, 
                        Naam = item.ps2.ps.p.Naam, 
                        Aantal = (int)(item.ps2.ps.s.Aantal), 
                        Eenheid = item.ps2.ps.p.Eenheid, 
                        Leverancier = item.l.Bedrijf,
                        Verkocht = (int)item.ps2.ps.s.Verkocht
                    });
                }
            }
        }
    }
    public class StockLine
    {
        public int Id { get; set; }
        public string Categorie { get; set; }
        public string Naam { get; set; }
        public int Aantal { get; set; }
        public string Eenheid { get; set; }
        public string Leverancier { get; set; }
        public int Verkocht { get; set; }
        public StockLine()
        {

        }
    }
}
