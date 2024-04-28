using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum FreeTrial
    {
        [EnumMember(Value = "Didnt Start")]
        DidntStart,
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Finish")]
        Finish
    }
}
