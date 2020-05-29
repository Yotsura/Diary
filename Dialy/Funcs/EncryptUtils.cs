using System;
using System.Text;
using System.Security.Cryptography;

namespace Dialy.Funcs
{
    internal class EncryptUtils
    {
        private static readonly int KeySize = 256;
        private static readonly int BlockSize = 128;
        private readonly static byte[] entropy = new byte[] { 0x72, 0xa2, 0x12, 0x04 };
        private EncryptUtils() { }//ユーティリティクラスのため、インスタンス化させません。

        internal static string AesEncrypt(string value)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Encoding.UTF8.GetBytes(value);
                var encryptor = aes.CreateEncryptor();
                var encryptValue = encryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                return Convert.ToBase64String(encryptValue);
            }
        }

        internal static string AesDecrypt(string encryptValue)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Convert.FromBase64String(encryptValue);
                var decryptor = aes.CreateDecryptor();
                var decryptValue = decryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                return Encoding.UTF8.GetString(decryptValue);
            }
        }

        private static AesManaged GetAesManaged()
        {
            var aes = new AesManaged
            {
                KeySize = KeySize,
                BlockSize = BlockSize,
                Mode = CipherMode.CBC
            };
            var encriptedIV = Environment.GetEnvironmentVariable("DiaryIV", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(encriptedIV))
            {
                //環境変数がなければ作成・設定する
                encriptedIV = DpapiEncrypt(System.Web.Security.Membership.GeneratePassword(16, 0));
                Environment.SetEnvironmentVariable("DiaryIV", encriptedIV, EnvironmentVariableTarget.User);
            }
            aes.IV = Encoding.UTF8.GetBytes(DpapiDecrypt(encriptedIV));
            var encriptedKey = Environment.GetEnvironmentVariable("DiaryKey", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(encriptedKey))
            {
                //環境変数がなければ作成・設定する
                encriptedKey = DpapiEncrypt(System.Web.Security.Membership.GeneratePassword(32, 0));
                Environment.SetEnvironmentVariable("DiaryKey", encriptedKey, EnvironmentVariableTarget.User);
            }
            aes.Key = Encoding.UTF8.GetBytes(DpapiDecrypt(encriptedKey));
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private static string DpapiEncrypt(string value)
        {
            //文字列をバイト型配列に変換
            byte[] userData = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] encryptedData = ProtectedData.Protect(userData, entropy, DataProtectionScope.CurrentUser);
            //暗号化されたデータを文字列に変換
            return System.Convert.ToBase64String(encryptedData);
        }

        private static string DpapiDecrypt(string encryptValue)
        {
            //文字列を暗号化されたデータに戻す
            byte[] encryptedData = System.Convert.FromBase64String(encryptValue);
            byte[] userData = ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);
            //復号化されたデータを文字列に変換
            return System.Text.Encoding.UTF8.GetString(userData);
        }
    }
}