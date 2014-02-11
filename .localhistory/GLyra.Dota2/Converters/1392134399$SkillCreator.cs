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

        public Skill createSkill(int heroId, string name, string description, List<KeyValuePair<string, string>> manaCostList, List<KeyValuePair<string, string>> coolDownList,
            List<KeyValuePair<string, string>> abilityCastTypeList, List<KeyValuePair<string, string>> targetAffectedTypeList, List<KeyValuePair<string, string>> damageTypeList, string videoUrl)
        {
            this.skill = new Skill();

            this.skill.HeroId = heroId;
            this.skill.Name = name;
            this.skill.Description = description;
            this.skill.VideoUrl = videoUrl;

            //Set Up ManaCost
            foreach (var manaCost in manaCostList)
            {
                //Check if the mana cost is for the current skill
                //We need to check this because certain skills don't need mana
                if (manaCost.Key == this.skill.Name)
                {
                    setManaCost(manaCost.Value);
                }
            }

            //Set up CoolDowns
            foreach (var coolDown in coolDownList)
            {
                //Check if the coolDown is for the current skill
                //We need to check this because certain skills don't have coolDown
                if (coolDown.Key == this.skill.Name)
                    setCoolDowns(coolDown.Value);
            }

            foreach (var abilityCastType in abilityCastTypeList)
            {
                //Check if the AbilityCastType is for the current skill
                //We need to check this because certain skills don't have AbilityCastType
                if (abilityCastType.Key == this.skill.Name)
                {
                    if (!string.IsNullOrEmpty(abilityCastType.Value))
                        setAbilityCastType(abilityCastType.Value);
                }
            }

            foreach (var targetAffectedType in targetAffectedTypeList)
            {
                //Check if the targetAffectedType is for the current skill
                //We need to check this because certain skills don't have targetAffectedType
                if (!string.IsNullOrEmpty(targetAffectedType.Value))
                    setTargetAffectedType(targetAffectedType.Value);
            }

            foreach (var damageType in damageTypeList)
            {
                //Check if the damageType is for the current skill
                //We need to check this because certain skills don't have damageType
                if (!string.IsNullOrEmpty(damageType.Value))
                    setDamageType(damageType.Value);
            }

            insertSkill();

            return this.skill;
        }

        void setManaCost(string manaCostString)
        {
            string[] manaCosts = manaCostString.Split('/');
            int size = manaCosts.Count();

            if (size > 0)
                this.skill.ManaCostLv1 = int.Parse(manaCosts.First());
            if (size > 1)
                this.skill.ManaCostLv2 = int.Parse(manaCosts.ElementAt(1));
            if (size > 2)
                this.skill.ManaCostLv3 = int.Parse(manaCosts.ElementAt(2));
            if (size > 3)
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

        void setAbilityCastType(string abilitiesCastTypeString)
        {
            AbilityType abilityType = new AbilityType();
            string[] abilityTypeArray = abilitiesCastTypeString.Split(',');

            for (int i = 0; i < abilityTypeArray; i++)
            {
                
            }
            
            using(Dota2Entities ctx = new Dota2Entities())
            {
                var abilityTp = (from p in ctx.AbilityType
                             where p.Name == abilitiesCastTypeString
                             select p).FirstOrDefault();

                abilityType = abilityTp;
            }

            if (abilityType != null)
            {
                this.skill.AbilityTypeId = abilityType.ID;
            }
            else
            {
                //TODO Incluir erro no log
                throw new Exception("No AbilityType has been returned. AbilityType: " + abilitiesCastTypeString);
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
                this.skill.DamageTypeId = damageType.ID;
            }
            else
            {
                //TODO Incluir este erro no log posteriormente
                throw new Exception("No DamageType has been returned. DamageType: " + damageTypeString);
            }
        }

        Skill insertSkill()
        {
            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    ctx.Skill.Add(this.skill);

                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    //TODO Adicionar ao log
                    throw e;
                }
            }

            return this.skill;
        }
    }
}
