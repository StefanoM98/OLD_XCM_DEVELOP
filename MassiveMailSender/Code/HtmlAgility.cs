using System.Linq;
using HtmlAgilityPack;

namespace MassiveMailSender.Code
{
    public class HtmlAgility
    {
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

        public string GetContentOfTag(string htmlText, string tag)
        {
            doc.LoadHtml(htmlText);
            return doc.DocumentNode.Descendants().Where(x => x.Name == tag).First().InnerHtml;

        }

        public string AppendTextInTag(string htmlText, string tag, string text)
        {
            HtmlNodeCollection nodeCollection = null;
            doc.LoadHtml(htmlText);
            if (tag == "style")
            {
                nodeCollection = doc.DocumentNode.SelectNodes("//style");

            }
            else if (tag == "body")
            {
                nodeCollection = doc.DocumentNode.SelectNodes("//body");
            }
            foreach (var e in nodeCollection)
            {
                e.InnerHtml += text;
            }
            return doc.DocumentNode != null ? doc.DocumentNode.InnerHtml : "";
        }


        public string ReplaceContentOfImgSrc(string htmlText, string newSrc)
        {
            var response = "";
            doc.LoadHtml(htmlText);
            var imageNodes = doc.DocumentNode.SelectNodes("//img").ToList();
            if(imageNodes.Count > 1)
            {
                return "";
            }
            var imgNode = imageNodes.First();
            imgNode.SetAttributeValue("src", newSrc);
            response = doc.DocumentNode.InnerHtml;
            return doc.DocumentNode != null ? doc.DocumentNode.InnerHtml : "";

        }
    }
}
