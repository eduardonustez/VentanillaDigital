using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
