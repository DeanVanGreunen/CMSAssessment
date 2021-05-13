using System;
using System.Collections.Generic;

namespace CMS_Test.Models
{
    public class Stock
    {
        public int ID { get; set; }
        public String RegNumber { get; set; }
        public String Make { get; set; }
        public String Model { get; set; }
        public int ModelYear { get; set; }
        public int KMS { get; set; }
        public String Colour { get; set; }
        public String VIN { get; set; }
        public int RetailPrice { get; set; }
        public int CostPrice { get; set; }
        public List<Accessory> Accessories { get; set; }
        public List<Image> Images { get; set; }
        public DateTime DTCreated { get; set; }
        public DateTime DTUpdated { get; set; }
    }
}
