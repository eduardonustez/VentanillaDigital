using Microsoft.Extensions.DependencyInjection;

namespace ApiGatewayAdministrador.Policies
{
    public static class PolicyBasedAuthorization
    {
       
        public static void AddPolicies(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminOnly", policy =>
                      policy.RequireRole("Administrador"));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireNotario", policy =>
                {
                    policy.RequireRole("Administrador", "Notario Encargado");
                });
            });
        }
    }
}
