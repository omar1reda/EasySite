using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum currency
    {
        [EnumMember(Value = "جنيه مصري - ج.م")]
        EgyptianPound,
        [EnumMember(Value = "دولار - USD")]
        Dollar,
        [EnumMember(Value = "يورو - EUR")]
        Euro,
        [EnumMember(Value = "ريال سعودي - SAR")]
        SR,
        [EnumMember(Value = "درهم مغربي - MAD")]
        MoroccanDirham,
        [EnumMember(Value = "دينار جزائري - DZD")]
        AlgerianDinar,
        [EnumMember(Value = "دينار تونسي - TND")]
        TunisianDinar


    }
}
