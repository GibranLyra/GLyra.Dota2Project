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
                    ctx.Configuration.LazyLoadingEnabled = false;

                    var result = from s in ctx.SkillEffectName
                                 where s.SkillId == skill.ID
                                    && s.Skill.Hero.Name == skill.Hero.Name
                                    && s.Skill.Description == skill.Description
                                 select s;
                    
                    if(result.Count() == 0)
                    {
                        Console.WriteLine("Skill " + name + " Already exists...");
                        return result.First();
                    }
                    else if(result.Count() > 1)
                    {
                        //TODO Create log
                        throw new Exception("More than 1 skilleffectname returned. Check this");
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

        

        public SkillEffectName InsertSkillEffectName(string name, string heroName, Skill skill, List<string> skillEffectValues)
        {
            SkillEffectName skillEffectName = new SkillEffectName();

            Skill completeSkill = SkillCreator.SelectByName(skill.Name);

            skillEffectName.Name = name.Trim();



            skillEffectName = checkIfSkillNameExists(name, completeSkill, completeSkill.Description);

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
