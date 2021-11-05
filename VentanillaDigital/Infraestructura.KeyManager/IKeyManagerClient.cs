using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.KeyManager.Models;

namespace Infraestructura.KeyManager
{
    public interface IKeyManagerClient
    {
        //Task<GenerateKeyResponse> GenerateKey(GenerateKeyRequest request);
        //Task<UserLoginResponse> Login(UserLoginRequest request);
        Task<RFDPostResponse> Petition(RFDPostRequest request);
        Task<GetCertificateResponse> GetFileTypes();
        Task<CertificateStatusResponse> CertificateStatus(string userId,string email);
        Task<bool> ChangePin(PinChangeRequest request);
        Task<string> SignDocument(SignDocumentRequest request);
        Task<SignHashResponse> SignHash(SignHashRequest request);
        Task<string> GetIDType(string abrev);
        Task<X509Certificate2> GetPublicKey(int idCertificate);


    }
}
