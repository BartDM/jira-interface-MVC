using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace JiraInterface.Helpers
{
    public class Data
    {
        public static List<string> GetChapters()
        {
            var mapPath = AppDomain.CurrentDomain.GetData("DataDirectory");
            XDocument doc = XDocument.Load(Path.Combine(mapPath.ToString(), "Chapters.xml"));
            var items = doc.Root.Elements("Item").Select(e => e.Attribute("nr").Value + " " + e.Value).ToList();
            return items;
        }

        public static Tuple<string,string,string> GetJiraCredentials()
        {
            var mapPath = AppDomain.CurrentDomain.GetData("DataDirectory");
            XDocument doc = XDocument.Load(Path.Combine(mapPath.ToString(), "JiraCredentials.xml"));
            var url = doc.Root.Element("Url") != null ? doc.Root.Element("Url").Value: string.Empty;
            var user = doc.Root.Element("User") != null ? doc.Root.Element("User").Value: string.Empty;
            var password = doc.Root.Element("Password") != null ? doc.Root.Element("Password").Value : string.Empty;

            return new Tuple<string,string, string>(url,user,password);
        }

        public static Tuple<string,string> GetEmailAddresses()
        {
            var mapPath = AppDomain.CurrentDomain.GetData("DataDirectory");
            XDocument doc = XDocument.Load(Path.Combine(mapPath.ToString(), "JiraCredentials.xml"));
            var mailFrom = doc.Root.Element("MailFrom") != null ? doc.Root.Element("MailFrom").Value : string.Empty;
            var mailTo = doc.Root.Element("MailTo") != null ? doc.Root.Element("MailTo").Value : string.Empty;

            return new Tuple<string, string>(mailFrom,mailTo);
        }
    }
}