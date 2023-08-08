using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonDistributionSystem.Models
{
    public class CartModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string UserName { get; set; }

        public string Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}