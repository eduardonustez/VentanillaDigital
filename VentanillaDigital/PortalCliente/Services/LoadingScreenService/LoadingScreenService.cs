using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.LoadingScreenService
{
    public class LoadingScreenService
    {
        internal static EventHandler<string> ShowEvent { get; set; }
        internal static EventHandler HideEvent { get; set; }
        public void Show(string message)
        {
            if (ShowEvent != null)
                ShowEvent.Invoke(this, message);
        }

        public void Hide()
        {
            if (HideEvent != null)
                HideEvent.Invoke(this, null);
        }
    }
}
