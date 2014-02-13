
using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
    public class SkillAbilityTypesCreator
    {
        SkillAbilityTypes skillAbilityType;
        public SkillAbilityTypes insert(int skillId, int abilityTypeId, out bool success)
        {
            skillAbilityType = new SkillAbilityTypes();            
            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    skillAbilityType.SkillId = skillId;
                    skillAbilityType.AbilityTypeId = abilityTypeId;

                    ctx.SkillAbilityTypes.Add(skillAbilityType);

                    ctx.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    success = false;
                    throw e;
                }
            }            

            return skillAbilityType;
        }
    }
}
