using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.Converters
{
    public class SkillCreator
    {
        Skill skill;
        public SkillCreator(string name, string description, List<string> manaCostList, List<string> coolDownList,
            List<string> abilityType, List<string> targetAffectedType, string damageType, string videoUrl)
        {
            this.skill = new Skill();

            

        }

        void setCoolDowns(List<string> coolDownList)
        {
            foreach (var coolDown in coolDownList)
            {
                
            }
        }
        
    }
}
