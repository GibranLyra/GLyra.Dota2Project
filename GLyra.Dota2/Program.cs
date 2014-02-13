using Dota.CentralDota.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota.CentralDota
{
    class Program
    {
        

        static void Main(string[] args)
        {            
            //var baseRepo = new BaseRepositoryApi( ConfigurationManager.AppSettings.Get("TestSteamId"));
            //baseRepo.getHeroes();
            var heroesRepo = new HeroDataConverter();
        }
    }
}
