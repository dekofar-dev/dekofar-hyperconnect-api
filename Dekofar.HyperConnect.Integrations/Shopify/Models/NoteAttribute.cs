using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.Shopify.Models
{
    public class NoteAttribute
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }
    }
}
