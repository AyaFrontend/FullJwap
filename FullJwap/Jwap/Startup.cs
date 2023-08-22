using Jwap.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;
using StackExchange.Redis;

using Jwap.API.Hubs;
using Jwap.BLL.Interfaces;
using Jwap.BLL.Repository;
using Jwap.API.Helper;
using Jwap.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Jwap.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jwap.API.Errors;
using Jwap.API.Middlewares;
using Jwap.BLL.Specfications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http.Features;

namespace Jwap
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
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory =
                actionContext =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Count > 0)
                                  .SelectMany(m => m.Value.Errors)
                                  .Select(e => e.ErrorMessage).ToArray();
                    var responseMessage = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(responseMessage);
                };
            });


            services.AddSingleton<IConnectionMultiplexer>(x =>
            {
                var connection = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(connection);
            });
            services.AddCors(options => options.AddPolicy("chatting",
                builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").AllowCredentials();

                })
            );
            services.AddSignalR().AddMessagePackProtocol();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Jwap", Version = "v1" });
            });
           
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddScoped(typeof(IMessageRepository), typeof(MessageRepository));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IResponseCacheRepository), typeof(ResponseCacheRepository));
            services.AddScoped(typeof(ChatHub), typeof(ChatHub));
            services.AddScoped(typeof(IMailService), typeof(EmailSettings));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddIdentity<User, IdentityRole>(option =>
           { }).AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultDB"));
            }

          ); 
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(
                options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                        ValidateLifetime = true,
                    };

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {



            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jwap v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<WebsocketMiddleware>();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("chatting");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatsocket", options =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                });
            });
           

        }
    }
}

