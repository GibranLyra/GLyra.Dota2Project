﻿using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
    public class SkillEffectNameCreator
    {
        /// <summary>
        /// Checks if the description of the skill exists
        /// </summary>
        protected bool checkIfSkillNameExists(string name, Skill skill, string skillDescription, out SkillEffectName skillEffectName)
        {
            using (Dota2Entities ctx = new Dota2Entities())
            {
                skillEffectName = new SkillEffectName();

                try
                {
                    skill = ctx.Skill.Include("Hero").Where(x => x.ID == skill.ID).First();

                    var result = from s in ctx.SkillEffectName
                                 where s.Name == name
                                    && s.SkillId == skill.ID
                                    && s.Skill.Hero.Name == skill.Hero.Name
                                    && s.Skill.Description == skill.Description
                                 select s;

                    if (result.Count() == 1)
                    {
                        Console.WriteLine(result.First().ID +  " Skill " + name + " Already exists..." );
                        skillEffectName = result.First();
                        return true;
                    }
                    else if (result.Count() > 1)
                    {
                        //TODO Create log
                        throw new Exception("More than 1 skillEffectname returned. Check this");
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    //TODO implementar log de erro
                    throw e;
                }                
            }
        }

        

        public SkillEffectName InsertSkillEffectName(string name, string heroName, Skill skill, List<string> skillEffectValues)
        {
            HeroCreator heroCreator = new HeroCreator();
            SkillEffectName skillEffectName;
            Hero hero = heroCreator.getHeroByName(heroName);

            Skill completeSkill = SkillCreator.SelectSkill(skill.Name, hero.ID);            

            bool exists = checkIfSkillNameExists(name, completeSkill, completeSkill.Description, out skillEffectName);

            if (!exists)
            {
                skillEffectName.Name = name.Trim();
                skillEffectName.SkillId = completeSkill.ID;
                if (skillEffectValues.FirstOrDefault() != null)
                    skillEffectName.ValueLv1 = skillEffectValues.First();
                if (skillEffectValues.ElementAtOrDefault(1) != null)
                    skillEffectName.ValueLv2 = skillEffectValues.ElementAt(1);
                if (skillEffectValues.ElementAtOrDefault(2) != null)
                    skillEffectName.ValueLv3 = skillEffectValues.ElementAt(2);
                if (skillEffectValues.ElementAtOrDefault(3) != null)
                    skillEffectName.ValueLv4 = skillEffectValues. ElementAt(3);
                if (skillEffectValues.LastOrDefault() != null)
                    skillEffectName.ValueScepter = skillEffectValues.Last();            

                using (Dota2Entities ctx = new Dota2Entities())
                {
                    try
                    {
                        skillEffectName = ctx.SkillEffectName.Add(skillEffectName);
                        ctx.SaveChanges();
                        Console.WriteLine("Skill " + name + " Created");
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return skillEffectName;
        }
    }
}
