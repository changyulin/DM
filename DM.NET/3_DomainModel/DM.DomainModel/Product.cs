using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.Domain;

namespace DM.DomainModel
{
    public class Product: EntityBase
    {
        public string ProductID { get; set; }
        public string BrandID { get; set; }
        public string Brand { get; set; }
        public string CategoryID { get; set; }
        public string Category { get; set; }
        public string ProductSize { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductColor { get; set; }
    }
}
