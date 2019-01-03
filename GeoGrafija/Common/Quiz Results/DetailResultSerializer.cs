using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Quiz_Results
{
    public static class DetailResultSerializer
    {
        public static QuizDetailedResults DesirializeFromXml(string xml)
        {
            var x = new XmlSerializer(typeof(QuizDetailedResults));
            TextReader reader = new StringReader(xml);
            
            var results =
            x.Deserialize(reader);

            return results as QuizDetailedResults;
        }

        public static string SerializeDetailedQuizResultToXml(QuizDetailedResults result)
        {
            var x = new XmlSerializer(typeof(QuizDetailedResults));
            var writer = new StringWriter();

            x.Serialize(writer, result);

            var serialized = writer.ToString();

            return serialized;
        }
    }
}
