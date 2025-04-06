using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        // Update these URLs after you deploy your XML/XSD files via GitHub Pages.
        public static string xmlURL = "https://snesan0821.github.io/cse445_a4/Hotels.xml"; // Q1.2
        public static string xmlErrorURL = "https://snesan0821.github.io/cse445_a4/HotelsErrors.xml"; // Q1.3
        public static string xsdURL = "https://snesan0821.github.io/cse445_a4/Hotels.xsd"; // Q1.1

        public static void Main(string[] args)
        {
            // Validate the correct XML file.
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine("Verification result for valid XML:");
            Console.WriteLine(result);
            Console.WriteLine();

            // Validate the error-injected XML file.
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine("Verification result for error XML:");
            Console.WriteLine(result);
            Console.WriteLine();

            // Convert valid XML to JSON.
            result = Xml2Json(xmlURL);
            Console.WriteLine("JSON conversion result:");
            Console.WriteLine(result);
        }

        // Q2.1: Validate XML against XSD.
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Load the XSD schema.
                XmlSchemaSet schemas = new XmlSchemaSet();
                using (XmlReader schemaReader = XmlReader.Create(xsdUrl))
                {
                    schemas.Add(null, schemaReader);
                }

                // Set up XML reader settings for validation.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemas;
                settings.ValidationType = ValidationType.Schema;
                string errorMsg = "";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errorMsg += $"Line {e.Exception.LineNumber}, Position {e.Exception.LinePosition}: {e.Message}\n";
                };

                // Read the XML document to trigger validation.
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return string.IsNullOrEmpty(errorMsg) ? "No Error" : errorMsg;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }

        // Q2.2: Convert XML to JSON.
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                // Load the XML document.
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                // Convert XML to JSON using Newtonsoft.Json.
                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                // Replace attribute prefix "@" with "_" to match the required JSON structure.
                jsonText = jsonText.Replace("\"@", "\"_");
                return jsonText;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
    }
}
