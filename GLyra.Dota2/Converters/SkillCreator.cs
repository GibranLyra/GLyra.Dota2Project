using GLyra.Dota2.Repositories;
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

        public Skill createSkill(int heroId, string name, string description, List<KeyValuePair<string, string>> manaCostList, List<string> coolDownList,
            string abilityCastType, string targetAffectedType, string damageType, string videoUrl)
        {
            this.skill = new Skill();

            this.skill.HeroId = heroId;
            this.skill.Name = name;
            this.skill.Description = description;
            this.skill.VideoUrl = videoUrl;
            //Set Up ManaCost
            foreach (var manaCost in manaCostList)
            {
                setManaCost(manaCost.Value);
            }

            //Set up CoolDowns
            foreach (var coolDown in coolDownList)
            {
                setCoolDowns(coolDown);
            }

            if(!string.IsNullOrEmpty(abilityCastType))
                setAbilityCastType(abilityCastType);

            if (!string.IsNullOrEmpty(targetAffectedType))
                setTargetAffectedType(targetAffectedType);

            if (!string.IsNullOrEmpty(damageType))
                setDamageType(damageType);

            return this.skill;

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
            int size = coolDowns.Count();
            
            if(size > 0)
                this.skill.CoolDownLv1 = decimal.Parse(coolDowns.First());
            if(size > 1)
                this.skill.CoolDownLv2 = decimal.Parse(coolDowns.ElementAt(1));
            if(size > 2)
                this.skill.CoolDownLv3 = decimal.Parse(coolDowns.ElementAt(2));
            if(size > 3)
                this.skill.CoolDownLv4 = decimal.Parse(coolDowns.ElementAt(3));
        }

        void setAbilityCastType(string abilityCastTypeString)
        {
            AbilityType abilityType = new AbilityType();
            using(Dota2Entities ctx = new Dota2Entities())
            {
                var abilityTp = (from p in ctx.AbilityType
                             where p.Name == abilityCastTypeString
                             select p).FirstOrDefault();

                abilityType = abilityTp;
            }

            if (abilityType != null)
            {
                this.skill.AbilityType = abilityType;
                this.skill.AbilityTypeId = abilityType.ID;
            }
            else
            {
                //TODO Incluir este erro no log posteriormente
                throw new Exception("No AbilityType has been returned. AbilityType: " + abilityCastTypeString);
            }            
        }

        void setTargetAffectedType (string targetAffectedTypeString)
        {
            TargetAffectedType targetAffectedType = new TargetAffectedType();
            using (Dota2Entities ctx = new Dota2Entities())
            {
                var targetAffectedTp = (from p in ctx.TargetAffectedType
                                        where p.Name == targetAffectedTypeString
                                 select p).FirstOrDefault();

                targetAffectedType = targetAffectedTp;
            }

            if (targetAffectedType != null)
            {
                this.skill.TargetAffectedType = targetAffectedType;
                this.skill.TargetAffectedTypeId = targetAffectedType.ID;
            }
            else
            {
                //TODO Incluir este erro no log posteriormente
                throw new Exception("No TargetAffectedType has been returned. TargetAffectedType: " + targetAffectedTypeString);
            }
        }

        void setDamageType(string damageTypeString)
        {
            DamageType damageType = new DamageType();
            using (Dota2Entities ctx = new Dota2Entities())
            {
                var damageTp = (from p in ctx.DamageType
                                        where p.Name == damageTypeString
                                        select p).FirstOrDefault();

                damageType = damageTp;
            }

            if (damageType != null)
            {
                this.skill.DamageType = damageType;
                this.skill.DamageTypeId = damageType.ID;
            }
            else
            {
                //TODO Incluir este erro no log posteriormente
                throw new Exception("No DamageType has been returned. DamageType: " + damageTypeString);
            }
        }
    }
}
