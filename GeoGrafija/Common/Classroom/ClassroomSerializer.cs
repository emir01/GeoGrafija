using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Classroom
{
    public static class ClassroomSerializer
    {
        public static Classroom DesirializeFromXml(string xml)
        {

            try
            {
                var x = new XmlSerializer(typeof(Classroom));
                TextReader reader = new StringReader(xml);

                var results =
                x.Deserialize(reader);

                return results as Classroom;
            }
            catch (Exception ex)
            {
                return new Classroom();
            }
        }

        public static string SerializeClassroomToXml(Classroom result)
        {
            try
            {
                var x = new XmlSerializer(typeof(Classroom));
                var writer = new StringWriter();

                x.Serialize(writer, result);

                var serialized = writer.ToString();

                return serialized;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
