using System.Web;
using System.Web.Mvc;

namespace ServiciosDistribuidos.CrossPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
