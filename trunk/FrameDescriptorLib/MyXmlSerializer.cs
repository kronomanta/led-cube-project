using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FrameDescriptorLib
{
    public class MyXmlSerializer
    {
        public static void Serialize<T>(string filePath, T obj)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

         
            using (TextWriter writer = new StreamWriter(filePath))
            {
                try
                {
                    var s = new XmlSerializer(typeof(T));
                    s.Serialize(writer, obj);
                }
                catch (SerializationException e)
                {
                    Logger.Logger.WriteLine("Exception in serialization: " + e.Message);
                }
            } 
        }

        public static T Deserialize<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default(T);
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    var s = new XmlSerializer(typeof (T));
                    return (T) s.Deserialize(stream);
                }
                catch (SerializationException e)
                {
                    Logger.Logger.WriteLine("Exception in deserialization: " + e.Message);
                    return default(T);
                }
            }
        }
    }
}
