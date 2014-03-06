using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
    public class HeroPortraitCreator
    {
        public HeroPortrait InsertHeroPortrait(int heroId, string imageUrl)
        {
            HeroPortrait HeroPortrait = new HeroPortrait();

            try
            {
                using (Dota2Entities ctx = new Dota2Entities())
                {
                    //Check if the image exists
                    if (!ctx.HeroPortrait.Any(si => si.Url == imageUrl))
                    {
                        HeroPortrait.HeroId = heroId;
                        HeroPortrait.Url = imageUrl;

                        ctx.HeroPortrait.Add(HeroPortrait);
                        ctx.SaveChanges();
                        Console.WriteLine("Image " + HeroPortrait.Url + " Added");
                    }
                    else
                    {
                        HeroPortrait = ctx.HeroPortrait.Where(si => si.Url == imageUrl).FirstOrDefault();
                        Console.WriteLine("HeroPortrait Already exists");
                    }
                }
            }
            catch (Exception)
            {
                //TODO implementar log
                throw;
            }

            return HeroPortrait;
        }
    }
}
