using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.AgenteReconoser
{
    public interface IAgenteReconoser
    {
        Task<response> registrarUsuarioRnec(InputUserRnec inputUser);
        Task<response> registrarMaquinaRnec(InputMachineRnec inputMachine);
        Task<response> registrarUsuarioMovilesRnec(InputUserMovilRnec inputUser);
    }
}
