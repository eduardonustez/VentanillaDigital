using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.AgenteReconoser
{
    public class AgenteReconoserModels
    {
    }

    public class AuthResponseModel
    {
        public string TokenAuthTnec { get; set; }

    }

    public class LoginAuthRnec
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class InputUserRnec
    {
        public long IdCliente { get; set; }
        public string IdConvenio { get; set; }
        public string Login { get; set; }
        public string Nombre { get; set; }
        public string IdTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public long IdOficina { get; set; }
        public string Cargo { get; set; }
        public string Area { get; set; }
        public bool Habilitado { get; set; }
    }


    public class InputUserMovilRnec
    {
        public string Login { get; set; }
        public string Correo { get; set; }
        public long IdCliente { get; set; }
        public long IdRol { get; set; }
        public string Nombre { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public long IdOficina { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool Habilitado { get; set; }
        public string Cargo { get; set; }
        public string Area { get; set; }
        public string IdConvenio { get; set; }
    }


    public class InputMachineRnec
    {
        public long IdCliente { get; set; }
        public string IdConvenio { get; set; }
        public long IdOficina { get; set; }
        public string MacEquipo { get; set; }
        public string IpEquipo { get; set; }
        public bool Habilitado { get; set; }
    }

    public class response
    {
        public string Estado { get; set; }
        public string CodigoError { get; set; }
        public string DescripcionError { get; set; }
    }

}
