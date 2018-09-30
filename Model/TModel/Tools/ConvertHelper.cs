using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Location.TModel.Tools
{
    public static class ConvertHelper
    {
        public static T CloneObjectByBinary<T>(this T obj) where T : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            return formatter.Deserialize(stream) as T;
        }
    }
}
