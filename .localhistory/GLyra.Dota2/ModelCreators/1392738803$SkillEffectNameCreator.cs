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
        /// Checks if the description of the skill exists, in negative case call the insertSkillDesc
        /// </summary>
        public SkillEffectName checkIfSkillNameExists(string name)
        {
            SkillEffectName skillEffectName = new SkillEffectName();
            using(Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    if (ctx.SkillEffectName.Any(x => x.Name == name))
                    {
                        Console.WriteLine("Skill " + name + " Already exists...");
                    }
                    else
                    {
                        skillEffectName = insertSkillEffectName(name);
                    }
                }w
                catch (Exception e)
                {
                    //TODO implementar log de erro
                    throw e;
                }
            }

            return skillEffectName;
        }

        public SkillEffectName insertSkillEffectName(string name)
        {
            SkillEffectName skillEffectName = new SkillEffectName();

            skillEffectName.Name = name.Trim();

            using(Dota2Entities ctx = new Dota2Entities())
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

            return skillEffectName;
        }
    }
}
