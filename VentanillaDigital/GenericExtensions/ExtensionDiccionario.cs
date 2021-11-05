using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericExtensions
{
    public static class ExtensionDiccionario
    {

        //public static IDictionary<string, object> ToDictionary(this object source)
        //{​​​​​​​
        //    return source.ToDictionary<object>();
        //}​​​​​​​

        public static Dictionary<string, TValue> ToDictionary<TValue> (this object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var ret = new Dictionary<string, TValue>();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary(prop, source, ret);
            return ret;
        }

        private static void AddPropertyToDictionary<TValue>(PropertyDescriptor prop,
                    object source, Dictionary<string,TValue> dict)
        {
            object value = prop.GetValue(source);
            //if(typeof(TValue).IsAssignableFrom(value.GetType()))
            //{
                dict.Add(prop.Name, (TValue)value);
            //}
            //else if( typeof(TValue).IsAssignableFrom(typeof(string)) )
            //{
            //    dict.Add(prop.Name, (TValue)(value));
            //}
        }
    }
}
