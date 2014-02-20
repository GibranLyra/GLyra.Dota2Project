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
        //protected SkillEffectName checkIfSkillNameExists(string name, string heroName, string skillId)
        //{
        //    SkillEffectName skillEffectName = new SkillEffectName();
        //    using(Dota2Entities ctx = new Dota2Entities())
        //    {
        //        //get SkillId
                
                
        //        if (skill != null)
        //        {
        //            try
        //            {
        //                if (ctx.SkillEffectName.Any(x =>
        //                                            x.Name == name &&
        //                                            x.Skill.Hero.Name == heroName))
        //                {
        //                    Console.WriteLine("Skill " + name + " Already exists...");
        //                    return true;
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                //TODO implementar log de erro
        //                throw e;
        //            }
        //        }
                
        //    }
        //}

        

        public SkillEffectName insertSkillEffectName(string name, string heroName, string skillId, List<string> skillEffectValues)
        {
            SkillEffectName skillEffectName = new SkillEffectName();

            skillEffectName.Name = name.Trim();

            skillEffectName  = checkIfSkillNameExists(name, heroName, skillId);

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
