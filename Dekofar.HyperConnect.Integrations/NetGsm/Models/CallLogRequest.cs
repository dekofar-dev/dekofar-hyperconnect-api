using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Models
{
    public class CallLogRequest
    {
        public string Date { get; set; } // Örn: "240720251200"
        public int Direction { get; set; } // 1: gelen, 2: giden, 3: her ikisi (dökümana göre)
    }

}
