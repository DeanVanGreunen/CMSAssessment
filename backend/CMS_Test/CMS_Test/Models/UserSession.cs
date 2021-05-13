using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CMS_Test.Models
{
    public class UserSession
    {
        public bool Success { get; set; }
        public String Message { get; set; }
        public String SessionKey { get; set; }

    }
}
