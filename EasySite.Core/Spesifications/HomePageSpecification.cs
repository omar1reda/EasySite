using Core.Entites.Homepage;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class HomePageSpecification:Spesification<Homepage>
    {
        public HomePageSpecification(int siteId):base(h=>h.SiteId== siteId)
        {
            Includes.Add(h=>h.DepartmentsInHomePages);
            Includes.Add(h => h.ProductsInHomePage);
            Includes.Add(h => h.Sliders);
            IncludeStrings.Add("Sliders.SliderImage");
        }
    }
}
