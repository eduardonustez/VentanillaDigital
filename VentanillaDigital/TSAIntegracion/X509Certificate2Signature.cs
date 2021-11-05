using iText.Signatures;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TSAIntegracion
{
    class X509Certificate2Signature : IExternalSignature
    {
        private string _hashAlgorithm;
        private string _encryptionAlgorithm;
        private X509Certificate2 _certificate;

        public X509Certificate2Signature(X509Certificate2 certificate,
            string hashAlgorithm )
        {
            if (!certificate.HasPrivateKey)
                throw new ArgumentException("No private key.");
            _certificate = certificate;
            _hashAlgorithm = DigestAlgorithms.GetDigest(
                DigestAlgorithms.GetAllowedDigest(hashAlgorithm));
            if (certificate.PrivateKey is RSACryptoServiceProvider)
                _encryptionAlgorithm = "RSA";
            else if (certificate.PrivateKey is DSACryptoServiceProvider)
                _encryptionAlgorithm = "DSA";

            else if (certificate.PrivateKey is RSACng)
                _encryptionAlgorithm = "RSA";
            else
                throw new ArgumentException($"Unknown encryption algorithm {certificate.PrivateKey}");
        }

        public virtual byte[] Sign(byte[] message)
        {
            if (_certificate.PrivateKey is RSACryptoServiceProvider)
            {
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)_certificate.PrivateKey;
                return rsa.SignData(message, _hashAlgorithm);
            }
            else if (_certificate.PrivateKey is DSACryptoServiceProvider)
            {
                DSACryptoServiceProvider dsa = (DSACryptoServiceProvider)_certificate.PrivateKey;
                return dsa.SignData(message);
            }
            else
            {
                RSA rsa = (RSA)_certificate.PrivateKey;
                return rsa.SignData(message, new HashAlgorithmName(_hashAlgorithm), RSASignaturePadding.Pkcs1);
            }
        }

        public virtual string GetHashAlgorithm()
        {
            return _hashAlgorithm;
        }

        public virtual string GetEncryptionAlgorithm()
        {
            return _encryptionAlgorithm;
        }
    }
}
