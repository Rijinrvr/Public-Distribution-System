using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonDistributionSystem.Models
{
    public class ProductsModel
    {
        public int Id { get; set; }

        public string Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}