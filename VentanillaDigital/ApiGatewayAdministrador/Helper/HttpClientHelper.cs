using ApiGateway.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Helper
{
    public class HttpClientHelper : IHttpClientHelper
    {
        HttpContext _context;
        IConfiguration _configuration;
        IHttpClientFactory _clientFactory;
        public HttpClientHelper(IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _context = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            string json = JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> ConsumirServicioRest<T>(string serviceUrl, HttpMethod Metodo,
            T content)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ClaimModel claimModel = GetClaims();
            //string uri = serviceUrl;
            string uri = claimModel == null ? serviceUrl : GetURI(serviceUrl, claimModel.UserId.ToString());
            string token = "";

            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                var request = new HttpRequestMessage(Metodo, uri);
                if (!string.IsNullOrEmpty(content.ToString()))
                {
                    string json = JsonConvert.SerializeObject(content);
                    dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(json);
                    obj.transaccionGuid = Guid.NewGuid();

                    if (claimModel != null)
                    {
                        //obj.userId = claimModel.UserId;
                        obj.userName = claimModel.UserName;
                        //obj.notariaId = claimModel.NotariaId;
                        client.DefaultRequestHeaders.Add("username", claimModel.UserName);
                    }

                    HttpContent httpContent = CreateHttpContent(obj);
                    request.Content = httpContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    string username = "usu_ventanilla_pru";
                    string password = "8474LSJ$1a";
                    string unencodedUsernameAndPassword = string.Format("{0}:{1}", username, password);
                    byte[] unencodedBytes = ASCIIEncoding.ASCII.GetBytes(unencodedUsernameAndPassword);
                    string base64UsernameAndPassword = System.Convert.ToBase64String(unencodedBytes);
                    string authorizationHeaderValue = string.Format("Basic {0}", base64UsernameAndPassword);
                    client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

                }

                StringValues direccionIp = "";
                StringValues direccionMac = "";
                _context.Request.Headers.TryGetValue("ipAddress", out direccionIp);
                _context.Request.Headers.TryGetValue("macAddress", out direccionMac);

                client.DefaultRequestHeaders.Add("ipAddress", direccionIp.ToString());
                client.DefaultRequestHeaders.Add("macAddress", direccionMac.ToString());

                if (!string.IsNullOrEmpty(token))
                {
                    client.SetBearerToken(token);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                HttpResponseMessage asyncRes = await client.SendAsync(request);

                if (asyncRes.Headers.Contains("x-total-notificaciones"))
                {
                    _context.Response.Headers.Add("x-total-notificaciones", asyncRes.Headers.GetValues("x-total-notificaciones").FirstOrDefault() ?? "");
                }

                if (asyncRes.Headers.Contains("x-current-page"))
                {
                    _context.Response.Headers.Add("x-current-page", asyncRes.Headers.GetValues("x-current-page").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-items-per-page", asyncRes.Headers.GetValues("x-items-per-page").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-total-items", asyncRes.Headers.GetValues("x-total-items").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-total-pages", asyncRes.Headers.GetValues("x-total-pages").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("Access-Control-Expose-Headers", new string[] { "x-current-page", "x-items-per-page", "x-total-items", "x-total-pages" });
                }

                return asyncRes;
            }
        }


        private string GetURI(string uri, string userId)
        {
            var param = new Dictionary<string, string>() { { "userId", userId }, { "token", GetToken() } };
            var newUrl = new Uri(QueryHelpers.AddQueryString(uri, param));

            return newUrl.AbsoluteUri.ToString();
        }

        public string GetToken()
        {
            string token = null;

            if (_context.Request.Headers.ContainsKey("Authorization") &&
                 _context.Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                token = _context.Request.Headers["Authorization"][0].Substring("Bearer ".Length).Trim();
            }

            return token;
        }

        private ClaimsPrincipal GetPrincipalFromToken()
        {
            string token = GetToken();
            if (token == null) return null;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Tokens:Key"))),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public ClaimModel GetClaims()
        {
            ClaimsPrincipal claimsPrincipal = GetPrincipalFromToken();
            if (claimsPrincipal == null)
                return null;
            ClaimModel claimModel = new ClaimModel()
            {
                UserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = claimsPrincipal.FindFirstValue(ClaimTypes.Name),
                Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
                NotariaId = claimsPrincipal.FindFirstValue("NotariaId") != null ? Convert.ToInt64(claimsPrincipal.FindFirstValue("NotariaId")) : 0
            };

            return claimModel;
        }
    }
}
