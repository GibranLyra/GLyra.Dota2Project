using System.Net;
using Newtonsoft.Json;
using System;
using PortableSteam;
using System.Configuration;

namespace Dota.CentralDota
{
    public class BaseRepositoryApi
    {
        #region Properties

        public enum APIVersion { v001, v0001, v0002 };

        public string SteamId { get; set; }
        public string SteamInterface { get; set; }

        private string _urlApiSteam;
        string UrlApiSteam
        {
            get { return _urlApiSteam; }
        }

        private string _steamApiKey;
        string SteamApiKey
        {
            get { return _steamApiKey; }
        }
        #endregion

        public BaseRepositoryApi(string steamId)
        {
            this.SteamId = steamId;
            _urlApiSteam = ConfigurationManager.AppSettings.Get("UrlApiSteam");
            _steamApiKey = ConfigurationManager.AppSettings.Get("SteamApiKey");
            bool isTesting = bool.Parse(ConfigurationManager.AppSettings.Get("Testing"));

            //In case of test use Beta TEST API(_205790)
            if (isTesting)
                SteamInterface = "_205790";
            else
                SteamInterface = "_570";            
        }

        public T GetAPIResult<T>(string steamInterface, string method, APIVersion apiVersion, string steamId) where T : new()
        {
            using (var w = new WebClient())
            {
                var jsonData = string.Empty;
                string url = string.Format("{0}/{1}/{2}/{3}/{4}", UrlApiSteam, steamInterface, method, apiVersion, "?key=" + SteamApiKey, SteamId);

                try
                {
                    jsonData = w.DownloadString(url);
                    
                }
                catch (Exception e)
                {
                    throw e;
                }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(jsonData) ? JsonConvert.DeserializeObject<T>(jsonData) : new T();
            }
        }

        //Sample Url: http://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1//?key=7CC0589DA258E2792540EDCAF588D63E
        public void getMatchHistory()
        {
            SteamInterface = "IDOTA2Match" + SteamInterface;
            GetAPIResult<object>(SteamInterface, "GetMatchHistory", APIVersion.v0001, this.SteamId);
        }

        //Sample Url: https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/
        public void getHeroes()
        {
            SteamInterface = "IEconDota2" + SteamInterface;
            //TODO create a localization rule: https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=<key>&language=en_us
            object heroResult = GetAPIResult<object>(SteamInterface, "GetHeroes", APIVersion.v0001, this.SteamId);
        }
    }
}
