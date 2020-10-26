using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace IndividueelProject
{
    public class Line
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public Line(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Total = (decimal)(Product.Inkoopprijs * Quantity);
        }
        public Line(Product product)
        {
            Product = product;
            Quantity = 10;
            Total = (decimal)(Product.Inkoopprijs * Quantity);
        }
    }
}
