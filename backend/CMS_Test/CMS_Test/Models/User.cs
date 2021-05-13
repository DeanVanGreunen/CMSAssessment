using System;

namespace CMS_Test.Models
{
    public class User
    {
        public int ID { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }
        public String SessionKey { get; set; }

    }
}
