using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Models
{
    public class SmsInboxRequest
    {
        public string StartDate { get; set; } // Format: yyyyMMddHHmm
        public string StopDate { get; set; }  // Format: yyyyMMddHHmm
    }
}
