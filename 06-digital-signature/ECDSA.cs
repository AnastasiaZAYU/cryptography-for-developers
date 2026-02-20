using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Digests;

namespace Digital_Signature
{
    class ECDSA
    {
        public bool Verify(string message, byte[] signature, AsymmetricKeyParameter pk)
        {
            var verifier = SignerUtilities.GetSigner("SHA-256withECDSA");
            verifier.Init(false, pk);
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            verifier.BlockUpdate(bytes, 0, bytes.Length);
            return verifier.VerifySignature(signature);
        }

        public byte[] Sign(string message, AsymmetricKeyParameter sk)
        {
            var signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, sk);
            var bytes = System.Text.Encoding.UTF8.GetBytes(message);
            signer.BlockUpdate(bytes, 0, bytes.Length);
            return signer.GenerateSignature();
        }

        public byte[] Hash(string message)
        {
            var sha3 = new Sha3Digest(256);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            sha3.BlockUpdate(data, 0, data.Length);
            byte[] hash = new byte[sha3.GetDigestSize()];
            sha3.DoFinal(hash, 0);
            return hash;
        }

        public (AsymmetricKeyParameter, AsymmetricKeyParameter) KeyGen()
        {
            var secure = new SecureRandom();
            var keyGen = new Org.BouncyCastle.Crypto.Generators.ECKeyPairGenerator();
            var genParam = new Org.BouncyCastle.Crypto.KeyGenerationParameters(secure, 256);
            keyGen.Init(genParam);
            var keyPair = keyGen.GenerateKeyPair();
            var pk = keyPair.Public;
            var sk = keyPair.Private;
            return (sk, pk);
        }

        public string ConvertToPEM(AsymmetricKeyParameter key)
        {
            using (var stringWriter = new StringWriter())
            {
                var pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(key);
                pemWriter.Writer.Flush();
                return stringWriter.ToString();
            }
        }

        public AsymmetricKeyParameter InputKey()
        {
            var key_build = new StringBuilder();
            string str = "";
            do
            {
                str = Console.ReadLine();
                key_build.AppendLine(str);
            } while (!str.Contains("-----END"));
            var pemKey = key_build.ToString();
            using (var stringReader = new StringReader(pemKey))
            {
                var pemReader = new PemReader(stringReader);
                var pemObject = pemReader.ReadObject();

                if (pemObject is AsymmetricCipherKeyPair)
                {
                    return ((AsymmetricCipherKeyPair)pemObject).Private;
                }
                else if (pemObject is AsymmetricKeyParameter)
                {
                    return (AsymmetricKeyParameter)pemObject;
                }
                throw new InvalidOperationException("Failed to parse PEM key");
            }
        }
    }
}
