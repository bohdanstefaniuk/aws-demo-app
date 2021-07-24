using ImagesApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImagesApp
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<DatabaseContext>(options =>
			{
				options.UseNpgsql(_configuration.GetValue<string>("AWS:RDSConnectionString"));
			});

			services.AddSwaggerGen();
			services.Configure<AwsSettings>(_configuration.GetSection("AWS"));
			services.AddTransient<MetadataRepository>();
			services.AddTransient<S3Repository>();
			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context)
		{
			context.Database.Migrate();
			app.UseDeveloperExceptionPage();
			app.UseRouting();
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
			});
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}