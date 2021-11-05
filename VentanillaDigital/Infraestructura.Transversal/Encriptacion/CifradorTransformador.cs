using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal
{
    public class CifradorTransformador<TId> : ITypeConverter<TId, string>
    {
        public string Convert(TId source, string destination, ResolutionContext context)
        {
            if (source.ToString() == "0") return string.Empty;

            string llaveEncriptacion = context.Options.Items[nameof(llaveEncriptacion)].ToString();
            bool cifradoEstatico = bool.TryParse(context.Options.Items[nameof(cifradoEstatico)].ToString(), out cifradoEstatico);
            string valorCifrado = string.Empty;

            if (string.IsNullOrEmpty(llaveEncriptacion)) llaveEncriptacion = this.GetType().Assembly.FullName;


            if (cifradoEstatico)
            {
                valorCifrado = Cifrador.CifradoEstatico(source.ToString(), llaveEncriptacion);
            }
            else
            {
                valorCifrado = Cifrador.CifradoVariable(source.ToString(), llaveEncriptacion);
            }

            return valorCifrado;
        }
    }
}
