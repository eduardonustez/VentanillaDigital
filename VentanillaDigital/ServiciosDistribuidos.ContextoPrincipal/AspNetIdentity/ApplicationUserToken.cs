using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity
{
    public class ApplicationUserToken:IdentityUserToken<string>
    {
        public DateTime ExpirationDate { get; set; }
        public bool IsDisabled { get; set; }
    }
}
