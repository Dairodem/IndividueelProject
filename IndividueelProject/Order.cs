using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividueelProject
{
    public class Order
    {
        public List<Line> LineList { get; set; }

        public Order()
        {
            LineList = new List<Line>();
        }
    }
}
