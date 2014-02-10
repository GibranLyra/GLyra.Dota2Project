using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class HeroCreator
    {
        Hero hero;
        AttributesCreator attributesCreator;
        
        public Hero createHero(string name, List<string> primaryStats, string biography)
        {
            this.hero = new Hero();

            this.hero.Name = name;
            this.hero.Biography = biography;

            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    ctx.Hero.Add(this.hero);

                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    //TODO Adicionar ao log
                    throw new e.Message;
                }
            }

            return this.hero;
        }
        
    }
}
