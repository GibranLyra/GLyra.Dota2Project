using GLyra.Dota2.ModelCreators;
using GLyra.Dota2.Repositories;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Dota.CentralDota.Converters
{
	internal class HeroDataConverter
	{
		private bool isInsert = false;

		private AgilityPackHelper agilityPackHelper;
		private List<string> heroesNames;
        private List<string> heroesPortraits;
		private List<string> heroesUrl;
		private List<string> skillImages;
		private List<string> skillNames;
		private List<string> skillDescriptions;
		private List<string> skillRemainingValuesDescriptions;//List os the descriptions collected in the remainingvalues method //TODO re-do this
		private List<string> primaryStatsImages;
		private Dictionary<string, string> primaryStatsValues;
		private string biography;
		private List<KeyValuePair<string, string>> manaCostDictionary;
		private List<KeyValuePair<string, string>> coolDownList;
		private List<KeyValuePair<string, string>> abilityCastType;
		private List<KeyValuePair<string, string>> skillTargetAffectedType;
		private List<KeyValuePair<string, string>> skillDamageType;
		private string skillVideo;
		private Dictionary<Skill, Dictionary<string, string>> skillRemainingValues;
		private HeroCreator heroCreator;
		private SkillCreator skillCreator;
		private Hero currentHero;

		public HeroDataConverter()
		{
			heroesNames = new List<string>();
			heroesUrl = new List<string>();
            heroesPortraits = new List<string>();
			skillImages = new List<string>();
			skillNames = new List<string>();
			skillDescriptions = new List<string>();
			skillRemainingValuesDescriptions = new List<string>();
			primaryStatsImages = new List<string>();
			primaryStatsValues = new Dictionary<string, string>();
			manaCostDictionary = new List<KeyValuePair<string, string>>();
			coolDownList = new List<KeyValuePair<string, string>>();
			abilityCastType = new List<KeyValuePair<string, string>>();
			skillTargetAffectedType = new List<KeyValuePair<string, string>>();
			skillDamageType = new List<KeyValuePair<string, string>>();
			skillRemainingValues = new Dictionary<Skill, Dictionary<string, string>>();
			heroCreator = new HeroCreator();
			skillCreator = new SkillCreator();
			agilityPackHelper = new AgilityPackHelper();

			heroesNames = GetHeroesName();

            for (int i = 0; i < heroesNames.Count; i++)
            {
                if (!isInsert)
                    currentHero = heroCreator.getHeroByName(heroesNames[i]);
                else
                    heroCreator.createHero(heroesNames[i], biography);



                getDataFromHtml(heroesNames[i]);


                //createSkillEffectName(heroesNames[i]);

                //createSkill();
                //createPrimaryAttributes();
            }
		}

        void createHeroPortrait(int heroNamesIndex)
        {
            HeroPortraitCreator heroPortraitCreator = new HeroPortraitCreator();
            heroPortraitCreator.InsertHeroPortrait(currentHero.HeroPortraitsId, heropor)
        }

		protected void createSkillEffectName(string heroName)
		{
			SkillEffectNameCreator effectNameCreator = new SkillEffectNameCreator();

			foreach (var item in skillRemainingValues)
			{
				foreach (var effectNameValueDic in skillRemainingValues[item.Key])
				{
					string effectName = effectNameValueDic.Key.Replace(":", string.Empty);
					List<string> effectValuesList = effectNameValueDic.Value.Split('/').ToList();
					//INSERT
					effectNameCreator.InsertSkillEffectName(effectName, heroName, item.Key, effectValuesList);
				}
			}
		}

		protected void getDataFromHtml(string heroName)
		{
			HtmlDocument doc = new HtmlDocument();

			doc = LoadHeroHtmlPage(heroName);

            heroesPortraits = GetSkillPortraits();
			skillImages = GetSkillImages(doc);
			skillNames = GetSkillNames(doc);
			skillDescriptions = GetSkillDescriptions(doc);
			primaryStatsImages = GetPrimaryStatsImages(doc);
			primaryStatsValues = GetPrimaryStatsValues(doc);
			biography = GetBiography(doc).Trim();
			manaCostDictionary = GetManaCost(doc);
			coolDownList = GetCoolDown(doc);
			abilityCastType = GetAbilityCastType(doc);
			skillTargetAffectedType = GetSkillTargetAffectedType(doc);
			skillDamageType = GetSkillDamageType(doc);
			skillVideo = GetSkillVideo(doc);
			skillRemainingValues = GetSkillRemainingValues(doc);

			Console.WriteLine("Getting info from Dota2 page Completed");
		}

        private List<string> GetSkillPortraits(HtmlDocument doc)
        {
            int expectedSize = 1;
            List<string> heroesPortraitsUrlsList = new List<string>();

            var heroTopPortraitContainer = doc.DocumentNode.SelectNodes("//*[(@id = 'heroTopPortraitContainer')]");

            heroesPortraitsUrlsList = agilityPackHelper.GetImageUrls(heroTopPortraitContainer, "heroPortraitImg", expectedSize);

            return heroesPortraitsUrlsList;

            throw new NotImplementedException();
        }

		protected void createSkill()
		{
			for (int i = 0; i < skillNames.Count; i++)
			{
				Skill skill = heroCreator.createHeroSkill(currentHero.ID, skillNames[i], skillDescriptions[i], manaCostDictionary, coolDownList, abilityCastType,
					skillTargetAffectedType, skillDamageType, skillVideo);
				Console.WriteLine("Skill " + skillNames[i] + " Created");
				//INSERT SKILL IMAGE
				createSkillImage(skill.ID, skillImages[i]);
			}
		}

		protected void createSkillImage(int skillId, string url)
		{
			SkillImageCreator skillImageCreator = new SkillImageCreator();
			skillImageCreator.InsertSkillImage(skillId, url);
		}

		protected void createPrimaryAttributes()
		{
			heroCreator.createHeroPrimaryStats(currentHero.ID, primaryStatsValues);
		}

		protected HtmlDocument LoadHeroHtmlPage(string heroName)
		{
			HtmlDocument doc = new HtmlDocument();
			string heroUrlName = heroName.Replace("'", "");
			heroUrlName = heroUrlName.Replace(" ", "_");

			string pageUrl = "http://www.dota2.com/hero/" + heroUrlName + "/?l=english";
			doc.Load(GetStream(pageUrl));
			Console.WriteLine("accessing: " + pageUrl);
			return doc;
		}

		protected HtmlDocument LoadHeroesPage()
		{
			HtmlDocument doc = new HtmlDocument();
			doc.Load(GetStream("http://www.dota2.com/heroes/?l=english"));

			return doc;
		}

		private List<string> GetHeroesName()
		{
			HtmlDocument doc = LoadHeroesPage();

			var heroesNameNode = doc.DocumentNode.SelectNodes("//*[(@id = 'filterName')]");

			var namesNode = heroesNameNode.First();

			//Get Heroes Names
			//Skip 2 because of the filters of steam website (All and HERO NAME)
			var names = (namesNode.Descendants("option")
				.Select(e => e.NextSibling.InnerText)).Skip(2);

			//Fill the urls by the names of the heros
			foreach (var name in names)
			{
				heroesUrl.Add(name.Replace(" ", "_"));
			}
			return names.ToList();
		}

		private List<string> GetSkillImages(HtmlDocument doc)
		{
			int expectedSize = 1;
			List<string> habilityImgUrlsList = new List<string>();

			var abilityIconHolder2 = doc.DocumentNode.SelectNodes("//*[(@class = 'abilityIconHolder2')]");

			habilityImgUrlsList = agilityPackHelper.GetImageUrls(abilityIconHolder2, "abilityIconHolder2", expectedSize);

			return habilityImgUrlsList;
		}

		private List<string> GetSkillNames(HtmlDocument doc)
		{
			int expectedSize = 1;
			var skillNamesList = new List<string>();

			var overviewAbilityRowDescription = doc.DocumentNode.SelectNodes("//*[@class = 'overviewAbilityRowDescription']");

			foreach (var skillName in overviewAbilityRowDescription)
			{
				//Get the skillNames
				var skillNames = skillName.Descendants("h2")
									.Select(n => n.InnerHtml);

				//Insert the skillName in the list
				if (skillNames.Count() > expectedSize)
					LogHandler.createWarningLog("overviewAbilityRowDescription-SkillName", expectedSize, skillNames.Count());

				skillNamesList.Add(skillNames.First());
			}

			return skillNamesList;
		}

		private List<string> GetSkillDescriptions(HtmlDocument doc)
		{
			int expectedSize = 1;
			var skillDescriptionsList = new List<string>();

			var overviewAbilityRowDescription = doc.DocumentNode.SelectNodes("//*[@class = 'overviewAbilityRowDescription']");

			foreach (var skillDescription in overviewAbilityRowDescription)
			{
				//Get the skillDescription
				var skillDescriptions = skillDescription.Descendants("p")
									.Select(n => n.InnerHtml);

				if (skillDescriptions.Count() > expectedSize)
					LogHandler.createWarningLog("overviewAbilityRowDescription-SkillDescription", expectedSize, skillDescriptions.Count());

				skillDescriptionsList.Add(skillDescriptions.First());
			}

			return skillDescriptionsList;
		}

		private List<KeyValuePair<string, string>> GetAbilityCastType(HtmlDocument doc)
		{
			var AbilityCastTypeList = new List<KeyValuePair<string, string>>();

			var skillList = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

			//Pega o valor do primeiro span(que é o que contém a informação do comportamento da skill
			foreach (var skillType in skillList)
			{
				//Get the name of the skill that the mana belongs
				string skillName = getSkillNameInSkillData(skillType);

				var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
				var abilityCastTypeKeyValue = new KeyValuePair<string, string>(skillName, spanList.First().InnerText.Trim());
				AbilityCastTypeList.Add(abilityCastTypeKeyValue);
			}

			return AbilityCastTypeList;
		}

		private List<KeyValuePair<string, string>> GetSkillTargetAffectedType(HtmlDocument doc)
		{
			var skillTargetAffectedTypeList = new List<KeyValuePair<string, string>>();

			var abilityFooterBoxLeft = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

			//Get the last span node(the one that have the info of type "damage type"
			foreach (var skillType in abilityFooterBoxLeft)
			{
				string skillName = getSkillNameInSkillData(skillType);

				var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");

				//Some skills don't have damage
				if (spanList.Count > 1)
				{
					KeyValuePair<string, string> skillTargetAffectedKeyValue;
					if (spanList.Count == 3)
					{
						skillTargetAffectedKeyValue = new KeyValuePair<string, string>(skillName, spanList.ElementAt(1).InnerText);
					}
					//Checks if the value is damage. If it isn't, then the value is "Affects"
					else if (spanList.Last().PreviousSibling.InnerText.Contains("AFFECTS"))
					{
						skillTargetAffectedKeyValue = new KeyValuePair<string, string>(skillName, spanList.Last().InnerText);
					}
					else
					{
						skillTargetAffectedKeyValue = new KeyValuePair<string, string>(string.Empty, string.Empty);
						skillTargetAffectedTypeList.Add(skillTargetAffectedKeyValue);
					}
					skillTargetAffectedTypeList.Add(skillTargetAffectedKeyValue);
				}
			}

			return skillTargetAffectedTypeList;
		}

		private List<KeyValuePair<string, string>> GetSkillDamageType(HtmlDocument doc)
		{
			var damageTypeList = new List<KeyValuePair<string, string>>();

			var abilityFooterBoxLeft = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

			//Get the last span Value(the one that contains the info of damage Type)
			foreach (var damageType in abilityFooterBoxLeft)
			{
				string skillName = getSkillNameInSkillData(damageType);

				var spanList = damageType.SelectNodes(".//*[@class = 'attribVal']");

				KeyValuePair<string, string> damageTypeKeyValue;
				//Some skills don't have damage
				if (spanList.Count > 1)
				{
					//Check if the value is "Damage", if it's not, the value is "Affects"
					if (spanList.Last().PreviousSibling.InnerText.Contains("DAMAGE") || spanList.Count == 3)
					{
						damageTypeKeyValue = new KeyValuePair<string, string>(skillName, spanList.Last().InnerText);
					}
					else
					{
						damageTypeKeyValue = new KeyValuePair<string, string>(String.Empty, String.Empty);
					}
				}
				else
				{
					damageTypeKeyValue = new KeyValuePair<string, string>(String.Empty, String.Empty);
				}

				damageTypeList.Add(damageTypeKeyValue);
			}

			return damageTypeList;
		}

		private Dictionary<Skill, Dictionary<string, string>> GetSkillRemainingValues(HtmlDocument doc)
		{
			var dicRemainingValues = new Dictionary<Skill, Dictionary<string, string>>();

			var skillList = new List<Skill>();//Where the skills are

			//We need to take this nodelist to get the skillName of the remaining values
			var abilityHeaderRowDescription = doc.DocumentNode.SelectNodes("//*[@class = 'abilityHeaderRowDescription']");

			//The node that the remaining values are
			var abilityFooterBoxRight = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxRight']");

			for (int i = 0; i < abilityFooterBoxRight.Count; i++)
			{
				var skillEffectNameList = new List<string>();//Where the descriptions of the skills are
				var skillEffectValuesList = new List<string>(); //Where the values of the skills are

				var divInnerHtml = abilityFooterBoxRight[i].SelectNodes(".//*[contains(@span, '')]");

				//Íf the divInnerHtml is empty, it means that the abilityFooterBoxRight don't have info about the skill
				//but the abilityHeaderRowDescription may have info, so we must check it
				if (divInnerHtml != null)
				{
					//? Get the list of br tags that contains the description
					var descriptionList = divInnerHtml.Where(x => x.Name == "br").ToList();

					//? Get the list of span tags that contains the values
					var valuesList = divInnerHtml.Where(x => x.Name == "span")
												 .Where(x => x.Attributes["class"].Value != "scepterVal");

					//? Get the name of the skills that the current remanining values are
					string skillName = getSkillNameInRemainingSkill(abilityHeaderRowDescription[i]);
					var skillComparer = new Skill() { Name = skillName };//Create skillobject to do the comparison

					//? Get the description of the skills that the current remanining values are
					skillComparer.Description = getSkillDescriptionInRemainingSkill(abilityHeaderRowDescription[i]);

					//Now a workaround, Because dictionaries doesn't accept keys with same value, we need to add a " "
					//to skills that have the same name.
					if (skillList.Contains(skillComparer))
						skillList.Add(workAroundToSkillsOfSameName(skillName));
					else
						skillList.Add(skillComparer);
					//In case that no tag "br" exists, it means that the skill has only one description
					//So we need to get the value inside the div tag
					if (descriptionList.Count <= 0)
					{
						HtmlNode htmlNode = abilityFooterBoxRight[i].ChildNodes.Where(x => x.Name == "#text").First();
						skillEffectNameList.Add(htmlNode.InnerText.Trim());
					}

					else
					{
						for (int it = 0; it < descriptionList.Count; it++)
						{
							//Como pegando somente o nextSibling perdemos o primeiro texto, quando o I for 0, pegamos o 1° texto
							if (it == 0)
								skillEffectNameList.Add(descriptionList[it].PreviousSibling.PreviousSibling.InnerText.Trim());

							if (descriptionList.Count > 1)
							{
								//When the value is "" the skill must be a scepter upgrade, so we need to check the scepterVal class
								if (descriptionList[it].NextSibling.InnerText.Trim() == "")
								{
									skillEffectNameList.Add(descriptionList[it].ParentNode.SelectNodes(".//*[@class = 'scepterVal']").First().InnerText.Trim());
									divInnerHtml.Remove(descriptionList[it].ParentNode.SelectNodes(".//*[@class = 'scepterVal']").First());
								}
								else
									skillEffectNameList.Add(descriptionList[it].NextSibling.InnerText.Trim());
							}
						}
					}

					foreach (var span in valuesList)
					{
						skillEffectValuesList.Add(span.InnerText.Trim());
					}
					Dictionary<string, string> dicDescValue = new Dictionary<string, string>();

					for (int ix = 0; ix < skillEffectNameList.Count; ix++)
					{
						//Now a workaround, Because dictionaries doesn't accept keys with same value, we need to add a " "
						//to skillDescriptionList that have the same name.
						//if (dicDescValue[skillName].Contains(skillDescriptionList[ix]))
						//{
						//    skillDescriptionList[ix] = workAroundToSkillsSameDescription(skillDescriptionList[ix]);
						//}
						//else

						if (skillEffectNameList.Count != skillEffectNameList.Distinct().Count())
						{
							var duplicates = skillEffectNameList.GroupBy(s => s).SelectMany(grp => grp.Skip(1));

							//TODO Comentar isto
							foreach (var duplicate in duplicates)
							{
								for (int it = 0; it < skillEffectNameList.Count; it++)
								{
									if (skillEffectNameList[it].Contains(duplicate))
									{
										skillEffectNameList[it] = skillEffectNameList[it] + " ";
									}
								}
							}
						}

						dicDescValue.Add(skillEffectNameList[ix], skillEffectValuesList[ix]);
					}

					dicRemainingValues.Add(skillList[i], dicDescValue);
				}
				//In this case, the skill don't have values or descriptions, so we only need to get their name
				else if (abilityHeaderRowDescription != null)
				{
					Skill skill = new Skill() { Name = getSkillNameInRemainingSkill(abilityHeaderRowDescription[i]) };
					skillList.Add(skill);
				}
			}
			return dicRemainingValues;
		}

		protected Skill workAroundToSkillsOfSameName(string skillName)
		{
			Skill skill = new Skill() { Name = skillName + " " };
			return skill;
		}

		protected string workAroundToSkillsSameDescription(string skillDescription)
		{
			return skillDescription + " ";
		}

		/// <summary>
		/// Returns the name of the skill when the node is inside the abilityHeaderRowDescription class
		/// </summary>
		protected string getSkillNameInRemainingSkill(HtmlNode abilityHeaderRowDescriptionClassNode)
		{
			string skillName = abilityHeaderRowDescriptionClassNode.ChildNodes.Where(x => x.Name == "h2").First().InnerText.Trim();
			return skillName;
		}

		/// <summary>
		/// Returns the description of the skill when the node is inside the abilityHeaderRowDescription class
		/// </summary>
		protected string getSkillDescriptionInRemainingSkill(HtmlNode abilityHeaderRowDescriptionClassNode)
		{
			string skillDescription = abilityHeaderRowDescriptionClassNode.ChildNodes.Where(x => x.Name == "p").First().InnerText.Trim();
			return skillDescription;
		}

		private string GetSkillVideo(HtmlDocument doc)
		{
			int expectedSize = 1;
			string skillVideo = String.Empty;

			var abilityVideoContainer = doc.DocumentNode.SelectNodes("//*[@class = 'abilityVideoContainer']");

			if (abilityVideoContainer != null)
			{
				foreach (var video in abilityVideoContainer)
				{
					//Get the skillDescription
					var skillVideos = video.Descendants("iframe")
										.Select(e => e.GetAttributeValue("src", null));

					if (skillVideos.Count() > expectedSize)
						LogHandler.createWarningLog("overviewAbilityRowDescription-SkillDescription", expectedSize, skillVideos.Count());

					skillVideo = skillVideos.First();
				}
			}

			return skillVideo;
		}

		private List<string> GetPrimaryStatsImages(HtmlDocument doc)
		{
			int expectedSize = 7;
			var primaryStatsImgUrlsList = new List<string>();

			var overviewPrimaryStats = doc.DocumentNode.SelectNodes("//*[@id = 'overviewPrimaryStats']");
			primaryStatsImgUrlsList = agilityPackHelper.GetImageUrls(overviewPrimaryStats, "overviewPrimaryStats", expectedSize);

			return primaryStatsImgUrlsList;
		}

		private Dictionary<string, string> GetPrimaryStatsValues(HtmlDocument doc)
		{
			int expectedSize = 6;
			var primaryStatsList = new Dictionary<string, string>();

			var overviewStatVal = doc.DocumentNode.SelectNodes("//*[@class = 'overview_StatVal']");
			var skillName = doc.DocumentNode.SelectNodes("//*[@class = 'overview_StatVal']");

			for (int i = 0; i < overviewStatVal.Count; i++)
			{
				//? GetThe name of primary Stat according to the position in the array
				//0 for intelligence, 1 for agility, 2 for strength
				//3 for Damage, 4 for MoveSpeed and 5 for Armor
				//string statName = overviewStatVal[i]
				string statsName = overviewStatVal[i].PreviousSibling.PreviousSibling.GetAttributeValue("title", string.Empty);

				//TODO implements log
				//Checks if the value is empty, if is empty writes a log
				if (statsName == string.Empty)
					throw new Exception();

				string statValue = overviewStatVal[i].InnerText;

				primaryStatsList.Add(statsName, statValue);
			}

			if (primaryStatsList.Count() > expectedSize)
				LogHandler.createWarningLog("overviewStatVal", expectedSize, overviewStatVal.Count());

			return primaryStatsList;
		}

		private string GetBiography(HtmlDocument doc)
		{
			string bio;

			var bioInner = doc.DocumentNode.SelectNodes("//*[@id = 'bioInner']");
			bio = bioInner.First().InnerText;

			return bio;
		}

		private List<KeyValuePair<string, string>> GetManaCost(HtmlDocument doc)
		{
			var manaCostDictionary = new List<KeyValuePair<string, string>>();

			var mana = doc.DocumentNode.SelectNodes("//*[@class = 'mana']");

			foreach (var manaCost in mana)
			{
				//Get the name of the skill that the mana belongs
				string skillName = getSkillNameInSkillData(manaCost);
				//Search for the span in the node
				var spanManaCost = manaCost.Descendants("span");
				//Remove the span that contains the text "Mana Cost: "
				manaCost.RemoveChild(spanManaCost.First());
				var manaCostKeyValue = new KeyValuePair<string, string>(skillName, manaCost.InnerText);
				manaCostDictionary.Add(manaCostKeyValue);
			}
			return manaCostDictionary;
		}

		private List<KeyValuePair<string, string>> GetCoolDown(HtmlDocument doc)
		{
			var coolDownList = new List<KeyValuePair<string, string>>();

			var cooldown = doc.DocumentNode.SelectNodes("//*[@class = 'cooldown']");

			foreach (var cooldownTime in cooldown)
			{
				//Get the name of the skill that the mana belongs
				string skillName = getSkillNameInSkillData(cooldownTime);
				//Search for the span in the node
				var spanCoolDownTime = cooldownTime.Descendants("span");
				//Remove the span that contains the text "Cooldown: "
				cooldownTime.RemoveChild(spanCoolDownTime.First());

				var coolDownKeyValue = new KeyValuePair<string, string>(skillName, cooldownTime.InnerText);
				coolDownList.Add(coolDownKeyValue);

				//coolDownList.Add(cooldownTime.InnerText);
			}

			return coolDownList;
		}

		/// <summary>
		/// Returns the name of the skill in the context of the function
		/// Functions that need the information are:
		/// CoolDOwn, Mana, abilityCast, abilityTarget and DamageType
		/// </summary>
		private string getSkillNameInSkillData(HtmlNode currentNode)
		{
			return currentNode.ParentNode.ParentNode.ParentNode.Descendants("h2").First().InnerText;
		}

		protected Stream GetStream(string url)
		{
			HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

			return aResponse.GetResponseStream();
		}
	}
}