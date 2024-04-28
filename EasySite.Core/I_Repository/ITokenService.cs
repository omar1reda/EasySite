using EasySite.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.I_Repository
{
    public interface ITokenService
    {
        Task<string> GetToken(AppUser  user);
    }
}
