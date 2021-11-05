using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorQR
{
    public interface IGeneradorQR
    {
        string StringToQR(string qrString);
    }
}
