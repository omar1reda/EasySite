using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum FontType
    {
        [EnumMember(Value = "Almarai")]
        Almarai,
        [EnumMember(Value = "Tajawal")]
        Tajawal,
        [EnumMember(Value = "IB MPlex Sans Arabic")]
        IBMPlexSansArabic,
        [EnumMember(Value = "El Messiri")]
        ElMessiri,
        [EnumMember(Value = "Mada")]
        Mada,
        [EnumMember(Value = "Readex Pro")]
        ReadexPro,
        [EnumMember(Value = "Changa")]
        Changa

    }
}
