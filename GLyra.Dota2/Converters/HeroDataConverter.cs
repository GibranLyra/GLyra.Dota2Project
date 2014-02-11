using GLyra.Dota2.Converters;
using GLyra.Dota2.Repositories;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dota.CentralDota.Repositories
{
    class HeroDataConverter
    {
        AgilityPackHelper agilityPackHelper;
        List<string> heroesName = new List<string>();
        List<string> heroesUrl = new List<string>();
        List<string> skillImages = new List<string>();
        List<string> skillNames = new List<string>();
        List<string> skillDescriptions = new List<string>();
        List<string> primaryStatsImages = new List<string>();
        Dictionary<string,string> primaryStatsValues = new Dictionary<string,string>();
        string biography;
        List<KeyValuePair<string, string>> manaCostDictionary = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> coolDownList = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> abilityCastType = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> skillTargetAffectedType = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> skillDamageType = new List<KeyValuePair<string, string>>();
        string skillVideo;
        List<List<string>> skillRemainingValues = new List<List<string>>();
        HeroCreator heroCreator = new HeroCreator();
        SkillCreator skillCreator = new SkillCreator();

        public HeroDataConverter()
        {
            agilityPackHelper = new AgilityPackHelper();
            HtmlDocument doc = new HtmlDocument();

            heroesName = GetHeroesName();

            foreach (var heroName in heroesName)
            {
                doc = LoadHeroHtmlPage(heroName);

                skillImages = GetSkillPortraits(doc);
                skillNames = GetSkillNames(doc);
                skillDescriptions = GetSkillDescriptions(doc);
                primaryStatsImages = GetPrimaryStatsImages(doc);
                primaryStatsValues = GetPrimaryStatsValues(doc);
                biography = GetBiography(doc).Trim() ;
                manaCostDictionary = GetManaCost(doc);
                coolDownList = GetCoolDown(doc);
                abilityCastType = GetAbilityCastType(doc);
                skillTargetAffectedType = GetSkillTargetAffectedType(doc);
                skillDamageType = GetSkillDamageType(doc);
                skillVideo = GetSkillVideo(doc);
                skillRemainingValues = GetSkillRemainingValues(doc);

                heroCreator.createHero(heroName, biography);

                for (int i = 0; i < skillNames.Count; i++)
                {
                    heroCreator.createHeroSkill(skillNames[i], skillDescriptions[i], manaCostDictionary, coolDownList, abilityCastType,
                        skillTargetAffectedType, skillDamageType, skillVideo);
                }

                heroCreator.createHeroPrimaryStats(primaryStatsValues);
            }
        }

        private HtmlDocument LoadHeroHtmlPage(string heroName)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStream("http://www.dota2.com/hero/" + heroName + "/?l=english"));            

            return doc;
        }

        private HtmlDocument LoadHeroesPage()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStream("http://www.dota2.com/heroes/?l=english"));

            return doc;
        }

        List<string> GetHeroesName()
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
            return  names.ToList();            
        }

        List<string> GetSkillPortraits(HtmlDocument doc)
        {
            int expectedSize = 1;
            List<string> habilityImgUrlsList = new List<string>();

            var abilityIconHolder2 = doc.DocumentNode.SelectNodes("//*[(@class = 'abilityIconHolder2')]");

            habilityImgUrlsList = agilityPackHelper.GetImageUrls(abilityIconHolder2, "abilityIconHolder2", expectedSize);            

            return habilityImgUrlsList;
        }

        List<string> GetSkillNames(HtmlDocument doc)
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

        List<string> GetSkillDescriptions(HtmlDocument doc)
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

        List<KeyValuePair<string, string>> GetAbilityCastType(HtmlDocument doc)
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

        List<KeyValuePair<string, string>> GetSkillTargetAffectedType(HtmlDocument doc)
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

        List<KeyValuePair<string, string>> GetSkillDamageType(HtmlDocument doc)
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

        List<List<string>> GetSkillRemainingValues(HtmlDocument doc)
        {
            var remainingValues = new List<List<string>>();

            var abilityFooterBoxRight = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxRight']");

            foreach (var remainingValue in abilityFooterBoxRight)
            {
                var divInnerHtml = remainingValue.SelectNodes(".//*[contains(@span, '')]");

                var brList = divInnerHtml.Where(x => x.Name == "br").ToList();                
                var spanList = divInnerHtml.Where(x => x.Name == "span").ToList();

                var brTextList = new List<string>();//Onde ficam as descrições do que a skill faz
                var spanTextList = new List<string>(); //Onde ficam os valores do que a skill faz

                for (int i = 0; i < brList.Count; i++)
                {
                    //Como pegando somente o nextSibling perdemos o primeiro texto, quando o I for 0, pegamos o 1° texto
                    if(i == 0)
                        brTextList.Add(brList[i].PreviousSibling.PreviousSibling.InnerText);

                    brTextList.Add(brList[i].NextSibling.InnerText);
                }

                foreach (var span in spanList)
                {
                    spanTextList.Add(span.InnerText);
                }


                remainingValues.Add(brTextList);
                remainingValues.Add(spanTextList);
                //spanList.
            }

            return remainingValues;
        }

        string GetSkillVideo(HtmlDocument doc)
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

        List<string> GetPrimaryStatsImages(HtmlDocument doc)
        {
            int expectedSize = 7;
            var primaryStatsImgUrlsList = new List<string>();

            var overviewPrimaryStats = doc.DocumentNode.SelectNodes("//*[@id = 'overviewPrimaryStats']");
            primaryStatsImgUrlsList = agilityPackHelper.GetImageUrls(overviewPrimaryStats, "overviewPrimaryStats", expectedSize);

            return primaryStatsImgUrlsList;
        }

        Dictionary<string, string> GetPrimaryStatsValues(HtmlDocument doc)
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
                if(statsName == string.Empty)
                    throw new Exception();

                string statValue = overviewStatVal[i].InnerText;

                primaryStatsList.Add(statsName, statValue);
            }

            if (primaryStatsList.Count() > expectedSize)
                LogHandler.createWarningLog("overviewStatVal", expectedSize, overviewStatVal.Count());                

            return primaryStatsList;
        }

        string GetBiography(HtmlDocument doc)
        {
            string bio;

            var bioInner = doc.DocumentNode.SelectNodes("//*[@id = 'bioInner']");
            bio = bioInner.First().InnerText;

            return bio;
        }

        List<KeyValuePair<string, string>> GetManaCost(HtmlDocument doc)
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

        List<KeyValuePair<string, string>> GetCoolDown(HtmlDocument doc)
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
        string getSkillNameInSkillData(HtmlNode currentNode)
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
