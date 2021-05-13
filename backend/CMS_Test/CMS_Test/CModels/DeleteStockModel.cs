using CMS_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMS_Test.CModels
{
    public sealed class DeleteStockModel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }
    }

}
