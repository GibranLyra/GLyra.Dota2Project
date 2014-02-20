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
        protected bool checkIfSkillNameExists(string name, Skill skill, string skillDescription, out SkillEffectName skillEffectName)
        {
            using (Dota2Entities ctx = new Dota2Entities())
            {
                skillEffectName = new SkillEffectName();

                try
                {
                    skill = ctx.Skill.Include("Hero").Where(x => x.ID == skill.ID).First();

                    var result = from s in ctx.SkillEffectName
                                 where s.SkillId == skill.ID
                                    && s.Skill.Hero.Name == skill.Hero.Name
                                    && s.Skill.Description == skill.Description
                                 select s;

                    if (result.Count() == 1)
                    {
                        Console.WriteLine("Skill " + name + " Already exists...");
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
            SkillEffectName skillEffectName;            

            Skill completeSkill = SkillCreator.SelectByName(skill.Name);            

            bool exists = checkIfSkillNameExists(name, completeSkill, completeSkill.Description, out skillEffectName);

            if (!exists)
            {
                skillEffectName.Name = name.Trim();
                skillEffectName.SkillId = completeSkill.ID;
                skillEffectName.ValueLv1 = int.Parse(skillEffectValues.First());
                skillEffectName.ValueLv2 = int.Parse(skillEffectValues.ElementAt(1));
                skillEffectName.ValueLv1 = int.Parse(skillEffectValues.ElementAt(2));
                skillEffectName.ValueLv1 = int.Parse(skillEffectValues.ElementAt(3));
                skillEffectName.ValueLv1 = int.Parse(skillEffectValues.Last());            

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
