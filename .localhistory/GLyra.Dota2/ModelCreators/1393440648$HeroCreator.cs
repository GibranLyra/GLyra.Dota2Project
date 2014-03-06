using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
    public class HeroCreator
    {
        public Hero createHero(string heroName, string biography)
        {
            Hero hero = new Hero();

            hero.Name = heroName;
            hero.Biography = biography;

            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    ctx.Hero.Add(hero);

                    ctx.SaveChanges();
                    Console.WriteLine("*************************** " + heroName + " Created(Hero)" + "***************************");
                }
                catch (Exception e)
                {
                    //TODO Adicionar ao log
                    throw e;
                }
            }

            return hero;
        }

        public Skill createHeroSkill(int heroId, string name, string description, List<KeyValuePair<string, string>> manaCostList, List<KeyValuePair<string, string>> coolDownList,
            List<KeyValuePair<string, string>> abilityCastTypeList, List<KeyValuePair<string, string>> targetAffectedTypeList, List<KeyValuePair<string, string>> damageTypeList, string videoUrl)
        {
            SkillCreator sCreator = new SkillCreator();
            return sCreator.createSkill(heroId, name, description, manaCostList, coolDownList, abilityCastTypeList, targetAffectedTypeList, damageTypeList, videoUrl);
        }

        public void createHeroPrimaryStats(Dictionary<string, string> primaryStats)
        {
            AttributesCreator aCreator = new AttributesCreator(heroId, primaryStats);
            
        }
    }
}
