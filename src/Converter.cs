using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace DataFormatConverter
{
    public class TOrder
    {
        public string AccountId { get; set; }
        public int InstrumentId { get; set; }
        public int TNumber { get; set; }
        public int TVersion { get; set; }
        public string TAction { get; set; }
        public string CorrectFlag { get; set; }
        public string CancelFlag { get; set; }
        public string NDDFlag { get; set; }
    }

    public class Converter
    {
        public IEnumerable<TOrder> CSV_to_Order(string csv_doc)
        {

            using (StringReader reader = new StringReader(csv_doc))
            {
                string line;
                line = reader.ReadLine(); // Move past first line; not doing anything with header yet
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(new[] { '|' });
                    yield return new TOrder()
                    {
                        AccountId = data[0],
                        InstrumentId = int.Parse(data[1]),
                        TNumber = int.Parse(data[2]),
                        TVersion = int.Parse(data[3]),
                        TAction = data[4],
                        CorrectFlag = data[50],
                        CancelFlag = data[51],
                        NDDFlag = data[98]
                    };
                }
            }
        }

        public string XML_to_CSV(string xml_doc)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml_doc);

                var root = doc.FirstChild;
                var csv_doc = new StringBuilder();

                for (int row = 0; row < root.ChildNodes.Count; row++)
                {
                    List<string> Attributes = new List<string>();
                    for (int attribute = 0; attribute < root.ChildNodes.Item(row).ChildNodes.Count; attribute++)
                    {
                        if (root.ChildNodes.Item(row).ChildNodes.Item(attribute).NodeType is XmlNodeType.Element)
                        {
                            if (String.IsNullOrEmpty(root.ChildNodes.Item(row).ChildNodes.Item(attribute).InnerText))
                            {
                                Attributes.Add("NULL");
                            }
                            else
                            {
                                Attributes.Add(root.ChildNodes.Item(row).ChildNodes.Item(attribute).InnerText);
                            }
                        }
                    }
                    csv_doc.Append(String.Join(",", Attributes));
                    csv_doc.Append("\n");
                }
                return (csv_doc.ToString());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"XML Exception: {ex}");
                return string.Empty;
            }
        }

        public string XML_to_JSON(string xml_doc)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml_doc);
            var jsonText = JsonConvert.SerializeXmlNode(doc);

            return jsonText;
        }
    }
}