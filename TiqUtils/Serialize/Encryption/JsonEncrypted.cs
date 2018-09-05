using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TiqUtils.Data;

namespace TiqUtils.Serialize.Encryption
{
    public static class JsonEncrypted
    {
        public static void CryptData<T>(this T data, string path, byte[] key)
        {
            try
            {
                if (data == null)
                    throw new ArgumentNullException();


                using (var memoryStream = new MemoryStream())
                {
                    SerializeToStream(memoryStream, data);
                    var eKey = GetProper16Key(key);
                    var iv = GetProper16Key(key, true);
                    File.WriteAllBytes(path, EncryptBytes(memoryStream.ToArray(), eKey, iv));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Something went wrong: {ex.Message}");
            }
        }

        public static string CryptDataToBase64<T>(this T data, byte[] key)
        {
            try
            {
                if (data == null)
                    throw new ArgumentNullException();

                using (var memoryStream = new MemoryStream())
                {
                    SerializeToStream(memoryStream, data);
                    var eKey = GetProper16Key(key);
                    var iv = GetProper16Key(key, true);
                    return Convert.ToBase64String(EncryptBytes(memoryStream.ToArray(), eKey, iv));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Something went wrong: {ex.Message}");
            }
        }

        private static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented};
            serializer.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            serializer.Error += (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            };

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        private static void SerializeToStream(Stream stream, object data)
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };
            serializer.Converters.Add(new StringEnumConverter { CamelCaseText = true });

            using (var sw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            using (var jsonTextReader = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonTextReader, data);
            }
        }

        public static T DecryptDataFromBase64<T>(string inputString, byte[] key)
        {
            try
            {
                return DecryptBytes<T>(Convert.FromBase64String(inputString), key);
            }
            catch
            {
                return default(T);
            }
        }

        private static T DecryptBytes<T>(byte[] data, byte[] key)
        {
            try
            {
                var eKey = GetProper16Key(key);
                var iv = GetProper16Key(key, true);
                using (var memoryStream = new MemoryStream(DecryptBytes(data, eKey, iv)))
                {
                    var obj = DeserializeFromStream<T>(memoryStream);
                    return obj;
                }

            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T DecryptData<T>(string path, byte[] key)
        {
            if (!File.Exists(path))
                return default(T);
                
            var fileArray = File.ReadAllBytes(path);
            return DecryptBytes<T>(fileArray, key);
        }

        #region CryptingZone
        internal static byte[] GetProper16Key(byte[] key, bool reverse = false)
        {
            if (key.Length >= 16)
                return reverse ? key.Reverse().Take(16).ToArray() : key.Take(16).ToArray();
            else
            {
                var key16 = new byte[16];
                var offset = 0;
                for (var i = 0; i < 16; i++)
                {
                    key16[i] = key[i - offset];
                    if (i - offset == key.Length - 1)
                    {
                        offset += key.Length;
                    }
                }
                return reverse ? key16.Reverse().ToArray() : key16;
            }
        }

        private static byte[] EncryptBytes(byte[] toEncrypt, byte[] key, byte[] iv)
        {
            using (var rmCrypto = new RijndaelManaged())
            using (var ms = new MemoryStream())
            using (var encryptor = rmCrypto.CreateEncryptor(key, iv))
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        private static byte[] DecryptBytes(byte[] toDecrypt, byte[] key, byte[] iv)
        {
            using (var rmCrypto = new RijndaelManaged())
            using (var ms = new MemoryStream())
            using (var decryptor = rmCrypto.CreateDecryptor(key, iv))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(toDecrypt, 0, toDecrypt.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
        #endregion
    }
}
