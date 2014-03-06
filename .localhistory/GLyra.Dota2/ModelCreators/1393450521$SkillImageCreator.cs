using GLyra.Dota2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLyra.Dota2.ModelCreators
{
    public class SkillImageCreator
    {
        public SkillImage InsertSkillImage(int skillId, string imageUrl)
        {
            SkillImage skillImage = new SkillImage();

            try
            {
                using (Dota2Entities ctx = new Dota2Entities())
                {
                    //Check if the image exists
                    if (!ctx.SkillImage.Any(si => si.Url == imageUrl))
                    {
                        skillImage.SkillId = skillId;
                        skillImage.Url = imageUrl;

                        ctx.SkillImage.Add(skillImage);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        skillImage = ctx.SkillImage.Where(si => si.Url == imageUrl).FirstOrDefault();
                        Console.WriteLine("SkillImage Already exists");                        
                    }
                }                
            }            
            catch (Exception)
            {
                //TODO implementar log
                throw;
            }

            return skillImage;
        }
    }
}
