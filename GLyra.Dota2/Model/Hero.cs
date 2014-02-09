using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota.CentralDota.Model
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get; set; }

        public string TinyHorizontalPortrait { get; set; }
        public string SmallHorizontalPortrait { get; set; }
        public string LargeHorizontalPortrait { get; set; }
        public string FullQualityHorizontalPortrait { get; set; }
        public string FullQualityVerticalPortrait { get; set; }        


    }
}
