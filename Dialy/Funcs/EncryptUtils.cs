using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Dialy.Funcs
{
    class EncryptUtils
    {
        //private static readonly string AesIV = @""; // 半角16文字のランダムな文字列にします。
        //private static readonly string AesKey = @""; // 半角32文字のランダムな文字列にします。
        private static readonly int KeySize = 256;
        private static readonly int BlockSize = 128;

        /**
         * @brief コンストラクタ
         *        ユーティリティクラスのため、インスタンス化させません。
         */
        private EncryptUtils() { }

        /**
         * @brief 文字列を暗号化します。
         *        暗号化した文字列は、Base64文字列で返却します。
         */
        internal static string AesEncrypt(string value)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Encoding.UTF8.GetBytes(value);
                var byteLength = byteValue.Length;

                var encryptor = aes.CreateEncryptor();
                var encryptValue = encryptor.TransformFinalBlock(byteValue, 0, byteLength);

                var base64Value = Convert.ToBase64String(encryptValue);
                return base64Value;
            }
        }

        /**
         * @brief 暗号化されたBase64文字列を復号化します。
         *
         * @param [in] encryptValue 暗号化されたBase64文字列
         * @return 復号化された文字列
         */
        internal static string AesDecrypt(string encryptValue)
        {
            using (var aes = GetAesManaged())
            {
                var byteValue = Convert.FromBase64String(encryptValue);
                var byteLength = byteValue.Length;

                var decryptor = aes.CreateDecryptor();
                var decryptValue = decryptor.TransformFinalBlock(byteValue, 0, byteLength);

                var stringValue = Encoding.UTF8.GetString(decryptValue);
                return stringValue;
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

            //var dpapiEncriptedIV = DpapiEncrypt(AesIV);
            //var dpapiEncriptedKey = DpapiEncrypt(AesKey);
            var DecriptedEnvIV = DpapiDecrypt(Environment.GetEnvironmentVariable("DiaryIV", EnvironmentVariableTarget.User));
            var DecriptedEnvKey = DpapiDecrypt(Environment.GetEnvironmentVariable("DiaryKey", EnvironmentVariableTarget.User));

            aes.IV = Encoding.UTF8.GetBytes(DecriptedEnvIV);
            aes.Key = Encoding.UTF8.GetBytes(DecriptedEnvKey);
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private readonly static byte[] entropy = new byte[] { 0x72, 0xa2, 0x12, 0x04 };
        private static string DpapiEncrypt(string value)
        {
            //文字列をバイト型配列に変換
            byte[] userData = System.Text.Encoding.UTF8.GetBytes(value);

            //暗号化する
            byte[] encryptedData = ProtectedData.Protect(
                userData, entropy,
                DataProtectionScope.CurrentUser);

            //暗号化されたデータを文字列に変換
            return System.Convert.ToBase64String(encryptedData);

        }

        private static string DpapiDecrypt(string encryptValue)
        {
            //文字列を暗号化されたデータに戻す
            byte[] encryptedData = System.Convert.FromBase64String(encryptValue);

            //復号化する
            byte[] userData = ProtectedData.Unprotect(
                encryptedData, entropy,
                DataProtectionScope.CurrentUser);

            //復号化されたデータを文字列に変換
            return System.Text.Encoding.UTF8.GetString(userData);

        }
    }
}