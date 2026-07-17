using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public class IdentityTokenDataModel
    {
        private static readonly byte[] EncryptKey = new byte[] { 26, 98, 30, 46, 15, 126, 247, 251, 254, 0, 12, 78, 45, 165, 3, 210, 65, 74 };
        private static readonly byte[] SaltBytes = { 251, 2, 13, 194, 45, 61, 7, 87 };

        public Guid IdAccount { get; private set; }

        public string IdentityToken { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime ExpiresIn { get; private set; }

        public bool IsExpires => DateTime.UtcNow > ExpiresIn;

        public IdentityTokenDataModel(Guid idAccount/*, TokenResponse token*/)
        {
            //if (token == null) throw new ArgumentNullException(nameof(token));

            //IdAccount = idAccount;
            //IdentityToken = token.IdentityToken;
            //RefreshToken = token.RefreshToken;
            //ExpiresIn = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
        }

        public static IdentityTokenDataModel? Deserialize(string serializeToken)
        {
            try
            {
                var token = Convert.FromBase64String(serializeToken);

                using (MemoryStream ms = new MemoryStream(token))
                {
                    using (var aes = Aes.Create())
                    {
                        var key = Rfc2898DeriveBytes.Pbkdf2(EncryptKey, SaltBytes, 1000, HashAlgorithmName.SHA256, (aes.KeySize + aes.BlockSize) / 8);

                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.Key = key[..(aes.KeySize / 8)];
                        aes.IV = key[(aes.KeySize / 8)..];

                        using (var csDecrypt = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            //if (System.Diagnostics.Debugger.IsAttached)
                            //System.Diagnostics.Debugger.Break();

                            //return Serializer.Deserialize<IdentityTokenDataModel>(csDecrypt);
                            throw new NotImplementedException();
                        }
                    }
                }
            }
            catch (CryptographicException ex)
            {
                CrossLoggerHost.GetLogger<IdentityTokenDataModel>().LogError(ex, "Error deserializing IdentityTokenDataModel");

                return null;
            }
            catch (SerializationException ex)
            {
                CrossLoggerHost.GetLogger<IdentityTokenDataModel>().LogError(ex, "Error deserializing IdentityTokenDataModel");

                return null;
            }
        }

        public string Serialize()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    var key = Rfc2898DeriveBytes.Pbkdf2(EncryptKey, SaltBytes, 1000, HashAlgorithmName.SHA256, (aes.KeySize + aes.BlockSize) / 8);

                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key[..(aes.KeySize / 8)];
                    aes.IV = key[(aes.KeySize / 8)..];

                    using (var csEncrypt = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        //Serializer.Serialize<IdentityTokenDataModel>(csEncrypt, this);
                        throw new NotImplementedException();
                    }
                }

                //if (System.Diagnostics.Debugger.IsAttached)
                //System.Diagnostics.Debugger.Break();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
