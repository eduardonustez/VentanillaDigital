using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public enum EnumDedos
    {
        PulgarDerecho = 1,
        IndiceDerecho=2,
        CorazonDerecho = 4,
        AnularDerecho =8,
        MeñiqueDerecho = 16,
        PulgarIzquierdo = 32,
        IndiceIzquierdo = 64,
        CorazonIzquierdo = 128,
        AnularIzquierdo = 256,
        MeñiqueIzquierdo = 512
    }
}
