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

            //Set Up ManaCost
            foreach (var manaCost in manaCostList)
            {
                setManaCost(manaCost);
            }

            //Set up CoolDowns
            foreach (var coolDown in coolDownList)
            {
                setCoolDowns(coolDown);
            }




        }

        void setManaCost(string manaCostString)
        {
            string[] manaCosts = manaCostString.Split('/');

            this.skill.ManaCostLv1 = int.Parse(manaCosts.First());
            this.skill.ManaCostLv2 = int.Parse(manaCosts.ElementAt(1));
            this.skill.ManaCostLv3 = int.Parse(manaCosts.ElementAt(2));
            this.skill.ManaCostLv4 = int.Parse(manaCosts.ElementAt(3));
        }

        void setCoolDowns(string coolDownString)
        {
            string[] coolDowns = coolDownString.Split('/');

            this.skill.CoolDownLv1 = decimal.Parse(coolDowns.First());
            this.skill.CoolDownLv2 = decimal.Parse(coolDowns.ElementAt(1));
            this.skill.CoolDownLv3 = decimal.Parse(coolDowns.ElementAt(2));
            this.skill.CoolDownLv4 = decimal.Parse(coolDowns.ElementAt(3));
        }

    }
}
