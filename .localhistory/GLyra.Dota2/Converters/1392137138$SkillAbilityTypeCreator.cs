
using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class SkillAbilityTypesCreator
    {
        public SkillAbilityTypes insert(int skillId, int abilityTypeId, Dota2Entities ctx)
        {
            SkillAbilityTypes skillAbilityType = new SkillAbilityTypes();
            try
            {
                
                skillAbilityType.SkillId = skillId;
                skillAbilityType.AbilityTypeId = abilityTypeId;

                ctx.SkillAbilityTypes.Add(skillAbilityType);
            }
            catch (Exception e)
            {   
                throw e;
            }

            return skillAbilityType;
        }
    }
}
