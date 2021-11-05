using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal
{
    public class DescifradorTransformador<TId> : ITypeConverter<string, TId>
    {
        public TId Convert(string source, TId destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
                return (TId)System.Convert.ChangeType(0, typeof(TId));

            string llaveEncriptacion = context.Options.Items[nameof(llaveEncriptacion)].ToString();
            bool cifradoEstatico = bool.TryParse(context.Options.Items[nameof(cifradoEstatico)].ToString(), out cifradoEstatico);
            string valorDescifrado = string.Empty;

            if (string.IsNullOrEmpty(llaveEncriptacion)) llaveEncriptacion = this.GetType().Assembly.FullName;
            if (cifradoEstatico)
            {
                valorDescifrado = Cifrador.DescifradoVariable(source, llaveEncriptacion);
            }
            else
            {
                valorDescifrado = Cifrador.DescifradoVariable(source, llaveEncriptacion);
            }

            return (TId)System.Convert.ChangeType(valorDescifrado, typeof(TId));
        }
    }
}
