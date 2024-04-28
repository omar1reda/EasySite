using EasySite.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Helper.SendEmail
{
    public interface IMailSettings
    {
        public void SendEmail(Email email);
    }
}
