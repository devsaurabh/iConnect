using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace iConnect.Server.Framework.Emoctions
{
    public class EmoctionParser
    {
        public List<Emoticons> GetAllEmoctions()
        {
            var emoctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Emoticons.xml");
            var xDoc = XDocument.Load(emoctionFile);
            var emoctions = xDoc.Elements().Select(element => new Emoticons
            {
                KeyCode = element.Attribute("KeyCode").Value,
                ImageCode = element.Attribute("ImageCode").Value,
                Description = element.Attribute("Description").Value,
                Group = element.Attribute("Group").Value,
            }).ToList();
            return emoctions;
        }

        public List<Emoticons> GetAutoReplaceEmoticons()
        {
            var emoctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Emoticons.xml");
            var xDoc = XDocument.Load(emoctionFile);
            var emoctions = xDoc.Element("Emoticons").Elements().Select(element => new Emoticons
            {
                KeyCode = element.Attribute("KeyCode").Value,
                ImageCode = element.Attribute("ImageCode").Value,
                Description = element.Attribute("Description").Value,
                Group = element.Attribute("Group").Value,
                AutoReplace = Convert.ToBoolean(element.Attribute("AutoReplace").Value)
            }).ToList();
            return emoctions.Where(t=> t.AutoReplace).ToList();
        }


        public void PrepareEmoctions()
        {
            var emoctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Emoticons.xml");
            var doc = new XDocument(new XElement("Emoticons"));
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Content/Emoticons");
            var emoticonFolder = new DirectoryInfo(path);
            
            foreach (var group in emoticonFolder.EnumerateDirectories())
            {
                foreach ( var emoticon in group.GetFiles())
                {
                    //File.Move(emoticon.FullName,emoticon.FullName.Replace("Emoji ",""));
                    var element = new XElement("Emoticon",
                        new XAttribute("KeyCode", Path.GetFileNameWithoutExtension(emoticon.Name)),
                        new XAttribute("ImageCode", emoticon.Name),
                        new XAttribute("Group", group.Name),
                        new XAttribute("Description", ""),
                        new XAttribute("AutoReplace", false)
                        );
                    doc.Element("Emoticons").Add(element);
                }
            }
            doc.Save(emoctionFile);
        }
       

        public void CreateEmoctionFile()
        {
            var emoctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Emoticons.xml");
            var doc =
                new XDocument(new XElement("Emoction", 
                    new XAttribute("KeyCode", ":)"),
                    new XAttribute("ImageCode", "smile"),
                    new XAttribute("Group", ""),
                    new XAttribute("Description", "")));
            doc.Save(emoctionFile);
        }
    }

    public class Emoticons
    {
        public string KeyCode { get; set; }
        public string ImageCode { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public bool AutoReplace { get; set; }
    }
}