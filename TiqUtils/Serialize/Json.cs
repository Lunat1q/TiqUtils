using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TiqUtils.Serialize
{
    public static class Json
    {
        public static bool SerializeDataJson<T>(this T data, string path, bool objectHandling = false)
        {
            try
            {
                var p = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(p))
                    Directory.CreateDirectory(p);
                var js = new JsonSerializer();
                js.Converters.Add(new JavaScriptDateTimeConverter());
                js.NullValueHandling = NullValueHandling.Ignore;
                js.Formatting = Formatting.Indented;
                if (objectHandling)
                    js.TypeNameHandling = TypeNameHandling.Objects;

                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                    js.Serialize(writer, data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string SerializeDataToString<T>(this T data, bool objectHandling = false)
        {
            try
            {
                var js = new JsonSerializer();
                js.Converters.Add(new JavaScriptDateTimeConverter());
                js.NullValueHandling = NullValueHandling.Ignore;
                js.Formatting = Formatting.Indented;
                if (objectHandling)
                    js.TypeNameHandling = TypeNameHandling.Objects;

                var ret = String.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                        js.Serialize(writer, data);
                    ret = sw.ToString();
                }
                return ret;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static T DeserializeDataFromString<T>(this string inputString)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<T>(inputString);
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T DeserializeDataFromFile<T>(string path, bool objectHandling = false)
        {
            if (!File.Exists(path))
                return default(T);
            try
            {
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    if (objectHandling)
                        serializer.TypeNameHandling = TypeNameHandling.Objects;

                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    var data = (T)serializer.Deserialize(file, typeof(T));
                    return data;
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
