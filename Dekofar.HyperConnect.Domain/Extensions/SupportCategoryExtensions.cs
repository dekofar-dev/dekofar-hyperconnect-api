using Dekofar.HyperConnect.Domain.Entities.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Domain.Extensions
{
    public static class SupportCategoryExtensions
    {
        public static string GetCode(this SupportCategory category) => category switch
        {
            SupportCategory.Iade => "IAD",
            SupportCategory.Degisim => "DGS",
            SupportCategory.EksikUrun => "EKU",
            SupportCategory.KargoSorunu => "KRG",
            SupportCategory.OdemeSorunu => "ODM",
            SupportCategory.GenelBilgi => "GEN",
            SupportCategory.Diger => "DGR",
            SupportCategory.Garanti => "GRN",
            _ => "GEN"
        };
    }
}
