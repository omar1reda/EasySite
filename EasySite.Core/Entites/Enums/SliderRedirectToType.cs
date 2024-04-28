using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum SliderRedirectToType
    {
        [EnumMember(Value = "Department")]
        Department,
        [EnumMember(Value = "Product" )]
        Product,
        [EnumMember(Value = "Page")]
        Page,
        [EnumMember(Value = "NoType")]
        NoType,

    }
}
