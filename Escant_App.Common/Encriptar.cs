using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace Escant_App.Common
{
    public class Encriptar
    {
        const string Key = "ABCDEFGHJKLMNOPQRSTUVWXYZABCDEF"; // must be 32 character
        const string IV = "ABCDEFGHIJKLMNOP"; // must be 16 character
        public static string aesKey = "J/PYjc1ftDFK5+77U1PB80v2TamokGap5yCIP2YI6tQ=";
        public static string iv = "gaOr3uvhZEwFeSbRHwlHcg==";
        public static string securityKey = "abcdefghijklmnop";
        
        public Encriptar()
        {

        }
        #region Encriptar

        /// <summary> 
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael). 
        /// Este es el mas simple posible, muchos de los datos necesarios los 
        /// definimos como constantes.
        /// </summary> 
        /// <param name="textoQueEncriptaremos">texto a encriptar</param> 
        /// <returns>Texto encriptado</returns> 
        public static string encriptar(string textoQueEncriptaremos)
        {
            return encriptar(textoQueEncriptaremos,
              "manijoda", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
            // "pass75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8"
        }

        /// <summary> 
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael) 
        /// </summary> 
        /// <returns>Texto encriptado</returns> 
        public static string encriptar(string textoQueEncriptaremos,
          string passBase, string saltValue, string hashAlgorithm,
          int passwordIterations, string initVector, int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,
              initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,
             CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();



            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();

            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        #endregion

        #region Desencriptar
        /// <summary> 
        /// Método para desencriptar un texto encriptado.
        /// </summary> 
        /// <returns>Texto desencriptado</returns> 
        public static string Desencriptar(string textoEncriptado)
        {

            return Desencriptar(textoEncriptado, "manijoda", "s@lAvz", "MD5",
              1, "@1B2c3D4e5F6g7H8", 128);
        }



        /// <summary> 
        /// Método para desencriptar un texto encriptado (Rijndael) 
        /// </summary> 
        /// <returns>Texto desencriptado</returns> 
        public static string Desencriptar(string textoEncriptado, string passBase,
          string saltValue, string hashAlgorithm, int passwordIterations,
          string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged()
            {

                Mode = CipherMode.CBC
            };

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,
              initVectorBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor,
              CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0,
              plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();


            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0,
              decryptedByteCount);
            return plainText;

        }

        #endregion

        #region Encriptar BCrypt
        public static string encriptarBcrypt(string textoQueEncriptaremos)
        {
            return BCrypt.Net.BCrypt.HashPassword(textoQueEncriptaremos, BCrypt.Net.BCrypt.GenerateSalt(10));
        }

        #endregion Encriptar BCrypt

        #region Encriptar AES
        public static string encriptarNodeC(string textoQueEncriptaremos)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
            aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] data = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(dest);
            }
        }
        #endregion Encriptar AES
        #region Desencriptar AES
        public static string desencriptarNodeC(string textoEncriptado)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                byte[] cipherText = Convert.FromBase64String(textoEncriptado);
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(IV);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(Key);
                ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
        #endregion Desencriptar AES
        #region EncriptaJCJ
        public static string encriptarJCJ(string textoQueEncriptaremos)
        {
            AesUtility aes = new AesUtility(aesKey);
            return aes.Encrypt(textoQueEncriptaremos, iv);
        }
        #endregion EncriptaJCJ
        #region desencriptaJCJ
        public static string desencriptarJCJ(string textoEncriptado)
        {
            AesUtility aes = new AesUtility(aesKey);
            return aes.Decrypt(textoEncriptado, iv);
        }
        #endregion desencriptaJCJ
        #region EncriptaCripto
        public static string encriptarCripto(string textoQueEncriptaremos)
        {            
            return CriptoJS.Encrypt(textoQueEncriptaremos, securityKey);
        }
        #endregion EncriptaCripto
        #region EncriptaCripto
        public static string desencriptarCripto(string textoEncriptado)
        {
            return CriptoJS.Decrypt(textoEncriptado, securityKey);
        }
        #endregion EncriptaCripto
        #region EncriptaAes
        public static string encriptarAes(string textoQueEncriptaremos)
        {
            return CriptoJS.EncryptStringAES(textoQueEncriptaremos);
        }
        #endregion EncriptaAes
        #region EncriptaRJC
        public static string encriptarRJC(string textoQueEncriptaremos)
        {
            byte[] plainText = Encoding.Unicode.GetBytes(textoQueEncriptaremos); // this is UTF-16 LE
            string cipherText;
            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes("Billing Key", Encoding.ASCII.GetBytes("Jose Manijoda"));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainText, 0, plainText.Length);
                        cs.Close();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }            
            return cipherText;
        }
        #endregion EncriptaRJC
    }
}
