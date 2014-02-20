using GLyra.Dota2.Repositories;
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
        protected SkillEffectName checkIfSkillNameExists(string name, Skill skill, string skillDescription)
        {
            SkillEffectName skillEffectName = new SkillEffectName();
            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    skillEffectName = ctx.SkillEffectName.Where(x =>
                                                                x.Name == name &&
                                                                x.Skill.Hero.Name == skill.Hero.Name &&
                                                                skillDescription == skill.Description).First();
                    if(skillEffectName != null)
                    {
                        Console.WriteLine("Skill " + name + " Already exists...");
                        return skillEffectName;
                    }
                    else
                    {
                        return null;   
                    }
                }
                catch (Exception e)
                {
                    //TODO implementar log de erro
                    throw e;
                }                
            }
        }

        

        public SkillEffectName insertSkillEffectName(string name, string heroName, string skillName, 
            List<string> skillEffectValues, string skillDescription)
        {
            SkillEffectName skillEffectName = new SkillEffectName();

            Skill skill = SkillCreator.SelectByName(skillName);

            skillEffectName.Name = name.Trim();

            

            skillEffectName = checkIfSkillNameExists(name, skill, skillDescription);

            if (skillEffectName == null)
            {
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
