using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SQLClientRepository;
using System;
using System.IO;
using System.Reflection;
using zAutoMapperRepository;
using zAzureClientRepository;
using zFireBaseRepository;
using zFluentAPIRepository;
using zGoogleCloudStorageClient;
using zIdentityServerRepository;
using zLineBotRepository;
using zWebCrawlingRepository;
using zPostgreSQLRepository;
using MongodbClientRepository;

namespace haha
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
         
           //services.AddNeo4jCleint(Configuration.GetSection("Neo4j").Get<Neo4jAuth>());
       
            services.AddMSQLjCleint(Configuration["SQL:connectionstring"]);
            services.AddPostgreSQLClient(Configuration);
            services.AddMongoDBCleint(Configuration);
            services.AddAzureServices();
            services.AddFireBaseService();
            services.AddGoogleStorageService();
            services.AddLineBotAllService();
            services.AddWebCrawlingService();
            services.AddCustomizedVaildator();
            services.AddAutoMapperService();
            services.AddIdentityServices(Configuration["SQL:IdentityConnection"]);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader();
                                  });
            });
            services.AddControllers().AddNewtonsoftJson(options  => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
              
             });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_Resources", Version = "v1", Description ="Connect mutiple service such as CLoudSQL, GoogleStorage, AzuerStorage, FireStore...." });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_Resources");
                c.RoutePrefix = string.Empty;
            });
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
