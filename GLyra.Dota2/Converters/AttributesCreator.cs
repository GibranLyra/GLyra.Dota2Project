using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class AttributesCreator
    {
        Attributes attribute;

        public AttributesCreator(int heroId, Dictionary<string, string> primaryStats)
        {


            attribute = new Attributes();
            attribute.HeroId = heroId;
            attribute.Intelligence = primaryStats["Intelligence"];
            attribute.Agility = primaryStats["Agility"];
            attribute.Strength = primaryStats["Strength"];
            attribute.Damage = primaryStats["Damage"];
            attribute.MoveSpeed = primaryStats["Movespeed"];
            attribute.Armor = primaryStats["Armor"];

            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    ctx.Attributes.Add(this.attribute);

                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    //TODO Adicionar ao log
                    throw e;
                }
            }
        }
    }
}
