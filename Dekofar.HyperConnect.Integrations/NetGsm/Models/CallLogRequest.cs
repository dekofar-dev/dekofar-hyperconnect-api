using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Models
{
    public class CallLogRequest
    {
        public string StartDate { get; set; } = string.Empty; // 01072025 gibi formatta
        public string EndDate { get; set; } = string.Empty;   // 13072025 gibi formatta
    }
}
