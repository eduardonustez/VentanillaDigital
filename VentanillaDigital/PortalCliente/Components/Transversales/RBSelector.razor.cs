using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Components.Transversales
{
    public partial class RBSelector 
    {
        [Parameter]
        public bool ExpandMultiple { get; set; } = true;

        [Parameter]
        public InitialExpandState InitialState { get; set; }


        private object[][] _categoriasArr;
        private object[] _categorias;

        [Parameter]
        public object[] Categorias { 
            get => _categorias; 
            set 
            { 
                _categorias = value;
                _categoriasArr = _categorias?.Select(c => Opciones(c).ToArray()).ToArray(); 
            } 
        }

        private Func<object, IEnumerable<object>> _opciones = x => (IEnumerable<object>)x;

        [Parameter]
        public Func<object, IEnumerable<object>> Opciones
        {
            get => _opciones;
            set
            {
                _opciones = value;
                _categoriasArr = _categorias?.Select(c => _opciones(c).ToArray()).ToArray();
            }
        }

        [Parameter]
        public String Name { get; set; } = "sel"+Guid.NewGuid().ToString().Replace("-","");

        [Parameter]
        public String Class { get; set; }


        [Parameter]
        public Func<object, String> EtiquetaElemento { get; set; } = (x) => x.ToString();

        [Parameter]
        public Func<object, String> EtiquetaCategoria { get; set; } = (x) => x.ToString();

        [Parameter]
        public object Seleccion { get; set; }

        [Parameter]
        public EventCallback<object> SeleccionChanged { get; set; }

        private async Task SeleccionEvent(ChangeEventArgs args)
        {
            string[] aux = ((String)args.Value).Split(';');
            int cat = int.Parse(aux[0])-1;
            int opt = int.Parse(aux[1])-1;
            Seleccion = _categoriasArr[cat][opt];
            await SeleccionChanged.InvokeAsync(Seleccion);
        }

        public enum InitialExpandState
        {
            ExpandAll,
            RetractAll,
            ExpandFirst
        }

    }
}
