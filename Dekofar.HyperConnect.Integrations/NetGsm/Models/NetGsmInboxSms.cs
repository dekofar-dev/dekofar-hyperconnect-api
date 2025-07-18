using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Integrations.NetGsm.Models
{
    public class NetGsmInboxSms
    {
        public string msgid { get; set; }
        public string orjinator { get; set; }
        public string msisdn { get; set; }
        public string datetime { get; set; }
        public string msg { get; set; }
    }

}
