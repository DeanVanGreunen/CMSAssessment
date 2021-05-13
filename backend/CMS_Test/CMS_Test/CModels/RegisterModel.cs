using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMS_Test.CModels
{
    public sealed class RegisterModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("confirmpassword")]
        public string ConfirmPassword { get; set; }
    }

}
