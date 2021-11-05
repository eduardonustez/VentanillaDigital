using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Infraestructura.PowerBI
{
    public class ReporteService : IReporteService
    {
        private ConfiguracionReportesPowerBI c_configuracionReportesPowerBI;

        public ReporteService(ConfiguracionReportesPowerBI configuracionReportesPowerBI)
        {
            c_configuracionReportesPowerBI = configuracionReportesPowerBI;
        }

        public EmbedParams ObtenerReporteEmbed(string tipoReporte, Guid filtroNotaria, [Optional] Guid additionalDatasetId)
        {
            var embedParams = new EmbedParams();
            try
            {
                var configReport = ConfiguracionReporte(tipoReporte);
                PowerBIClient pbiClient = GetPowerBIClient(tipoReporte);
                var pbiReport = pbiClient.Reports.GetReportInGroup(configReport.WorkspaceId, configReport.ReportId);

                var datasetIds = new List<Guid>
            {
                Guid.Parse(pbiReport.DatasetId)
            };

                if (additionalDatasetId != Guid.Empty)
                {
                    datasetIds.Add(additionalDatasetId);
                }

                var embedReports = new List<EmbedReport>() {
                new EmbedReport
                {
                    ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                }
            };
                var embedToken = GetEmbedToken(configReport.ReportId, datasetIds, configReport.WorkspaceId, pbiClient);

                embedParams = new EmbedParams
                {
                    EmbedReport = embedReports,
                    Type = "Report",
                    EmbedToken = embedToken,
                    Filter = filtroNotaria
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al obtener el reporte.", ex);
            }
            return embedParams;

        }

        private PowerBIClient GetPowerBIClient(string TipoReporte)
        {
            ConfiguracionReporte configReport = ConfiguracionReporte(TipoReporte);
            X509Certificate2 certificate = new X509Certificate2();
            if (configReport.esCertificate)
                certificate = ReadCertificateFromVault(configReport);
            var token = GetAccessToken(TipoReporte, certificate);
            var tokenCredentials = new TokenCredentials(token, "Bearer");
            return new PowerBIClient(new Uri(c_configuracionReportesPowerBI.ApiUrl), tokenCredentials);
        }
        private string GetAccessToken(string tipoReporte, X509Certificate2 certificate)
        {
            try
            {
                var configReport = ConfiguracionReporte(tipoReporte);

                AuthenticationResult authenticationResult;
                if (configReport.esMasterUser)
                {
                    IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(configReport.ApplicationId.ToString())
                        .WithAuthority(c_configuracionReportesPowerBI.AuthorityUrl).Build();
                    var userAccounts = clientApp.GetAccountsAsync().Result;
                    try
                    {
                        authenticationResult = clientApp.AcquireTokenSilent(
                            new List<string>() { c_configuracionReportesPowerBI.ResourceUrl },
                            userAccounts.FirstOrDefault()).ExecuteAsync().Result;
                    }
                    catch (MsalUiRequiredException)
                    {
                        SecureString password = new SecureString();
                        foreach (var key in configReport.Password)
                        {
                            password.AppendChar(key);
                        }
                        authenticationResult = clientApp.AcquireTokenByUsernamePassword(
                            new List<string>() { c_configuracionReportesPowerBI.ResourceUrl },
                            configReport.Username,
                            password).ExecuteAsync().Result;
                    }
                }
                else
                {
                    IConfidentialClientApplication clientApp;
                    var tenantSpecificUrl = c_configuracionReportesPowerBI.AuthorityUrl.Replace("common", configReport.Tenant.ToString());
                    if (configReport.esCertificate)
                    {
                        clientApp = ConfidentialClientApplicationBuilder
                        .Create(configReport.ApplicationId.ToString())
                        .WithCertificate(certificate)
                        .WithAuthority(tenantSpecificUrl)
                        .Build();

                        authenticationResult = clientApp.AcquireTokenForClient(new List<string>() { c_configuracionReportesPowerBI.ResourceUrl }).ExecuteAsync().Result;
                    }
                    else
                    {
                        clientApp = ConfidentialClientApplicationBuilder
                        .Create(configReport.ApplicationId.ToString())
                        .WithClientSecret(Uri.EscapeDataString(configReport.ApplicationSecret))
                        .WithAuthority(tenantSpecificUrl)
                        .Build();

                        authenticationResult = clientApp.AcquireTokenForClient(new List<string>() { c_configuracionReportesPowerBI.ResourceUrl }).ExecuteAsync().Result;
                    }
                }

                return authenticationResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al obtener el token de Azure", ex);
            }
        }

        private EmbedToken GetEmbedToken(Guid reportId,
            IList<Guid> datasetIds
            , [Optional] Guid targetWorkspaceId
            , PowerBIClient pbiClient
            //string tipoReporte
            )
        {
            try
            {
                //PowerBIClient pbiClient = GetPowerBIClient(tipoReporte);

                var tokenRequest = new GenerateTokenRequestV2(

                    reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                    datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                    targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
                );

                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al generar token de acceso embed", ex);
            }
        }

        private ConfiguracionReporte ConfiguracionReporte(string tipoReporte)
        {
            return c_configuracionReportesPowerBI.Reportes.Where(x => x.TipoReporte == tipoReporte).FirstOrDefault();
        }

        private X509Certificate2 ReadCertificateFromVault(ConfiguracionReporte configReport)
        {
            var serviceTokenProvider = new AzureServiceTokenProvider("RunAs=App;");
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));
            CertificateBundle certificate;
            SecretBundle secret;
            try
            {
                certificate = keyVaultClient.GetCertificateAsync($"{configReport.UrlVaultAzure}", configReport.CertifiedName).Result;
                secret = keyVaultClient.GetSecretAsync(certificate.SecretIdentifier.Identifier).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new X509Certificate2(Convert.FromBase64String(secret.Value));
        }
    }
}
