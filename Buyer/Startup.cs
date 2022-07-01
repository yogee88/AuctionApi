using Buyer.Models;
using Buyer.Repository;
using EventBus.Messages.Queue;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyer.QueueConsumer;

namespace Buyer
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Buyer", Version = "v1" });
            });           

            // Configure Rabbit MQ using Mass Transit
            services.AddMassTransit(c =>
            {
                c.AddConsumer<QueueConsumer.QueueConsumer>();
                c.UsingRabbitMq((ct, cg) =>
                {
                    cg.Host(Configuration["ServiceBusSettings:HostAddress"]);

                    cg.ReceiveEndpoint(QueueConstant.ProductBuyerQueue, cn =>
                    {
                        cn.ConfigureConsumer<QueueConsumer.QueueConsumer>(ct);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.Configure<Database1>(Configuration.GetSection("eAuctionDatabase"));
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddScoped<QueueConsumer.QueueConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Buyer v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
