using GLyra.Dota2.Converters;
using GLyra.Dota2.Repositories;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
        List<string> skillImages = new List<string>();
        List<string> skillNames = new List<string>();
        List<string> skillDescriptions = new List<string>();
        List<string> primaryStatsImages = new List<string>();
        List<string> primaryStatsValues = new List<string>();
        string biography;
        List<string> manaCostList = new List<string>();
        List<string> coolDownList = new List<string>();
        List<string> abilityCastType = new List<string>();
        List<string> skillTargetAffectedType = new List<string>();
        List<string> skillDamageType = new List<string>();
        string skillVideo;
        List<List<string>> skillRemainingValues = new List<List<string>>();
        HeroCreator heroCreator = new HeroCreator();

        public HeroDataConverter()
        {
            agilityPackHelper = new AgilityPackHelper();
            HtmlDocument doc = new HtmlDocument();

            doc = LoadHeroHtmlPage();

            heroesName = GetHeroesName();
            skillImages = GetSkillPortraits(doc);
            skillNames = GetSkillNames(doc);
            skillDescriptions = GetSkillDescriptions(doc);
            primaryStatsImages = GetPrimaryStatsImages(doc);
            primaryStatsValues = GetPrimaryStatsValues(doc);
            biography = GetBiography(doc);            
            manaCostList = GetManaCost(doc);
            coolDownList = GetCoolDown(doc);
            abilityCastType = GetAbilityCastType(doc);
            skillTargetAffectedType = GetSkillTargetAffectedType(doc);
            skillDamageType = GetSkillDamageType(doc);            
            skillVideo = GetSkillVideo(doc);
            skillRemainingValues = GetSkillRemainingValues(doc);

            //heroCreator.createHero(name, biography);
            for (int i = 0; i < skillNames.Count; i++)
            {
                heroCreator.createHeroSkill(skillNames[i], skillDescriptions[i], manaCostList, coolDownList, abilityCastType[i],
                    skillTargetAffectedType[i], skillDamageType[i], skillVideo);
            }
        }

        void createSkill(int heroId, string name, string description, List<string> manaCostList, List<string> coolDownList,
            string abilityCastType, string targetAffectedType, string damageType, string videoUrl)
        {
            SkillCreator sCretor = new SkillCreator();
            sCretor.createSkill(heroId, name, description, manaCostList, coolDownList, abilityCastType, targetAffectedType, damageType, videoUrl);
        }


        private HtmlDocument LoadHeroHtmlPage()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStream("http://www.dota2.com/hero/Storm_Spirit/?l=english"));            

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

            List<string> heroesNames = new List<string>();

            var heroesNameNode = doc.DocumentNode.SelectNodes("//*[(@id = 'filterName')]");

            

            foreach (var option in heroesNameNode)
            {
                foreach (var childNode in option.ChildNodes)
                {
                    var names =(childNode.Descendants("option")
                                        .Select(e => e.InnerText))
                                        .Where(s => !String.IsNullOrEmpty(s));
                    break;
                }
                //Get Heroes Names
                //var names = option.Descendants("option")
                //                    .Select(e => e.GetAttributeValue("value", null))
                //                    .Where(s => !String.IsNullOrEmpty(s));







            }

            return heroesNames;
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

        List<string> GetAbilityCastType(HtmlDocument doc)
        {
            var AbilityCastTypeList = new List<string>();

            var skillList = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");


            //Pega o valor do primeiro span(que é o que contém a informação do comportamento da skill
            foreach (var skillType in skillList)
            {
                var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
                AbilityCastTypeList.Add(spanList.First().InnerText);
            }

            return AbilityCastTypeList;
        }

        List<string> GetSkillTargetAffectedType(HtmlDocument doc)
        {
            var skillTargetAffectedTypeList = new List<string>();

            var abilityFooterBoxLeft = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

            //Pega o valor do ultimo span(que é o que contém a informação do tipo de dano da skill)
            foreach (var skillType in abilityFooterBoxLeft)
            {
                var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
                //Algumas skills não tem dano
                if (spanList.Count > 1)
                    if (spanList.Count == 3)
                        skillTargetAffectedTypeList.Add(spanList.ElementAt(1).InnerText);
                    //Verifica se o valro é damage, se nao for o valor é "Affects"
                    else if (spanList.Last().PreviousSibling.InnerText.Contains("AFFECTS"))
                        skillTargetAffectedTypeList.Add(spanList.Last().InnerText);
                    
                    else
                        skillTargetAffectedTypeList.Add(String.Empty);
            }

            return skillTargetAffectedTypeList;
        }

        List<string> GetSkillDamageType(HtmlDocument doc)
        {
            var damageType = new List<string>();

            var abilityFooterBoxLeft = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

            //Pega o valor do ultimo span(que é o que contém a informação do tipo de dano da skill)
            foreach (var skillType in abilityFooterBoxLeft)
            {
                var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
                //Algumas skills não tem dano
                if (spanList.Count > 1)
                    //Verifica se o valro é damage, se nao for o valor é "Affects"
                    if(spanList.Last().PreviousSibling.InnerText.Contains("DAMAGE") || spanList.Count == 3)
                        damageType.Add(spanList.Last().InnerText);
                else
                    damageType.Add(String.Empty);
            }

            return damageType;
        }

        List<List<string>> GetSkillRemainingValues(HtmlDocument doc)
        {
            var remainingValues = new List<List<string>>();

            var abilityFooterBoxRight = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxRight']");

            foreach (var remainingValue in abilityFooterBoxRight)
            {
                //TODO Terminar está função

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

        List<string> GetPrimaryStatsValues(HtmlDocument doc)
        {
            int expectedSize = 6;
            var primaryStatsList = new List<string>();

            var overviewStatVal = doc.DocumentNode.SelectNodes("//*[@class = 'overview_StatVal']");

            foreach (var primaryStat in overviewStatVal)
            {
                //Get PrimaryStat
                primaryStatsList.Add(primaryStat.InnerText);
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

        List<string> GetManaCost(HtmlDocument doc)
        {
            var manaCostList = new List<string>();

            var mana = doc.DocumentNode.SelectNodes("//*[@class = 'mana']");

            foreach (var manaCost in mana)
            {
                //Search for the span in the node
                var spanManaCost = manaCost.Descendants("span");
                //Remove the span that contains the text "Mana Cost: "
                manaCost.RemoveChild(spanManaCost.First());

                manaCostList.Add(manaCost.InnerText);                
            }
            
            return manaCostList;
        }

        List<string> GetCoolDown(HtmlDocument doc)
        {
            var coolDownList = new List<string>();

            var cooldown = doc.DocumentNode.SelectNodes("//*[@class = 'cooldown']");

            foreach (var cooldownTime in cooldown)
            {
                //Search for the span in the node
                var spanCoolDownTime = cooldownTime.Descendants("span");
                //Remove the span that contains the text "Cooldown: "
                cooldownTime.RemoveChild(spanCoolDownTime.First());

                coolDownList.Add(cooldownTime.InnerText);
            }

            return coolDownList;
        }

        protected Stream GetStream(string url)
        {            
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            return aResponse.GetResponseStream();
        }
    }
}
