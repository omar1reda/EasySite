using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Order
{
    public class Email
    {
        public string email { get; set; }
        public bool IsEmail { get; set; }
        public int Index { get; set; }
        public bool IsImportant { get; set; }
        public string Text_Important { get; set; }
    }
}
