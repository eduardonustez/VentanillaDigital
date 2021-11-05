using Infraestructura.KeyManager;
using iText.Signatures;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Infraestructura.KeyManager.Models;
using System.Threading.Tasks;

namespace TSAIntegracion
{
    public class X509CertificateSignatureFD : IExternalSignature
    {
        private int _idcertificate;
        private int _pin;
        private string _hashAlgorithm;
        private string _encryptionAlgorithm;
        public X509Certificate2 _certificate;
        private readonly IKeyManagerClient _keyManagerClient;
        public X509CertificateSignatureFD(X509Certificate2 certificate, int idcertificate, int pin,
            IKeyManagerClient keyManagerClient)
        {
            _certificate = certificate;
            _hashAlgorithm = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(DigestAlgorithms.SHA256));
            _encryptionAlgorithm = certificate.PublicKey.Key.SignatureAlgorithm;
            _idcertificate = idcertificate;
            _pin = pin;
            _keyManagerClient = keyManagerClient;
        }

        public virtual byte[] Sign(byte[] message)
        {
            //TODO...
            //return ConsumoServicioFirmarHash(_idcertificate, _pin, message);
            Task<byte[]> task = Task.Run(async ()=> await SignHash(new SignHashRequest()
            {
                certificateId = _idcertificate,
                pin = _pin,
                hash = Convert.ToBase64String(message)
            }));
            byte[] bytes = task.Result;
            return bytes;
        }
        private async Task<byte[]> SignHash(SignHashRequest signHashRequest)
        {
            //var response = await _keyManagerClient.SignHash(signHashRequest);
            //return Convert.FromBase64String(response.data);
            return Convert.FromBase64String("MUswGAYJKoZIhvcNAQkDMQsGCSqGSIb3DQEHATAvBgkqhkiG9w0BCQQxIgQgcKOJFm2SDDFdCYb5f08FMXKPwWdHXAjk658Wp4p9oyo=");
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
