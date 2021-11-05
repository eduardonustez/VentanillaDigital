using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.Template
{
    public static class TemplateBodyHTML
    {
        private static readonly string ANIO = "Year";
        public static bool ObtenerCuerpoHTML<T>(string templateHtml, T ViewModel, out string value)
        {
            value = default;
            try
            {
                if (ViewModel == null) return false;
                if (!string.IsNullOrEmpty(templateHtml))
                {
                    Type tipoParametro = ViewModel.GetType();
                    PropertyInfo[] datosViewModel = tipoParametro.GetProperties();
                    StringBuilder sb = new StringBuilder(templateHtml);
                    foreach (var item in datosViewModel)
                    {
                        if (templateHtml.Contains(item.Name))
                        {
                            if (item.GetValue(ViewModel) != null)
                                sb = sb.Replace($"[{item.Name}]", item.GetValue(ViewModel).ToString());

                            if (item.Name == ANIO)
                                sb = sb.Replace($"[{item.Name}]", DateTime.Now.Year.ToString());
                        }
                    }
                    value = sb.ToString();
                    return true;
                }
            }
            catch
            {
                throw;
            }
            value = default;
            return false;
        }
    }
}
