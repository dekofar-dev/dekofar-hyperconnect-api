using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Models
{
    public class CallLogResponse
    {
        public string Tarih { get; set; }       // örn: "2025-07-01 14:33:22"
        public string ArayanNo { get; set; }    // örn: "0850xxxxxxx"
        public string ArananNo { get; set; }    // örn: "0532xxxxxxx"
        public int Sure { get; set; }           // örn: 60 (saniye)
        public string Yön { get; set; }         // "Giden" / "Gelen"
                                                // Gerekirse diğer alanları da ekle
    }
}
