using System.Xml.Linq;

namespace AkiApplication.Data.Models
{
    public class XmlHandler
    {

        public void CreateXmlFile(string filePath, string Comment, XElement element)
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "true"),
                new XComment(Comment),
                element);

            xml.Save(filePath);
        }

        public XDocument GetXmlFile(string path)
        {
            // ファイルへのパス
            return XDocument.Load(path);
        }
    }

}
