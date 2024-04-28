using EasySite.Core.Entites.SittingFormOrder;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasySite.DTOs.HomePage.ToReturnEndUser
{
    public class DepartmentsToReturnEndUser
    {
        public int Index { get; set; }
        public string TypeItem { get; set; }
        public List<DepartmentToreturnDto> DepartmentToreturnDto { get; set; } = new List<DepartmentToreturnDto>();
    }
}
