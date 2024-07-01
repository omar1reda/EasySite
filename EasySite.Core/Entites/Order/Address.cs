using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Order
{
    public class Address
    {
        public string address { get; set; }
        public bool IsAddress { get; set; }
        public int Index { get; set; }
        public bool IsImportant { get; set; }
        public string Text_Important { get; set; }
    }
}
