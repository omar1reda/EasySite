using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum TypeUser
    {
        [EnumMember(Value = "Basic")]
        Basic,
        [EnumMember(Value = "FreeTrial")]
        FreeTrial,
        [EnumMember(Value = "Pro")]
        Pro
    }
}
