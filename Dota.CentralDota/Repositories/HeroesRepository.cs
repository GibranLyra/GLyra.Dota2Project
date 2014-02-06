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

        public HeroesRepository()
        {
            // load snippet
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet = LoadHtmlSnippetFromFile();

            // extract hrefs
            List<string> hrefTags = new List<string>();
            hrefTags = ExtractAllAHrefTags(htmlSnippet);

        }


        /// <summary>
        /// Load the html snippet from the txt file
        /// </summary>
        private HtmlDocument LoadHtmlSnippetFromFile()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(GetStream("http://www.dota2.com/hero/Earthshaker/"));            

            return doc;
        }

        /// <summary>
        /// Extract all anchor tags using HtmlAgilityPack
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<string> ExtractAllAHrefTags(HtmlDocument doc)
        {
            List<string> habilityImageUrl = new List<string>();

            var allElementsWithClassFloat = doc.DocumentNode.SelectNodes("//*[contains(@class,'abilityIconHolder2')]");

            foreach (var abilityIconHolder in allElementsWithClassFloat)
            {
                var urls = abilityIconHolder.Descendants("img")
                                .Select(e => e.GetAttributeValue("src", null))
                                .Where(s => !String.IsNullOrEmpty(s));

                if (urls.Count() > 1)
                {
                    Console.WriteLine("Warning, node " + abilityIconHolder.Id + " has more than one hability Image");

                }
                else
                {
                    habilityImageUrl.Add(urls.First());
                }
                
            }


            return habilityImageUrl;
        }

        protected Stream GetStream(string url)
        {            
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            return aResponse.GetResponseStream();
        }
    }
}
