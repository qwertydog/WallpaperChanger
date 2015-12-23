using System.Net;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WallpaperChanger
{
    public class Subreddit
    {

        public enum LinkType : int
        {
            Hot,
            New,
            Random,
            Top
        }

        public string name;
        public List<Link> urls;

        public List<Link> RetrieveLinks()
        {

            string sURL = Link();

            List<Link> links = new List<Link>();

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            wrGETURL.Method = "GET";

            Stream objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string Json = objReader.ReadToEnd();

            JObject link = JObject.Parse(Json);

            IList<JToken> results = link["data"]["children"].Children()["data"].ToList();

            // For each reddit link retrieved
            foreach (JToken result in results)
            {
                Link searchResult = JsonConvert.DeserializeObject<Link>(result.ToString());

                // Make sure only image links are included
                if (searchResult.Url.Contains("imgur") ||
                    searchResult.Url.Contains(".bmp") ||
                    searchResult.Url.Contains(".jpg") ||
                    searchResult.Url.Contains(".jpeg")
                    )
                {
                    searchResult.Print();


                    Match match = Regex.Match(searchResult.Url, @"[^/]+$");
                    Match match2 = Regex.Match(match.Value, @"^[^\.]+");

                    searchResult.ID = match2.Value;

                    links.Add(searchResult);
                }
            }

            return links;

        }

        public string Link(LinkType linkType = LinkType.Hot, bool AddJsonExtension = true)
        {

            string returnString = @"http://www.reddit.com/r/";

            returnString += name;
            returnString += @"/";
            returnString += linkType.ToString().ToLower();

            if (!AddJsonExtension)
            {

                return returnString;

            }
            else
            {
                return returnString + @".json";

            }
        }

        /*public static void loadSubreddits(List<Subreddit> subList, UserDetails userInfo)
        {
            if (userInfo != null)
            {

                subList = userInfo.RetrieveUserSubs();


            }
            else
            {

                /*Console.Write("Enter subreddits seperated by a comma: ");

                string line = Console.ReadLine();

                string[] subreddits = line.Split(',');

                foreach (string sub in subreddits)
                {
                    Subreddit subreddit = new Subreddit();

                    subreddit.name = sub;

                    subList.Add(subreddit);
                }

            }
        }*/
    }
}
