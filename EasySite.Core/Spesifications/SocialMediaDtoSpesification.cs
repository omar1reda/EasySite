using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class SocialMediaDtoSpesification:Spesification<SocialMedia>
    {
        public SocialMediaDtoSpesification(int siteId) : base(s=>s.SiteId == siteId) 
        {
            //Includes.Add(s => s.Site);
        }

    }
}
