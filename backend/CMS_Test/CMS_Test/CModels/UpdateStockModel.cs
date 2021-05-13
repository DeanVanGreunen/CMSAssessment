using CMS_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMS_Test.CModels
{
    public sealed class UpdateStockModel
    {
        [JsonPropertyName("stock")]
        public Stock Stock { get; set; }

        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }
    }

}
