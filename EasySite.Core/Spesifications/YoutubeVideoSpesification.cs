using EasySite.Core.Entites.Admin;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class YoutubeVideoSpesification:Spesification<YoutubeVideo>
    {
        public YoutubeVideoSpesification(string videoName):base(y=>y.VideoName==videoName)
        {
            
        }
    }
}
