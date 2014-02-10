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
            List<string> abilityCastType, List<string> targetAffectedType, List<string> damageType, string videoUrl)
        {
            this.skill = new Skill();

            this.skill.Name = name;
            this.skill.Description = description;

            

        }

        void setManaCost(List<string> n)
        {
            this.skill.ManaCostLv1 = int.Parse(coolDownList.First());
            this.skill.CoolDownLv2 = int.Parse(coolDownList.ElementAt(1));
            this.skill.CoolDownLv3 = int.Parse(coolDownList.ElementAt(2));
            this.skill.CoolDownLv4 = int.Parse(coolDownList.ElementAt(3));
        }

        void setCoolDowns(List<string> coolDownList)
        {
            this.skill.CoolDownLv1 = int.Parse(coolDownList.First());
            this.skill.CoolDownLv2 = int.Parse(coolDownList.ElementAt(1));
            this.skill.CoolDownLv3 = int.Parse(coolDownList.ElementAt(2));
            this.skill.CoolDownLv4 = int.Parse(coolDownList.ElementAt(3));
        }
        
    }
}
