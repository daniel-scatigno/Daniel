using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
namespace NCPC.Utils
{
   public static class EncryptUtils
   {

      static readonly string PasswordHash = "nCpCs!t!";
      static readonly string SaltKey = "VEr16mnp";
      static readonly string VIKey = "NCbU37abTnqQ2LgJ";

      public static string Encrypt(string plainText)
      {
         byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

         byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
         var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
         var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

         byte[] cipherTextBytes;

         using (var memoryStream = new MemoryStream())
         {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
               cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
               cryptoStream.FlushFinalBlock();
               cipherTextBytes = memoryStream.ToArray();
               cryptoStream.Close();
            }
            memoryStream.Close();
         }
         return Convert.ToBase64String(cipherTextBytes);
      }

      public static string Decrypt(string encryptedText)
      {
         byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
         byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
         var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

         var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
         var memoryStream = new MemoryStream(cipherTextBytes);
         var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
         byte[] plainTextBytes = new byte[cipherTextBytes.Length];

         int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
         memoryStream.Close();
         cryptoStream.Close();
         return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
      }

      /// <summary>
      /// Transforma um password em um Hash para ser armazenado no banco de dados
      /// </summary>
      /// <param name="password">Senha</param>
      /// <returns></returns>
      public static string HashDeSenha(string senha)
      {
         return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(senha)));

      }

      /// <summary>
      /// Verifica se a senha esta correta
      /// </summary>
      /// <param name="password"></param>
      /// <param name="hashedPassword"></param>
      /// <returns></returns>
      public static bool SenhaCorreta(string password, string hashedPassword)
      {
         return HashDeSenha(password) == hashedPassword;
      }

   }

}