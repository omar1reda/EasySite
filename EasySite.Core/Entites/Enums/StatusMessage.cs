using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum StatusMessage
    {
        [EnumMember(Value = "تم الارسال")]
        Been_Sent,
        [EnumMember(Value = "تم الاستلام")]
        Was_Delivered,
        [EnumMember(Value = "تم المشاهده")]
        Was_Seen
    }
}
