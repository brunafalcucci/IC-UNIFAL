using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Certificate;
using Application.Context;
using Microsoft.EntityFrameworkCore;
using Application.Repositories;

namespace Application
{
    public class Startup
    {
        private readonly static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static void ConfigureServices(IServiceCollection service)
        {
            service.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate();

            service.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            service.AddControllers();

            /*-------------------------------- DBCONTEXT --------------------------------*/
            string connectionString = "Server=db.iecxgqiqfqncxnwfhaxh.supabase.co;Port=5432;User Id=postgres;Password=ICUnifal2023;Database=postgres";
            service.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            service.AddScoped<ICriticalityIndexRepository, CriticalityIndexRepository>();
            service.AddScoped<IChemicalTreatmentRepository, ChemicalTreatmentRepository>();
            service.AddScoped<IMechanicalTreatmentRepository, MechanicalTreatmentRepository>();
            service.AddScoped<IWasteEnergyUseRepository, WasteEnergyUseRepository>();
            service.AddScoped<IMotorRepository, MotorRepository>();
            service.AddScoped<IHeatingRepository, HeatingRepository>();
            service.AddScoped<IAirCompressorRepository, AirCompressorRepository>();
            service.AddScoped<ICoolingSystemRepository, CoolingSystemRepository>();
            service.AddScoped<ILightingSystemRepository, LightingSystemRepository>();
            service.AddScoped<ICondenserRepository, CondenserRepository>();
            service.AddScoped<IBoilerRepository, BoilerRepository>();
            service.AddScoped<IAirConditioningRepository, AirConditioningRepository>();
            service.AddScoped<IVentilationRepository, VentilationRepository>();
        }

        public static void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}