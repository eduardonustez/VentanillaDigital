
using PortalAdministrador.Services.LoadingScreenService;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PortalAdministrador.HttpHandler
{
    public class SpinnerHttpHandler : DelegatingHandler
    {
        private readonly LoadingScreenService _loadingScreen;
        public SpinnerHttpHandler(LoadingScreenService loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var src = new CancellationTokenSource();
            var token = src.Token;
            //La pantalla de carga sólo se muestra para solicitudes que duren más de 150ms
            var delay = Task.Delay(150, token)
                .ContinueWith((x) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        _loadingScreen.Show(string.Empty);
                    }
                }, token);

            var send = base.SendAsync(request, cancellationToken);
            var finish = send.ContinueWith( (x) =>
                {
                    src.Cancel();
                });
            try
            {
                await Task.WhenAll(delay, finish);
            }
            catch { }
            finally
            {
                _loadingScreen.Hide();
            }
            return await send;
        }
    }
}