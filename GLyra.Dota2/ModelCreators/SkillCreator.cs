﻿using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
	public class SkillCreator
	{
		List<AbilityType> abilityTypeList;
		Skill skill;

		public Skill createSkill(int heroId, string name, string description, List<KeyValuePair<string, string>> manaCostList, List<KeyValuePair<string, string>> coolDownList,
			List<KeyValuePair<string, string>> abilityCastTypeList, List<KeyValuePair<string, string>> targetAffectedTypeList, List<KeyValuePair<string, string>> damageTypeList, string videoUrl)
		{

            if (SkillExists(name, description, heroId))
            {
                return SelectSkill(name, description, heroId);
            }
            else
            {
                this.skill = new Skill();
                this.abilityTypeList = new List<AbilityType>();
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

                InsertSkill();

            }
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
			string[] abilityTypeArray = abilitiesCastTypeString.Split(',');
			List<AbilityType> abilityTypes = new List<AbilityType>();
			 for (int i = 0; i < abilityTypeArray.Length; i++)
			{
				abilityTypes.Add(new AbilityType() { Name = abilityTypeArray[i].Trim() });
			}

			 foreach (var abilitType in abilityTypes)
			 {
				 //Verify is the AbilityType Exists
				 using (Dota2Entities ctx = new Dota2Entities())
				 {
					 try
					 {
						 var abilityTp = (from p in ctx.AbilityType
										  where p.Name == abilitType.Name
										  select p).FirstOrDefault();

						 //If the abilityType doesn't exists, throw a error
						 if (abilityTp == null)
						 {
							 //TODO Incluir erro no log
							 throw new Exception("No AbilityType has been returned. AbilityType: " + abilitiesCastTypeString);
						 }
						 //If it exists, add to the abilityType list, because one skill may have more than one abilityType
						 else
						 {
							 abilityTypeList.Add(abilityTp);
						 }
					 }
					 catch (Exception e)
					 {
						 //TODO Incluir erro no log
						 throw e;
					 }
				 }
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

		/// <summary>
		/// Insert a new Skill and SkillAbilityTypes
		/// </summary>        
		private Skill InsertSkill()
		{
			SkillAbilityTypesCreator skillAbilityTypesCreator = new SkillAbilityTypesCreator();
			bool isSkillAbilityTypeInserted = false;
			using (Dota2Entities ctx = new Dota2Entities())
			{
				try
				{
					ctx.Skill.Add(this.skill);
					ctx.SaveChanges();
					foreach (var abilityType in abilityTypeList)
					{
						skillAbilityTypesCreator.insert(this.skill.ID, abilityType.ID, out isSkillAbilityTypeInserted);
						//If we have a error when inserting SkillAbilityType, then delete the inserted skill
						if(!isSkillAbilityTypeInserted)
						{
							//TODO Create a function to remove all the SkillAbilityTypes that have been created for this skill
							ctx.Skill.Remove(this.skill);
						}
					}
					
					
				}
				catch (Exception e)
				{
					//TODO Adicionar ao log
					throw e;
				}
			}

			return this.skill;
		}

        public static Skill SelectSkill(string name, string description, int heroId)
        {
            Skill skill = new Skill();
            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    skill = ctx.Skill.Where(s => s.Name == name &&
                                                 s.HeroId == heroId &&
                                                 s.Description == description).FirstOrDefault();
                }
                catch (Exception e)
                {
                    //TODO adicionar log
                    throw e;
                }
            }

            return skill;
        }

        
        /// <summary>
        /// Workaround 
        /// Get the first skill description because there're skills that have the same name and the same heroName, but they are equals, so get the first description
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public static Skill SelectSkill(string name, int heroId)
        {
            Skill skill = new Skill();
            using (Dota2Entities ctx = new Dota2Entities())
            {
                try
                {
                    skill = ctx.Skill.Where(s => s.Name == name &&
                                                 s.HeroId == heroId).FirstOrDefault();
                }
                catch (Exception e)
                {
                    //TODO adicionar log
                    throw e;
                }
            }

            return skill;
        }


        public bool SkillExists(string skillName, string skillDescription, int heroId)
        {
            //Check if the skill exists
            using (Dota2Entities ctx = new Dota2Entities())
            {
                bool skillExists = ctx.Skill.Any(s => s.HeroId == heroId
                                      && s.Name == skillName
                                    && s.Description == skillDescription);

                return skillExists;
            }
                                    
        }
	}


}
