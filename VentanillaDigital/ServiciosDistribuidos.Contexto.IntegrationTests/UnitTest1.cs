using Aplicacion.ContextoPrincipal.Modelo.Evento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ServiciosDistribuidos.Contexto.IntegrationTests
{
    public class UnitTest1 : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;
        public UnitTest1(TestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _client = fixture.Client;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Test1()
        {
            // Arrange
           
            // Act:
            List<int> vecesQueAparece = new List<int>();
            for(int i = 0; i < 100; i++)
            {
                var request = new HttpRequestMessage(
               HttpMethod.Get, "/Patrocinador/ObtenerPatrocinadores");
                var response = await _client.SendAsync(request);
                var res = await response.Content.ReadAsStringAsync();
                IEnumerable<PatrocinadorForReturnDTO> patrocinadores = JsonConvert.DeserializeObject<IEnumerable<PatrocinadorForReturnDTO>>(res);
                foreach(PatrocinadorForReturnDTO patrocinadorDTO in patrocinadores)
                {
                    vecesQueAparece.Add(patrocinadorDTO.PatrocinadorId);
                }
            }

            // Assert:

           
            Assert.InRange(vecesQueAparece.Where(x => x == 11).Count(), 0,100);
       
            _testOutputHelper.WriteLine("Master:" + vecesQueAparece.Where(x => x == 2).Count());
            _testOutputHelper.WriteLine("Ultra:" + vecesQueAparece.Where(x => x == 3).Count());
            _testOutputHelper.WriteLine("Premier:" + vecesQueAparece.Where(x => x == 4).Count());
            _testOutputHelper.WriteLine("Platino:" + vecesQueAparece.Where(x => x == 11).Count());
            _testOutputHelper.WriteLine("Single:" + vecesQueAparece.Where(x => x == 5).Count());

        }
    }
}
