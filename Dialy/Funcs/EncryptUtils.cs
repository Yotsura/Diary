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
                var byteValue = Encoding.UTF8.GetBytes("0123456789ABCDEF" + value);
                var encryptor = aes.CreateEncryptor();
                var encryptedValue = encryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                return Convert.ToBase64String(encryptedValue);
            }
        }

        internal static string AesDecrypt(string encryptedValue)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Convert.FromBase64String(encryptedValue);
                var decryptor = aes.CreateDecryptor();
                var decryptValue = decryptor.TransformFinalBlock(byteValue, 0, byteValue.Length);
                return Encoding.UTF8.GetString(decryptValue, 16, decryptValue.Length - 16);
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
            //IV初期ベクトルはあくまで同じ平文・鍵で別の暗号文を生成するためのもの。
            aes.IV = Encoding.UTF8.GetBytes(System.Web.Security.Membership.GeneratePassword(16, 0));
            var encryptedKey = Environment.GetEnvironmentVariable("DiaryKey", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(encryptedKey))
            {
                //環境変数がなければ作成・設定する
                encryptedKey = DpapiEncrypt(System.Web.Security.Membership.GeneratePassword(32, 0));
                Environment.SetEnvironmentVariable("DiaryKey", encryptedKey, EnvironmentVariableTarget.User);
            }
            aes.Key = Encoding.UTF8.GetBytes(DpapiDecrypt(encryptedKey));
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

        private static string DpapiDecrypt(string encryptedValue)
        {
            //文字列を暗号化されたデータに戻す
            byte[] encryptedData = System.Convert.FromBase64String(encryptedValue);
            byte[] userData = ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);
            //復号化されたデータを文字列に変換
            return System.Text.Encoding.UTF8.GetString(userData);
        }
    }
}