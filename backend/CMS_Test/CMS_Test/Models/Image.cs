using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Test.Models
{
    public class Image
    {
        public int ID { get; set; }
        public int StockID { get; set; }
        public String Name { get; set; }
        public  String Data { get; set; } // Stored as Base64
    }
}
