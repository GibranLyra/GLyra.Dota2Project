using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class HeroCreator
    {
        Hero hero;
        AttributesCreator attributesCreator;

        public HeroCreator(string name, List<string> primaryStats, string biography)
        {
            this.hero = new Hero();
            

            this.hero.Name = name;
            this.hero.Biography = biography;
            this.hero.Skill = 

            this.attributesCreator = new AttributesCreator(1, primaryStats);
            

            
        }

        
    }
}
