using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace FrameDescriptorLib
{
    public class DataSerializer
    {
        public static void SaveFrameDescriptor(string filePath, IList<FrameDescriptor> obj)
        {
            Save(filePath, obj);
        }

        public static IList<FrameDescriptor> LoadFrameDescriptor(string filePath)
        {
            return Load<IList<FrameDescriptor>>(filePath);
        }

        public static void Save<T>(string filePath, T obj)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    var bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, obj);
                }
                catch (SerializationException e)
                {
                    Logger.Logger.WriteLine("Exception in serialization: " + e.Message);
                }
            }

        }

        public static T Load<T>(string filePath) where T : class
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    var bformatter = new BinaryFormatter();
                    return (T)bformatter.Deserialize(stream);
                }
                catch (SerializationException e)
                {
                    Logger.Logger.WriteLine("Exception in deserialization: " + e.Message);
                    return null;
                }
            }
        }
    }
}

