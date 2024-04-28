using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Admin
{
    public class YoutubeVideo:BaseEntites
    {
        [Required]
        public string VideoName { get; set; }
        [Required]
        public string UrlVideo { get; set; }
        [Required]
        public string DescriptionVideo { get; set; }
    }
}
