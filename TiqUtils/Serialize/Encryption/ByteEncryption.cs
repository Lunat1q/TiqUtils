using System.IO;
using System.Security.Cryptography;

namespace TiqUtils.Serialize.Encryption
{
    public static class ByteEncryption
    {
        public static byte[] Encrypt(this byte[] toEncrypt, byte[] key, byte[] iv)
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
        public static byte[] Decrypt(this byte[] toDecrypt, byte[] key, byte[] iv)
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
    }
}