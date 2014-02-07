using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota.CentralDota
{
    public class AgilityPackHelper
    {
        /// <summary>
        /// Receives a nodeCollection and get all src values in the img nodes of the colection
        /// </summary>
        /// <param name="nodeCollection">NodeCollection</param>
        /// <param name="nodeCollectionName">The name that appears in the log(Generally the target node name)</param>
        /// <param name="expectedSize">The expected size of the return</param>
        /// <returns></returns>
        public List<string> GetImageUrls(HtmlNodeCollection nodeCollection, string nodeCollectionName, int expectedSize)
        {
            List<string> imgUrlsList = new List<string>();

            foreach (var imgUrl in nodeCollection)
            {
                //Get ImagesUrls
                var imgUrls = imgUrl.Descendants("img")
                                    .Select(e => e.GetAttributeValue("src", null))
                                    .Where(s => !String.IsNullOrEmpty(s));


                //Create a log if the return number isnot the expected
                if (imgUrls.Count() != expectedSize)
                    LogHandler.createWarningLog(nodeCollectionName, expectedSize, imgUrls.Count());

                //Caso o valor esperado seja maior que um, temos que adicionar todos os itens a lista
                if (expectedSize > 1)
                {
                    foreach (var url in imgUrls)
                    {
                        imgUrlsList.Add(url);
                    }
                }
                //Se o valor esperado for um, adiciona o primeiro da lista
                else
                    imgUrlsList.Add(imgUrls.First());
            }

            return imgUrlsList;
        }
    }
}
