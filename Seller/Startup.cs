using EventBus.Messages.Queue;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Seller.Models;
using Seller.QueueHandlers;
using Seller.Repository;
using System.Reflection;
using MediatR;
using Seller.ServiceHandlers.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Common.Models;

namespace Seller
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
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionsHandler<,>));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seller", Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                        ValidIssuer = Configuration["Token:Issuer"],
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            // Configure Rabbit MQ using Mass Transit
            services.AddMassTransit(c =>
            {
                c.AddConsumer<QueueConsumer>();
                c.UsingRabbitMq((ct, cg) =>
                {
                    cg.Host(Configuration["ServiceBusSettings:HostAddress"]);
                    cg.ReceiveEndpoint(QueueConstant.ProductBidQueue, cn =>
                    {
                        cn.ConfigureConsumer<QueueConsumer>(ct);
                    });
                });
            });

            services.AddMassTransitHostedService();
            services.AddSingleton<IConfiguration>(Configuration);
            //services.Configure<Database>(Configuration.GetSection("eAuctionDatabase"));
            //var builder = WebApplication.CreateBuilder(args);

            // builder.Services.AddDbContext<MvcMovieContext>(options =>
            //   options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext")));

            services.AddDbContext<CoreDbContext>(op => op.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"), options => options.EnableRetryOnFailure()));
            services.AddTransient<IRepository, Repository.Repository>();
           
            services.AddScoped<QueueConsumer>();
            services.AddSingleton<IQueueClient>(x => new QueueClient("ServiceBusConnectionString","from-rabbitmq"));
            //services.AddSingleton<IQueueClient, QueueClient>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seller v1"));
            }
            
            app.UseRouting();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
