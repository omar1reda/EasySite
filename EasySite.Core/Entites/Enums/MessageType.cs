using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum MessageType
    {
        [EnumMember(Value = "Text")]
        Text,
        [EnumMember(Value = "File")]
        File,
        [EnumMember(Value = "Voice")]
        Voice
    }
}
