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
        
        public Hero createHero(string name, string biography)
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
                    throw e;
                }
            }

            return this.hero;
        }
        

        public void createHeroSkill(string name, string description, List<string> manaCostList, List<string> coolDownList,
            string abilityCastType, string targetAffectedType, string damageType, string videoUrl)
        {
            SkillCreator sCreator = new SkillCreator();
            fsCreator.createSkill(this.hero.ID, name, description, manaCostList, coolDownList, abilityCastType, targetAffectedType, damageType, videoUrl);
        }

        public void createHeroPrimaryStats(List<string> primaryStats)
        {
            AttributesCreator aCreator = new AttributesCreator(this.hero.ID, primaryStats);
            
        }
    }
}
