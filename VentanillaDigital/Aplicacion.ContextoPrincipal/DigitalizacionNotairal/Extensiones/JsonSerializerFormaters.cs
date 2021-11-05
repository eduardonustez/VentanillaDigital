using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Extensiones
{
    public static class JsonSerializerFormaters
    {
        public static string ToSerialize<T>(this T model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented, jsonSerializerSettings);
        }

        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}