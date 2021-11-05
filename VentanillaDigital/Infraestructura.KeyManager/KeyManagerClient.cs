using Infraestructura.KeyManager.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infraestructura.KeyManager
{
    public class KeyManagerClient : IKeyManagerClient
    {
        private readonly HttpClient _httpClient;
        private readonly UserLoginRequest _userLoginRequest;
        public KeyManagerClient(HttpClient httpClient,UserLoginRequest userLoginRequest)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _userLoginRequest = userLoginRequest ?? throw new ArgumentNullException(nameof(userLoginRequest));
        }
        public async Task<RFDPostResponse> Petition(RFDPostRequest request)
        {
            try
            {
                await GetAuthorization();
                request.Datos.Ciudad = RemoveAccentsWithRegEx(request.Datos.Ciudad);
                request.Datos.Departamento = RemoveAccentsWithRegEx(request.Datos.Departamento);
                var httpResponse = await _httpClient.PostAsJsonAsync("/Gateway/api/v1_0/petition", request);
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RFDPostResponse>(jsonString);
                return response;
            }
            catch(Exception ex)
            {
                return new RFDPostResponse() { Success = false,Mensaje=ex.Message };
            }
        }
        public async Task<GetCertificateResponse> GetFileTypes()
        {
            var httpResponse = await _httpClient.PostAsync("/Gateway/api/v1_0/get-FileTypes",null);
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadFromJsonAsync<GetCertificateResponse>();
            }
            else
            {
                return new GetCertificateResponse() { Success = false };
            }
        }

        public async Task<CertificateStatusResponse> CertificateStatus(string userId,string email)
        {
            await GetAuthorization();
            var user = new { userId = userId,email=email };
            var httpResponse = await _httpClient.PostAsJsonAsync("/api/Certificate/Gateway/api/v1_0/certificate-status", user);
            return await httpResponse.Content.ReadFromJsonAsync<CertificateStatusResponse>();
        }
        public async Task<string> SignDocument(SignDocumentRequest request)
        {
            await GetAuthorization();
            var httpResponse = await _httpClient.PostAsJsonAsync("/api/Sign/Gateway/api/v1_0/SignDocument", request);
            var jsonString = await httpResponse.Content.ReadAsStringAsync();            
            var response = JsonConvert.DeserializeObject<SignDocumentResponse>(jsonString);
            if (response.message == "The file has been signed successfully.")
                return response.data;
            else
                return null;
        }
        public async Task<SignHashResponse> SignHash(SignHashRequest request)
        {
            await GetAuthorization();
            var httpResponse = await _httpClient.PostAsJsonAsync("/api/Sign/Gateway/api/v1_0/SignHash", request);
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<SignHashResponse>(jsonString);
            return response;
        }
        public async Task<string> GetIDType(string abrev)
        {
            switch (abrev)
            {
                case "CC": return "CopiaCedula";
                case "CE": return "CopiaCedulaExtranjeria";
                case "P": return "CopiaPasaporte";
                default: return "CopiaCedula";
            }
        }
        public async Task<X509Certificate2> GetPublicKey(int idCertificate)
        {
            await GetAuthorization();
            var httpResponse = await _httpClient.GetAsync($"/api/KeyManager/get-publickey?Idcertificate={idCertificate}");
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PublicKeyResponse>(jsonString);
            if (response.Success)
            {
                byte[] bytes = Convert.FromBase64String(response.Data);
                return new X509Certificate2(bytes);
            }
            return null;
        }
        public async Task<bool> ChangePin(PinChangeRequest request)
        {
            await GetAuthorization();
            var httpResponse = await _httpClient.PostAsJsonAsync($"/api/KeyManager/Gateway/api/v1_0/ChangePin",request);
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PinChangeResponse>(jsonString);
            return response.Success;
            
        }
        private async Task GetAuthorization()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("/Gateway/api/v1_0/login", _userLoginRequest);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<UserLoginResponse>();
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.JwtToken}");
                }
            }
        }
        private string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }

      
    }
}
