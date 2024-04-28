using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Enums
{
    public enum StatusOrder
    {
        [EnumMember(Value = "قيد المراجعه")]
        UnderReview,

        [EnumMember(Value = "قيد المعالجه")]
        UnderProcessing,

        [EnumMember(Value = "قيد التوصيل")]
        UnderDelivery,

              [EnumMember(Value = "تم التوصيل")]
        Delivered,
              [EnumMember(Value = "تم إلغاء الطلب")]
        Canceled
    }
}
