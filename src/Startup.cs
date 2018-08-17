using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using dapperOdata.Models;

namespace dapperOdata
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddTransient<Repo>();//which transient, singleton.. etc...
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            builder.EntitySet<BookFast>("Books");

            app.UseHttpsRedirection();
            app.UseMvc(routeBuilder =>
                {// and this line to enable OData query option, for example $filter
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                ///routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());

                routeBuilder.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: builder.GetEdmModel());

                // uncomment the following line to Work-around for #1175 in beta1
                // routeBuilder.EnableDependencyInjection();
                }
            );
        }
    }
}
