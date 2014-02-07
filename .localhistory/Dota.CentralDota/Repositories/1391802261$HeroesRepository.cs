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
    class HeroesRepository
    {
        AgilityPackHelper agilityPackHelper;

        public HeroesRepository()
        {
            var skillImages = new List<string>();
            var skillNames = new List<string>();
            var skillDescriptions = new List<string>();
            var primaryStatsImages = new List<string>();
            var primaryStatsValues = new List<string>();
            string biography;
            var manaCosts = new List<string>();
            var coolDowns = new List<string>();
            var skillBehaviours = new List<string>();
            var skillDamageType = new List<string>();
            //TODO Armazenar quantidade de skills que o heroi tem
            agilityPackHelper = new AgilityPackHelper();
            // load snippet
            HtmlDocument doc = new HtmlDocument();
            doc = LoadHtmlSnippetFromFile();
           
            skillImages = GetSkillPortraits(doc);
            skillNames = GetSkillNames(doc);
            skillDescriptions = GetSkillDescriptions(doc);
            primaryStatsImages = GetPrimaryStatsImages(doc);
            primaryStatsValues = GetPrimaryStatsValues(doc);
            biography = GetBiography(doc);
            //TODO Stats(Atributos)
            manaCosts = GetManaCost(doc);
            coolDowns = GetCoolDown(doc);
            skillBehaviours = GetSkillBehaviour(doc);
            skillDamageType = GetSkillDamageType(doc);
        }

        private HtmlDocument LoadHtmlSnippetFromFile()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStream("http://www.dota2.com/hero/Earthshaker/"));            

            return doc;
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

        List<string> GetSkillBehaviour(HtmlDocument doc)
        {
            var passiveSkillList = new List<string>();

            var skillList = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");


            //Pega o valor do primeiro span(que é o que contém a informação do comportamento da skill
            foreach (var skillType in skillList)
            {
                var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
                passiveSkillList.Add(spanList.First().InnerText);
            }

            return passiveSkillList;
        }

        List<string> GetSkillDamageType(HtmlDocument doc)
        {
            var damageType = new List<string>();

            var skillList = doc.DocumentNode.SelectNodes("//*[@class = 'abilityFooterBoxLeft']");

            //Pega o valor do ultimo span(que é o que contém a informação do tipo de dano da skill)
            foreach (var skillType in skillList)
            {
                var spanList = skillType.SelectNodes(".//*[@class = 'attribVal']");
                //Algumas skills não tem dano
                if (spanList.Count > 1)
                    damageType.Add(spanList.Last().InnerText);
                else
                    damageType.Add(String.Empty);
            }

            return damageType;
        }

        //TODO Get the description data of the skills

        List<string> GetSkillVideo(HtmlDocument doc)
        {
            int expectedSize = 1;
            var skillVideoList = new List<string>();

            var abilityVideoContainer = doc.DocumentNode.SelectNodes("//*[@class = 'abilityVideoContainer']");

            foreach (var skillDescription in abilityVideoContainer)
            {
                //Get the skillDescription
                var skillDescriptions = skillDescription.Descendants("p")
                                    .Select(n => n.InnerHtml);


                if (skillDescriptions.Count() > expectedSize)
                    LogHandler.createWarningLog("overviewAbilityRowDescription-SkillDescription", expectedSize, skillDescriptions.Count());

                skillVideoList.Add(skillDescriptions.First());
            }

            return skillVideoList;
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
